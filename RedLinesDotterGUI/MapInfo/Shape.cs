using MapInfoWrap;
using RedLinesDotterGUI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedLinesDotterGUI.MapInfo
{
    internal abstract class Shape
    {
        protected static readonly IApplicationCommand app =
            MapApplication.Instance.App;

        protected List<PolygonPoint> Points { get; set; }
        protected readonly int _dotsAmount;

        internal Shape(int dotsAmount)
        {
            _dotsAmount = dotsAmount;
            Points = new List<PolygonPoint>();
        }

        internal virtual void FillPoints()
        {
            for (int i = 1; i <= _dotsAmount; i++)
            {
                Points.Add(GetPoint(i));
            }
        }

        internal virtual PolygonPoint GetPoint(int dotNumber, int polygonNumber = 1)
        {
            double x = MapInfoCommand.GetXCoordinate(dotNumber, polygonNumber);
            double y = MapInfoCommand.GetYCoordinate(dotNumber, polygonNumber);

            return new PolygonPoint(x, y);
        }

        #region AsyncMethods

        internal virtual async Task FillPointsAsync()
        {
            await Task.Run(() => {
                for (int i = 1; i <= _dotsAmount; i++)
                {
                    Points.Add(GetPoint(i));
                }
            }).ConfigureAwait(false);
        }
            

        #endregion
    }
}
