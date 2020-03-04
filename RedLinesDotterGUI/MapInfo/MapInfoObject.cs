using MapInfoWrap;
using System;
using System.Diagnostics.Contracts;

namespace RedLinesDotterGUI.MapInfo
{
    public sealed class MapInfoObject
    {
        private readonly Table table;


        internal Shape CurrentShape { get; private set; }

        public MapInfoObject(Table table)
        {
            Contract.Requires(table != null);
            this.table = table;

            MapInfoCommand.FetchFirstObject(table.Name);
        }

        internal void GetFirstObject()
        {
            CurrentShape = CreateShape();
            CurrentShape.FillPoints();
        }

        private static Shape CreateShape()
        {
            var objectType = MapInfoCommand.GetObjectType();
            var dotsAmount = MapInfoCommand.GetDotsAmount();

            // Объет является полилинией
            if (objectType == 4)
                return new MapPolyline(dotsAmount);
            // Объект является полигоном
            else if (objectType == 7)
            {
                var polygonsAmount = MapInfoCommand.GetPolygonsAmount();
                if (polygonsAmount == 1)
                    return new MapPolygon(dotsAmount);
                // Объект содержит много полигонов
                else if (polygonsAmount > 1)
                    return new MapMultyPolygon(polygonsAmount, dotsAmount);
                else
                    throw new Exception("Некорректное значение числа полигонов");
            }
            else
                throw new Exception("Не удалось создать объект Shape");
        }
    }
}
