using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RedLinesDotterGUI.Helpers;
using RedLinesDotterGUI.MapInfo;
using RedLinesDotterGUI.Services;
using RedLinesDotterGUI.ViewModels;

namespace RedLinesDotterGUI.Views
{
    /// <summary>
    /// Interaction logic for DotterPane.xaml
    /// </summary>
    public partial class DotterPane : UserControl
    {
        private TablesViewModel tablesViewModel;

        public DotterPane()
        {
            InitializeComponent();
            tablesViewModel = new TablesViewModel();
            DataContext = tablesViewModel;
        }

        #region ListBox
        private void tablesList_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            MapApplication.Instance.App.RefreshTables();
            tablesViewModel.SetTables();

            var tabName = tablesViewModel.SelectedTable.Name;
            var table = MapApplication.Instance.App.GetTable(tabName);

            MapInfoObject mapInfoObject = new MapInfoObject(table);
            mapInfoObject.GetFirstObject();

            var newobj = (MapMultyPolygon)mapInfoObject.CurrentShape;
            newobj.FillPointsAsync().ConfigureAwait(false);
            

            //var pointCollector = new PointCollector();
            //pointCollector.GetFirstObject(tabName);

            //var pointsTypeTuple = GetPoints();
            //var middlePoint = GetMiddlePoint(pointsTypeTuple.points);
            //var points = ChangeCoordynateSystem(pointsTypeTuple.points, middlePoint);
            //var dimensions = GetLinearDimensions(points);
            //var coeff = Coefficient(dimensions.VerticalDimension, dimensions.HorizontalDimension);
            //var expandedPoints = ExpandPoints(points, coeff);
            //var centredPoints = GetCanvasCS(expandedPoints);
            //CreateLines(centredPoints);
        }

        /// <summary>
        /// Возвращает точки и тип первого объекта выбранного слоя
        /// </summary>
        /// <returns></returns>
        private (int objectType, List<Point> points) GetPoints()
        {
            var app = MapApplication.Instance.App;
            var table = app.GetTable(tablesViewModel.SelectedTable.Name);

            app.Do("Dim firstObj As Object");
            app.Do("Fetch First From " + table.Name);
            app.Do("firstObj = " + table.Name + ".Obj");

            var objType = Convert.ToInt32(app.Eval("ObjectInfo(firstObj, 1)"));
            var dotsQty = Convert.ToInt32(app.Eval("ObjectInfo(firstObj, 20)"));

            /// TODO: Проверить тип объекта. Если это не полилилиния или регион - вбросить исключение.


            var points = new List<Point>();

            for (int i = 1; i <= dotsQty; i++)
            {
                var point = GetPoint(i);
                points.Add(point);
            }

            return (objType, points);
        }

        /// <summary>
        /// По заданным точкам вычисляет центральную точку минимального
        /// фрейма объекта
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private Point GetMiddlePoint(List<Point> points)
        {
            var xmin = points.Min(p => p.X);
            var xmax = points.Max(p => p.X);

            var ymin = points.Min(p => p.Y);
            var ymax = points.Max(p => p.Y);

            return new Point((xmax + xmin) / 2, (ymax + ymin) / 2);
        }

        /// <summary>
        /// Возвращает i-ую точку объекта
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Point GetPoint(int i)
        {
            var app = MapApplication.Instance.App;
            // MapInfo возвращает строку. В случае с дробными разделеителем является точка.
            string xcoord = app.Eval("ObjectNodeX(firstObj, 1, " + i + ")");
            string ycoord = app.Eval("ObjectNodeY(firstObj, 1, " + i + ")");

            double x = Convert.ToDouble(xcoord, System.Globalization.CultureInfo.InvariantCulture);
            double y = Convert.ToDouble(ycoord, System.Globalization.CultureInfo.InvariantCulture);

            return new Point(x, y);
        }

