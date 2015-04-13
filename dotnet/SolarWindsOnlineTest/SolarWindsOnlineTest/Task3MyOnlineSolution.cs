using System.Linq;

namespace SolarWindsOnlineTest
{
    public class Task3MyOnlineSolution
    {
        public int solution(int[] A, int[] B)
        {
            // write your code in C# 5.0 with .NET 4.5 (Mono)
            var orderedA = A.OrderBy(value => value).ToList();
            var orderedB = B.OrderBy(value => value).ToList();
            var indexB = 0;

            for (int aIndex = 0; aIndex < orderedA.Count; aIndex++)
            {
                for (int bIndex = 0; bIndex < orderedB.Count; bIndex++)
                {
                    if (orderedA[aIndex] == orderedB[bIndex])
                    {
                        return orderedA[aIndex];
                    }

                    if (orderedA[aIndex] < orderedB[bIndex])
                    {
                        break;
                    }
                }
            }

            return -1;
        }
    }
}
