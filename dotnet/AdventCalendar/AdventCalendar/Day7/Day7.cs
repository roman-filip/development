using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar.Day7
{
    /// <summary>
    /// --- Day 7: Some Assembly Required ---
    /// http://adventofcode.com/day/7
    /// </summary>
    public class Day7
    {
        private readonly Regex _regExp = new Regex("([a-z]*|\\d*)( AND | OR | LSHIFT | RSHIFT |NOT ){0,1}([a-z]*|\\d*)( -> )([a-z]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private readonly Dictionary<string, Wire> _wires = new Dictionary<string, Wire>();

        /// <summary>
        /// This year, Santa brought little Bobby Tables a set of wires and bitwise logic gates! Unfortunately, little Bobby is a little under the recommended age range, and he needs help assembling the circuit.
        /// Each wire has an identifier (some lowercase letters) and can carry a 16-bit signal (a number from 0 to 65535). A signal is provided to each wire by a gate, another wire, or some specific value. Each wire can only get a signal from one source, but can provide its signal to multiple destinations. A gate provides no signal until all of its inputs have a signal.
        /// The included instructions booklet describes how to connect the parts together: x AND y -> z means to connect wires x and y to an AND gate, and then connect its output to wire z.
        /// 
        /// For example:
        ///     123 -> x means that the signal 123 is provided to wire x.
        ///     x AND y -> z means that the bitwise AND of wire x and wire y is provided to wire z.
        ///     p LSHIFT 2 -> q means that the value from wire p is left-shifted by 2 and then provided to wire q.
        ///     NOT e -> f means that the bitwise complement of the value from wire e is provided to wire f.
        /// 
        /// Other possible gates include OR (bitwise OR) and RSHIFT (right-shift). If, for some reason, you'd like to emulate the circuit instead, almost all programming languages (for example, C, JavaScript, or Python) provide operators for these gates.
        /// 
        /// For example, here is a simple circuit:
        ///     123 -> x
        ///     456 -> y
        ///     x AND y -> d
        ///     x OR y -> e
        ///     x LSHIFT 2 -> f
        ///     y RSHIFT 2 -> g
        ///     NOT x -> h
        ///     NOT y -> i
        /// 
        /// After it is run, these are the signals on the wires:
        ///     d: 72
        ///     e: 507
        ///     f: 492
        ///     g: 114
        ///     h: 65412
        ///     i: 65079
        ///     x: 123
        ///     y: 456
        /// 
        /// In little Bobby's kit's instructions booklet (provided as your puzzle input), what signal is ultimately provided to wire a?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Dictionary<string, ushort> Part1(string input)
        {
            _wires.Clear();

            ProcessCircuits(input);

            var result = new Dictionary<string, ushort>();
            _wires.Values.ToList().ForEach(ce => result[ce.Identifier] = ce.GetSignalValue());
            return result;
        }

        private void ProcessCircuits(string input)
        {
            var circuits = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var i = 0;
            while (circuits.Count > 0)
            {
                var circuit = circuits[i];
                if (ProcessCircuit(circuit))
                {
                    circuits.Remove(circuit);
                }

                i++;
                if (i >= circuits.Count)
                {
                    i = 0;
                }
            }
        }

        private bool ProcessCircuit(string circuit)
        {
            var match = _regExp.Match(circuit);
            if (!match.Success)
            {
                throw new ApplicationException("No regular expression match for " + circuit);
            }

            var input1 = GetWireOrCreateSignal(match.Groups[1].ToString());
            var operation = match.Groups[2].ToString().Trim();
            var input2 = GetWireOrCreateSignal(match.Groups[3].ToString());
            var output = match.Groups[5].ToString();

            var wire = CreateWire(operation, input1, input2, output);
            if (wire != null)
            {
                _wires[wire.Identifier] = wire;
                return true;
            }
            return false;
        }

        private ICircuitElement GetWireOrCreateSignal(string input)
        {
            if (_wires.ContainsKey(input))
            {
                return _wires[input];
            }

            ushort signalValue;
            if (ushort.TryParse(input, out signalValue))
            {
                var signal = new Signal(signalValue);
                return signal;
            }

            return null;
        }

        private Wire CreateWire(string operation, ICircuitElement input1, ICircuitElement input2, string output)
        {
            Wire wire = null;
            switch (operation)
            {
                case "NOT":
                    if (input2 != null)
                    {
                        wire = new Wire(output, new NotGate(input2));
                    }
                    break;
                case "AND":
                    if (input1 != null && input2 != null)
                    {
                        wire = new Wire(output, new AndGate(input1, input2));
                    }
                    break;
                case "OR":
                    if (input1 != null && input2 != null)
                    {
                        wire = new Wire(output, new OrGate(input1, input2));
                    }
                    break;
                case "LSHIFT":
                    if (input1 != null && input2 != null)
                    {
                        wire = new Wire(output, new LeftShiftGate(input1, input2));
                    }
                    break;
                case "RSHIFT":
                    if (input1 != null && input2 != null)
                    {
                        wire = new Wire(output, new RightShiftGate(input1, input2));
                    }
                    break;
                case "":
                    if (input1 != null || input2 != null)
                    {
                        wire = new Wire(output, input1 ?? input2);
                    }
                    break;
                default:
                    throw new ApplicationException("Unsupported operation " + operation);
            }
            return wire;
        }

        /// <summary>
        /// Now, take the signal you got on wire a, override wire b to that signal, and reset the other wires (including wire a). What new signal is ultimately provided to wire a?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part2(string input)
        {
            return -1;
        }
    }
}
