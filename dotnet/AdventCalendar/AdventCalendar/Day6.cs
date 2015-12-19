using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    /// <summary>
    /// --- Day 6: Probably a Fire Hazard ---
    /// http://adventofcode.com/day/6
    /// </summary>
    public class Day6
    {
        /// <summary>
        /// Because your neighbors keep defeating you in the holiday house decorating contest year after year, you've decided to deploy one million lights in a 1000x1000 grid.
        /// Furthermore, because you've been especially nice this year, Santa has mailed you instructions on how to display the ideal lighting configuration.
        /// Lights in your grid are numbered from 0 to 999 in each direction; the lights at each corner are at 0,0, 0,999, 999,999, and 999,0. The instructions include whether to turn on, turn off, or toggle various inclusive ranges given as coordinate pairs. Each coordinate pair represents opposite corners of a rectangle, inclusive; a coordinate pair like 0,0 through 2,2 therefore refers to 9 lights in a 3x3 square. The lights all start turned off.
        /// To defeat your neighbors this year, all you have to do is set up your lights by doing the instructions Santa sent you in order.
        /// 
        /// For example:
        ///     turn on 0,0 through 999,999 would turn on (or leave on) every light.
        ///     toggle 0,0 through 999,0 would toggle the first line of 1000 lights, turning off the ones that were on, and turning on the ones that were off.
        ///     turn off 499,499 through 500,500 would turn off (or leave off) the middle four lights.
        /// 
        /// After following the instructions, how many lights are lit?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part1(string input)
        {
            var lights = new int[1000, 1000];

            var commands = LightCommand.CreateCommands(input, LightCommand.CreateCommand);
            commands.ToList().ForEach(command => command.Apply(lights));

            return GetTurnedOnLights(lights);
        }

        /// <summary>
        /// You just finish implementing your winning light pattern when you realize you mistranslated Santa's message from Ancient Nordic Elvish.
        /// The light grid you bought actually has individual brightness controls; each light can have a brightness of zero or more. The lights all start at zero.
        /// The phrase turn on actually means that you should increase the brightness of those lights by 1.
        /// The phrase turn off actually means that you should decrease the brightness of those lights by 1, to a minimum of zero.
        /// The phrase toggle actually means that you should increase the brightness of those lights by 2.
        /// What is the total brightness of all lights combined after following Santa's instructions?
        /// 
        /// For example:
        ///     turn on 0,0 through 0,0 would increase the total brightness by 1.
        ///     toggle 0,0 through 999,999 would increase the total brightness by 2000000.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part2(string input)
        {
            var lights = new int[1000, 1000];

            var commands = LightCommand.CreateCommands(input, LightCommand.CreateCommand2);
            commands.ToList().ForEach(command => command.Apply(lights));

            return GetTurnedOnLights(lights);
        }

        private static int GetTurnedOnLights(int[,] lights)
        {
            var count = 0;
            for (int row = 0; row <= lights.GetUpperBound(0); row++)
            {
                for (int column = 0; column <= lights.GetUpperBound(1); column++)
                {
                    count += lights[row, column];
                }
            }
            return count;
        }
    }

    public abstract class LightCommand
    {
        public Point StarPoint { get; private set; }

        public Point EndPoint { get; private set; }

        protected LightCommand(int row1, int col1, int row2, int col2)
        {
            StarPoint = new Point(row1, col1);
            EndPoint = new Point(row2, col2);
        }

        public static IEnumerable<LightCommand> CreateCommands(string input, Func<string, int, int, int, int, LightCommand> createCommandFunc)
        {
            var commands = new List<LightCommand>();
            var txtCommands = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var regexp = new Regex("(turn on |turn off |toggle )(\\d+)(,)(\\d+)( through )(\\d+)(,)(\\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            foreach (var txtCommand in txtCommands)
            {
                var match = regexp.Match(txtCommand);
                if (match.Success)
                {
                    var commandType = match.Groups[1].ToString().Trim();
                    var row1 = int.Parse(match.Groups[2].Value);
                    var col1 = int.Parse(match.Groups[4].Value);
                    var row2 = int.Parse(match.Groups[6].Value);
                    var col2 = int.Parse(match.Groups[8].Value);

                    commands.Add(createCommandFunc(commandType, row1, col1, row2, col2));
                }
            }

            return commands;
        }

        public static LightCommand CreateCommand(string commandType, int row1, int col1, int row2, int col2)
        {
            switch (commandType)
            {
                case "turn on":
                    return new TurnOnLightCommand(row1, col1, row2, col2);
                case "turn off":
                    return new TurnOffLightCommand(row1, col1, row2, col2);
                case "toggle":
                    return new ToggleLightCommand(row1, col1, row2, col2);
                default:
                    throw new ArgumentOutOfRangeException("commandType", "Unsupported type of command");
            }
        }

        public static LightCommand CreateCommand2(string commandType, int row1, int col1, int row2, int col2)
        {
            switch (commandType)
            {
                case "turn on":
                    return new TurnOnLightCommand2(row1, col1, row2, col2);
                case "turn off":
                    return new TurnOffLightCommand2(row1, col1, row2, col2);
                case "toggle":
                    return new ToggleLightCommand2(row1, col1, row2, col2);
                default:
                    throw new ArgumentOutOfRangeException("commandType", "Unsupported type of command");
            }
        }

        public void Apply(int[,] lights)
        {
            for (int row = StarPoint.X; row <= EndPoint.X; row++)
            {
                for (int col = StarPoint.Y; col <= EndPoint.Y; col++)
                {
                    ApplyOperation(ref lights[row, col]);
                }
            }
        }

        protected abstract void ApplyOperation(ref int light);
    }

    public class TurnOnLightCommand : LightCommand
    {
        public TurnOnLightCommand(int row1, int col1, int row2, int col2)
            : base(row1, col1, row2, col2)
        { }

        protected override void ApplyOperation(ref int light)
        {
            light = 1;
        }
    }

    public class TurnOffLightCommand : LightCommand
    {
        public TurnOffLightCommand(int row1, int col1, int row2, int col2)
            : base(row1, col1, row2, col2)
        { }

        protected override void ApplyOperation(ref int light)
        {
            light = 0;
        }
    }

    public class ToggleLightCommand : LightCommand
    {
        public ToggleLightCommand(int row1, int col1, int row2, int col2)
            : base(row1, col1, row2, col2)
        { }

        protected override void ApplyOperation(ref int light)
        {
            light = light == 1 ? 0 : 1;
        }
    }

    public class TurnOnLightCommand2 : LightCommand
    {
        public TurnOnLightCommand2(int row1, int col1, int row2, int col2)
            : base(row1, col1, row2, col2)
        { }

        protected override void ApplyOperation(ref int light)
        {
            light++;
        }
    }

    public class TurnOffLightCommand2 : LightCommand
    {
        public TurnOffLightCommand2(int row1, int col1, int row2, int col2)
            : base(row1, col1, row2, col2)
        { }

        protected override void ApplyOperation(ref int light)
        {
            if (light > 0)
            {
                light--;
            }
        }
    }

    public class ToggleLightCommand2 : LightCommand
    {
        public ToggleLightCommand2(int row1, int col1, int row2, int col2)
            : base(row1, col1, row2, col2)
        { }

        protected override void ApplyOperation(ref int light)
        {
            light += 2;
        }
    }
}
