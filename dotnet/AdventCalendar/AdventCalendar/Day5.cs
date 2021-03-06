﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    /// <summary>
    /// --- Day 5: Doesn't He Have Intern-Elves For This? ---
    /// http://adventofcode.com/day/5
    /// </summary>
    public class Day5
    {
        private readonly Regex _regExVowels = new Regex("((.)*(a|e|i|o|u)(.)*){3,}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private readonly Regex _regExNaughty = new Regex("(.)*(ab|cd|pq|xy)(.)*", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// Santa needs help figuring out which strings in his text file are naughty or nice.
        /// 
        /// A nice string is one with all of the following properties:
        ///     It contains at least three vowels (aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
        ///     It contains at least one letter that appears twice in a row, like xx, abcdde (dd), or aabbccdd (aa, bb, cc, or dd).
        ///     It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.
        /// 
        /// For example:
        ///     ugknbfddgicrmopn is nice because it has at least three vowels (u...i...o...), a double letter (...dd...), and none of the disallowed substrings.
        ///     aaa is nice because it has at least three vowels and a double letter, even though the letters used by different rules overlap.
        ///     jchzalrnumimnmhp is naughty because it has no double letter.
        ///     haegwjzuvuyypxyu is naughty because it contains the string xy.
        ///     dvszwmarrgswjxmb is naughty because it contains only one vowel.
        /// 
        /// How many strings are nice?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part1(string input)
        {
            var words = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var niceWords = words.Where(word => IsNiceWord(word));

            return niceWords.Count();
        }

        private bool IsNiceWord(string word)
        {
            return ContainsVowels(word) && ContainsLetterPair(word) && NotNaughty(word);
        }

        private bool ContainsVowels(string word)
        {
            return _regExVowels.Match(word).Success;
        }

        private bool ContainsLetterPair(string word)
        {
            for (var i = 0; i < word.Length - 1; i++)
            {
                if (word[i] == word[i + 1])
                {
                    return true;
                }
            }
            return false;
        }

        private bool NotNaughty(string word)
        {
            return !_regExNaughty.Match(word).Success;
        }

        /// <summary>
        /// --- Part Two ---
        /// Realizing the error of his ways, Santa has switched to a better model of determining whether a string is naughty or nice. None of the old rules apply, as they are all clearly ridiculous.
        /// 
        /// Now, a nice string is one with all of the following properties:
        ///     It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
        ///     It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
        /// 
        /// For example:
        ///     qjhvhtzxzqqjkmpb is nice because is has a pair that appears twice (qj) and a letter that repeats with exactly one letter between them (zxz).
        ///     xxyxx is nice because it has a pair that appears twice and a letter that repeats with one between, even though the letters used by each rule overlap.
        ///     uurcxstgmygtbstg is naughty because it has a pair (tg) but no repeat with a single letter between them.
        ///     ieodomkazucvgmuy is naughty because it has a repeating letter with one between (odo), but no pair that appears twice.
        /// 
        /// How many strings are nice under these new rules?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part2(string input)
        {
            var words = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var nicerWords = words.Where(word => IsNicerWord(word));

            return nicerWords.Count();
        }

        private bool IsNicerWord(string word)
        {
            return ContainsSamePair(word) && ContainsLetterPairWithSpace(word);
        }

        private bool ContainsSamePair(string word)
        {
            var pairs = new Dictionary<string, int>();
            for (int i = 0; i < word.Length - 1; i++)
            {
                var pair = word.Substring(i, 2);
                int index;
                if (pairs.TryGetValue(pair, out index))
                {
                    if (i - index > 1)
                    {
                        return true;
                    }
                }
                else
                {
                    pairs[pair] = i;
                }
            }
            return false;
        }

        private static bool ContainsLetterPairWithSpace(string word)
        {
            for (int i = 0; i < word.Length - 2; i++)
            {
                if (word[i] == word[i + 2])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
