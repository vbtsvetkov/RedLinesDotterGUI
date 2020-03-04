using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedLinesDotterGUI.MapInfo
{
    internal sealed class MapMultyPolygon : MapPolygon
    {
        private readonly int _polygonsAmount;
        private readonly Dictionary<int, int> polygonsDotsdAmount;

        internal MapMultyPolygon(int polygonsAmount, int dotsAmount) : base (dotsAmount)
        {
            _polygonsAmount = polygonsAmount;
            polygonsDotsdAmount = new Dictionary<int, int>();
        }

        internal override void FillPoints()
        {
            GetPolygonsDotsAmount();

            foreach (var polygon in polygonsDotsdAmount.OrderBy(p => p.Key))
            {
                FillPoints(polygon.Value, polygon.Key);
            }
        }

        internal async Task FillPointsAsync()
        {
            await GetPolygonsDotsAmountAsync().ConfigureAwait(false);
            
            await Task.Run(() => {
                foreach (var polygon in polygonsDotsdAmount.OrderBy(p => p.Key))
                {
                    FillPoints(polygon.Value, polygon.Key);
                }
            }).ConfigureAwait(false);
        }

        private void FillPoints(int dotsAmount, int polygonIndex)
        {
            for (int dotIndex = 1; dotIndex <= dotsAmount; dotIndex++)
            {
                Points.Add(base.GetPoint(dotIndex, polygonIndex));
            }
        }

        private async Task FillPointsAsync(int dotsAmount, int polygonIndex)
        {
            await Task.Run(() => {
                for (int dotIndex = 1; dotIndex <= dotsAmount; dotIndex++)
                {
                    Points.Add(base.GetPoint(dotIndex, polygonIndex));
                }
            }).ConfigureAwait(false);
        }

        private void GetPolygonsDotsAmount()
        {
            for (int polygonIndex = 1; polygonIndex <= _polygonsAmount; polygonIndex++)
            {
                var dotsAmount = MapInfoCommand.GetPolygonDotsAmount(polygonIndex);
                polygonsDotsdAmount.Add(polygonIndex, dotsAmount);
            }
        }

        private async Task GetPolygonsDotsAmountAsync()
        {
            Points.Clear();
            polygonsDotsdAmount.Clear();
            await Task.Run(() => {
                for (int polygonIndex = 1; polygonIndex <= _polygonsAmount; polygonIndex++)
                {
                    var dotsAmount = MapInfoCommand.GetPolygonDotsAmount(polygonIndex);
                    polygonsDotsdAmount.Add(polygonIndex, dotsAmount);
                }
            }).ConfigureAwait(false);
        }
    }
}
