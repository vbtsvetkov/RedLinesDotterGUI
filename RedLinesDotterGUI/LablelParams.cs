using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedLinesDotterGUI
{
    public sealed  class LablelParams
    {
        /// <summary>
        /// Высота текстового фрейма
        /// </summary>
        public double BoxHeight { get; set; }

        /// <summary>
        /// Ширина текстового фрейма
        /// </summary>
        public double BowWidth { get; set; }

        /// <summary>
        /// Высота выноса фрейма
        /// </summary>
        public double LabelHeight { get; set; }

        public LablelParams(double boxHeight, double boxWidth, double labelHeight)
        {
            BoxHeight = boxHeight;
            BowWidth = boxWidth;
            LabelHeight = labelHeight;
        }
    }
}
