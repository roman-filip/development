using System;

namespace Puzzlers
{
    public class AreaPuzzler
    {
        private int _areaStartIndex;

        public int CalculateArea(int[] bars)
        {
            if (bars == null || bars.Length < 3)
            {
                return 0;
            }

            _areaStartIndex = 0;
            var totalArea = 0;
            while (_areaStartIndex < bars.Length)
            {
                totalArea += CalculateSingleArea(bars);
            }

            return totalArea;
        }

        private int CalculateSingleArea(int[] bars)
        {
            var area = 0;

            var leftBorderIndex = GetLeftBorderIndex(bars, _areaStartIndex);
            var leftBorderHeight = bars[leftBorderIndex];

            var rightBorderIndex = GetRightBorderIndex(bars, leftBorderIndex + 1);
            var rightBorderHeight = bars[rightBorderIndex];

            var lowerBorderHeiht = Math.Min(leftBorderHeight, rightBorderHeight);

            for (int i = leftBorderIndex + 1; i < rightBorderIndex; i++)
            {
                area += lowerBorderHeiht - bars[i];
            }

            _areaStartIndex = rightBorderIndex + 1;

            return area;
        }

        private int GetLeftBorderIndex(int[] bars, int startIndex)
        {
            for (int i = startIndex; i < bars.Length - 1; i++)
            {
                if (bars[i] > bars[i + 1])
                {
                    return i;
                }
            }

            return bars.Length - 1;
        }

        private int GetRightBorderIndex(int[] bars, int startIndex)
        {
            for (int i = startIndex; i < bars.Length - 1; i++)
            {
                if ((bars[i] > bars[i - 1]) && (bars[i] > bars[i + 1]))
                {
                    return i;
                }
            }

            return bars[bars.Length - 1] < bars[bars.Length - 2] ? startIndex : bars.Length - 1;
        }
    }
}