        /// <summary>
        /// Меняет систему координат на системы с центром в центральной точке
        /// </summary>
        /// <param name="points">Точки с координатами из мапинфо</param>
        /// <param name="middlePoint">центральная точка фрейма</param>
        /// <returns>Список с новыми координатами</returns>
        /// <remarks>Это сделано для переноса координатной системы в центр минимального фрейма объекта</remarks>
        private List<Point> ChangeCoordynateSystem(List<Point> points, Point middlePoint)
        {
            return points.Select(p => new Point((p.X - middlePoint.X), (p.Y - middlePoint.Y))).ToList();
        }

        /// <summary>
        /// Линейные размеры фрейма
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private (double VerticalDimension, double HorizontalDimension) GetLinearDimensions (List<Point> points)
        {
            double h = points.Max(p => p.X) - points.Min(p => p.X);
            double v = points.Max(p => p.Y) - points.Min(p => p.Y);

            return (v, h);
        }

        /// <summary>
        /// Возвращает минимальный 
        /// </summary>
        /// <param name="verticalDimension">Вертикальный размер фрейма</param>
        /// <param name="HorizontalDimension">Горизонтальный размер фрейма</param>
        private double Coefficient(double verticalDimension, double HorizontalDimension)
        {
            // Минутка хардкода размеров канваса в методе.
            double verticalCoeff = 350 / verticalDimension;
            double horizontalCoeff = 360 / HorizontalDimension;

            return (verticalCoeff > horizontalCoeff) ? horizontalCoeff : verticalCoeff;
        }

        private List<Point> ExpandPoints(List<Point> points, double coeff)
        {
            return points.Select(p => new Point(p.X * coeff, p.Y * coeff)).ToList();
        }

        private List<Point> GetCanvasCS(List<Point> points)
        {
            // И снова хардкод от размеров канваса.
            double Xc = 180.0;
            double Yc = 175.0;
            return points.Select(p => new Point((p.X + Xc), (-p.Y + Yc))).ToList();
        }

        private void CreateLines(List<Point> points)
        {
            Canvas.Children.Clear();
            Polyline polyline = new Polyline();
            polyline.Points = new PointCollection();
            foreach (var point in points)
            {
                polyline.Points.Add(point);
            }
            polyline.Stroke = Brushes.Black;
            Canvas.Children.Add(polyline);
        }
        #endregion

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var bH = Convert.ToDouble(this.boxHeight.Text);
            var bW = Convert.ToDouble(this.boxWidth.Text);
            var lH = Convert.ToDouble(this.labelHeight.Text);

            var parametres = new LablelParams(bH, bW, lH);

            JsonWriter.Write(parametres);
        }

        private void boxHeight_Up_Click(object sender, RoutedEventArgs e)
        {
            double value = Convert.ToDouble(this.boxHeight.Text);
            value += 0.0001;
            this.boxHeight.Text = value.ToString();
        }

        private void boxHeight_Down_Click(object sender, RoutedEventArgs e)
        {
            double value = Convert.ToDouble(this.boxHeight.Text);
            value -= 0.0001;
            this.boxHeight.Text = value.ToString();
        }

        private void boxWidth_Up_Click(object sender, RoutedEventArgs e)
        {
            double value = Convert.ToDouble(this.boxWidth.Text);
            value += 0.0001;
            this.boxWidth.Text = value.ToString();
        }

        private void boxWidth_Down_Click(object sender, RoutedEventArgs e)
        {
            double value = Convert.ToDouble(this.boxWidth.Text);
            value -= 0.0001;
            this.boxWidth.Text = value.ToString();
        }

        private void labelHeight_Up_Click(object sender, RoutedEventArgs e)
        {
            double value = Convert.ToDouble(this.labelHeight.Text);
            value += 0.0001;
            this.labelHeight.Text = value.ToString();
        }

        private void labelHeight_Down_Click(object sender, RoutedEventArgs e)
        {
            double value = Convert.ToDouble(this.labelHeight.Text);
            value -= 0.0001;
            this.labelHeight.Text = value.ToString();
        }
    }
}
