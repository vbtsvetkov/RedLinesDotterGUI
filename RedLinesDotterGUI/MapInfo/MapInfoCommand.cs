using MapInfoWrap;
using RedLinesDotterGUI.Services;
using System.Collections.Generic;
using System.Text;

namespace RedLinesDotterGUI.MapInfo
{
    /// <summary>
    /// Взамидействие с МапИнфо 
    /// </summary>
    internal static class MapInfoCommand
    {
        private static readonly IApplicationCommand app =
            MapApplication.Instance.App;
        private static readonly StringBuilder builder =
           new StringBuilder();
        private const string objectName = "currectObject";

        /// <summary>
        /// Выбирает первый объект таблицы
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        internal static void FetchFirstObject(string tableName)
        {
            var commandList = new List<string>
            {
                "Dim currectObject As Object",
                "Fetch First From " + tableName,
                "currectObject = " + tableName + ".Obj"
            };

            DoMany(commandList);
        }

        #region ObjectInfo
        /// <summary>
        /// Возвращает идентификатор типа объекта
        /// </summary>
        /// <returns>
        /// 4 - полилиния
        /// 7 - полигон
        /// </returns>
        internal static int GetObjectType()
        {
            return app.Eval(objectName.ObjectInfo(1)).ToInt();
        }

        /// <summary>
        /// Возвращает количество точек объекта
        /// </summary>
        /// <returns></returns>
        internal static int GetDotsAmount()
        {
            return app.Eval(objectName.ObjectInfo(20)).ToInt();
        }

        /// <summary>
        /// Возвращает количество точек в конкретном полигоне
        /// </summary>
        /// <param name="polygonIndex">Индекс полигона</param>
        /// <returns></returns>
        internal static int GetPolygonDotsAmount(int polygonIndex)
        { 
            int baseIndex = 21;
            return app.Eval(objectName.ObjectInfo(baseIndex + polygonIndex)).ToInt();
        }

        /// <summary>
        /// Возаращает количество полигонов объекта
        /// </summary>
        /// <returns></returns>
        internal static int GetPolygonsAmount()
        {
            return app.Eval(objectName.ObjectInfo(21)).ToInt();
        }
        #endregion

        #region Coordinates
        /// <summary>
        /// Возвращает координату Х конкретной точки
        /// </summary>
        /// <param name="dotNumber">Номер точки</param>
        /// <param name="polygonNumber">Номер полигона</param>
        /// <returns></returns>
        internal static double GetXCoordinate(int dotNumber, int polygonNumber)
        {
            var command = GetNode(objectName, "X", dotNumber, polygonNumber);

            return app.Eval(command).ToDouble();
        }

        /// <summary>
        /// Возвращает координату Y конкретной точки
        /// </summary>
        /// <param name="dotNumber">Номер точки</param>
        /// <param name="polygonNumber">Номер полигона</param>
        /// <returns></returns>
        internal static double GetYCoordinate(int dotNumber, int polygonNumber)
        {
            var command = GetNode(objectName, "Y", dotNumber, polygonNumber);

            return app.Eval(command).ToDouble();
        }
        #endregion

        #region StringBuilder
        private static string GetNode(this string objectName, string axis, int dotNumber, int polygonNumber)
        {
            builder.Append("ObjectNode");
            builder.Append(axis);
            builder.Append("(");
            builder.Append(objectName);
            builder.Append(", ");
            builder.Append(polygonNumber);
            builder.Append(", ");
            builder.Append(dotNumber);
            builder.Append(")");

            var result = builder.ToString();
            builder.Clear();

            return result;
        }

        private static string ObjectInfo(this string objectName, int attributeId)
        {
            builder.Append("ObjectInfo(");
            builder.Append(objectName);
            builder.Append(", ");
            builder.Append(attributeId);
            builder.Append(")");

            string result = builder.ToString();
            builder.Clear();

            return result;
        }
        #endregion

        #region Executor
        private static void DoMany(List<string> commandList)
        {
            foreach (var command in commandList)
            {
                app.Do(command);
            }
        }
        #endregion
    }
}
