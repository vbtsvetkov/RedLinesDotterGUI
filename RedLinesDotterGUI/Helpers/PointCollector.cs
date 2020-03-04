using RedLinesDotterGUI.Services;
using RedLinesDotterGUI.ViewModels;
using RedLinesDotterGUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedLinesDotterGUI.Helpers
{
    public sealed class PointCollector
    {
        public Figure Figure { get; private set; }
        

        public void GetFirstObject(string tableName)
        {
            var app = MapApplication.Instance.App;
            var table = app.GetTable(tableName);

            app.Do("Dim firstObj As Object");
            app.Do("Fetch First From " + table.Name);
            app.Do("firstObj = " + table.Name + ".Obj");

            CreateFigure();

            var dotsQty = Convert.ToInt32(app.Eval("ObjectInfo(firstObj, 20)"));
            var polygonQty = Convert.ToInt32(app.Eval("ObjectInfo(firstObj, 21)"));

            var polygonsAmount = new Dictionary<int, int>();

            for (int i = 1; i <= polygonQty; i++)
            { 
                string currentPolygon = (21 + i).ToString();
                int dotsInPolygon = Convert.ToInt32(app.Eval("ObjectInfo(firstObj, " + currentPolygon +")"));

                polygonsAmount.Add(i, dotsInPolygon);
            }

            int polygonNumber = 1;

            for (int i = 1; i <= dotsQty; i++)
            {
                try
                {
                    var point = GetPoint(i, polygonNumber);
                    Figure.Add(point, polygonNumber);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    polygonNumber++;
                    var point = GetPoint(i, polygonNumber);
                    Figure.Add(point, polygonNumber);
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Возвращает i-ую точку объекта
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Point GetPoint(int i, int polygonNumber)
        {
            var app = MapApplication.Instance.App;
            // MapInfo возвращает строку. В случае с дробными разделеителем является точка.
            try
            {
                string xcoord = app.Eval("ObjectNodeX(firstObj, " + polygonNumber + ", " + i + ")");
                string ycoord = app.Eval("ObjectNodeY(firstObj, " + polygonNumber + ", " + i + ")");

                double x = Convert.ToDouble(xcoord, System.Globalization.CultureInfo.InvariantCulture);
                double y = Convert.ToDouble(ycoord, System.Globalization.CultureInfo.InvariantCulture);

                return new Point(x, y);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private FigureType GetFigureType()
        { 
            int objTypeId = Convert.ToInt32(MapApplication.Instance.App.Eval("ObjectInfo(firstObj, 1)"));
            if (objTypeId == 4)
                return FigureType.Polyline;
            else if (objTypeId == 7)
                return FigureType.Polygon;
            else
                throw new Exception("Не подходящий объект");
        }

        private void CreateFigure()
        {
            var type = GetFigureType();
            Figure = new Figure(type);
        }

    }
}
