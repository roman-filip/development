using System;
using System.Security.Cryptography;
using System.Text;

namespace AdventCalendar
{
    /// <summary>
    /// --- Day 4: The Ideal Stocking Stuffer ---
    /// http://adventofcode.com/day/4
    /// </summary>
    public class Day4
    {
        /// <summary>
        /// Santa needs help mining some AdventCoins (very similar to bitcoins) to use as gifts for all the economically forward-thinking little girls and boys.
        /// To do this, he needs to find MD5 hashes which, in hexadecimal, start with at least five zeroes. The input to the MD5 hash is some secret key (your puzzle input, given below) followed by a number in decimal. To mine AdventCoins, you must find Santa the lowest positive number (no leading zeroes: 1, 2, 3, ...) that produces such a hash.
        /// For example:
        ///     If your secret key is abcdef, the answer is 609043, because the MD5 hash of abcdef609043 starts with five zeroes (000001dbbfa...), and it is the lowest such number to do so.
        ///     If your secret key is pqrstuv, the lowest number it combines with to make an MD5 hash starting with five zeroes is 1048970; that is, the MD5 hash of pqrstuv1048970 looks like 000006136ef....
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part1(string input)
        {
            return CrackHash(input, md5Hash => md5Hash[0] == 0 && md5Hash[1] == 0 && md5Hash[2] < 16);
        }

        /// <summary>
        /// Now find one that starts with six zeroes.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part2(string input)
        {
            return CrackHash(input, md5Hash => md5Hash[0] == 0 && md5Hash[1] == 0 && md5Hash[2] == 0);
        }

        private static int CrackHash(string input, Func<byte[], bool> isHashCracked)
        {
            using (var md5 = MD5.Create())
            {
                var number = 0;
                do
                {
                    var stringToHash = string.Format("{0}{1}", input, number);
                    var md5Hash = md5.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                    if (isHashCracked(md5Hash))
                    {
                        return number;
                    }

                    number++;
                } while (number > 0);

                return -1;
            }
        }
    }
}
