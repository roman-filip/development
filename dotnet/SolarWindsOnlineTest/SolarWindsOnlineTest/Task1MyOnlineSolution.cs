namespace SolarWindsOnlineTest
{
    public class Task1MyOnlineSolution
    {
        public int solution(int[] A)
        {
            var leftIndexes = new int?[100000];
            var result = 0;
            for (int i = 0; i < A.Length; i++)
            {
                if (leftIndexes[A[i]] == null)
                {
                    leftIndexes[A[i]] = i;
                }
                else
                {
                    var distance = i - leftIndexes[A[i]].Value;
                    if (distance > result)
                    {
                        result = distance;
                    }
                }
            }

            return result;
        }
    }
}
