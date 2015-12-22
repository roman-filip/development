using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    /// <summary>
    /// --- Day 7: Some Assembly Required ---
    /// http://adventofcode.com/day/7
    /// </summary>
    public class Day7
    {
        private const string _regExpStr = "([a-z]*|\\d*)( AND | OR | LSHIFT | RSHIFT |NOT ){0,1}([a-z]*|\\d*)( -> )([a-z]*)";

        private readonly Regex _regExp = new Regex(_regExpStr, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private readonly Dictionary<string, Wire> _circuitElements = new Dictionary<string, Wire>();

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
            var result = new Dictionary<string, ushort>();

            var signals = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            int i = 0;
            while (signals.Count > 0)
            {
                var signal = signals[i];
                var match = _regExp.Match(signal);
                if (match.Success)
                {
                    var input1 = GetCircuitElement(match.Groups[1].ToString());
                    var operation = match.Groups[2].ToString().Trim();
                    var input2 = GetCircuitElement(match.Groups[3].ToString());
                    var output = match.Groups[5].ToString();
                    switch (operation)
                    {
                        case "NOT":
                            if (input2 != null)
                            {
                                _circuitElements[output] = new Wire(output, new NotGate(input2));
                                signals.Remove(signal);
                            }
                            break;
                        case "AND":
                            if (input1 != null && input2 != null)
                            {
                                _circuitElements[output] = new Wire(output, new AndGate(input1, input2));
                                signals.Remove(signal);
                            }
                            break;
                        case "OR":
                            if (input1 != null && input2 != null)
                            {
                                _circuitElements[output] = new Wire(output, new OrGate(input1, input2));
                                signals.Remove(signal);
                            }
                            break;
                        case "LSHIFT":
                            if (input1 != null && input2 != null)
                            {
                                _circuitElements[output] = new Wire(output, new LeftShiftGate(input1, input2));
                                signals.Remove(signal);
                            }
                            break;
                        case "RSHIFT":
                            if (input1 != null && input2 != null)
                            {
                                _circuitElements[output] = new Wire(output, new RightShiftGate(input1, input2));
                                signals.Remove(signal);
                            }
                            break;
                        case "":
                            if (input1 != null || input2 != null)
                            {
                                _circuitElements[output] = new Wire(output, input1 ?? input2);
                                signals.Remove(signal);
                            }
                            break;
                        default:
                            throw new ApplicationException("Unsupported operation " + operation);
                    }
                }
                else
                {
                    throw new ApplicationException("No regular expression match for " + signal);
                }



                i++;
                if (i >= signals.Count)
                {
                    i = 0;
                }
            }


            var ii = 0;
            foreach (var circuitElement in _circuitElements)
            {
                if (circuitElement.Key != "a")
                {
                    ii++;
                    Debug.WriteLine(circuitElement.Key + " = " + circuitElement.Value.GetSignalValue());
                }
            }

            //_circuitElements.Values
            //    .ToList()
            //    .ForEach(ce => result[ce.Identifier] = ce.GetSignalValue());

            //_circuitElements.Values
            //    .ToList()
            //    .ForEach(ce => ce.WriteDebug());


            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Part2(string input)
        {
            return -1;
        }

        private ICircuitElement GetCircuitElement(string input)
        {
            ushort signalValue;
            if (ushort.TryParse(input, out signalValue))
            {
                var signal = new Signal(signalValue);
                return signal;
            }
            else
            {
                if (_circuitElements.ContainsKey(input))
                {
                    return _circuitElements[input];
                }
                else
                {
                    return null;
                }
            }
        }
    }


    public interface ICircuitElement
    {
        ushort GetSignalValue();
    }

    public class Wire : ICircuitElement
    {
        public string Identifier { get; private set; }

        private readonly ICircuitElement _circuitElement;

        public Wire(string identifier, ICircuitElement circuitElement)
        {
            Identifier = identifier;
            _circuitElement = circuitElement;
        }

        public ushort GetSignalValue()
        {
            Debug.WriteLine(Identifier);

            return _circuitElement.GetSignalValue();
        }

        public void WriteDebug()
        {
            if (_circuitElement is Signal)
            {
                Debug.Write(_circuitElement.GetSignalValue());
            }
            if (_circuitElement is NotGate)
            {
                Debug.Write("NOT " + _circuitElement.GetSignalValue());
            }


            Debug.WriteLine(" -> " + Identifier);
        }
    }

    public class Signal : ICircuitElement
    {
        private readonly ushort _value;

        public Signal(ushort value)
        {
            _value = value;
        }

        public ushort GetSignalValue()
        {
            return _value;
        }
    }

    public abstract class BinaryGate : ICircuitElement
    {
        protected ICircuitElement Input1 { get; private set; }

        protected ICircuitElement Input2 { get; private set; }

        protected BinaryGate(ICircuitElement input1, ICircuitElement input2)
        {
            Input1 = input1;
            Input2 = input2;
        }

        public abstract ushort GetSignalValue();
    }

    public class AndGate : BinaryGate
    {
        public AndGate(ICircuitElement input1, ICircuitElement input2)
            : base(input1, input2)
        { }

        public override ushort GetSignalValue()
        {
            return (ushort)(Input1.GetSignalValue() & Input2.GetSignalValue());
        }
    }

    public class OrGate : BinaryGate
    {
        public OrGate(ICircuitElement input1, ICircuitElement input2)
            : base(input1, input2)
        { }

        public override ushort GetSignalValue()
        {
            return (ushort)(Input1.GetSignalValue() | Input2.GetSignalValue());
        }
    }

    public class NotGate : ICircuitElement
    {
        private ICircuitElement _input;

        public NotGate(ICircuitElement input)
        {
            _input = input;
        }

        public ushort GetSignalValue()
        {
            return (ushort)~_input.GetSignalValue();
        }
    }

    public abstract class ShiftGate : ICircuitElement
    {
        protected ICircuitElement Input { get; private set; }

        protected ICircuitElement Shift { get; private set; }

        protected ShiftGate(ICircuitElement input, ICircuitElement shift)
        {
            Input = input;
            Shift = shift;
        }

        public abstract ushort GetSignalValue();
    }

    public class LeftShiftGate : ShiftGate
    {
        public LeftShiftGate(ICircuitElement input, ICircuitElement shift)
            : base(input, shift)
        { }

        public override ushort GetSignalValue()
        {
            return (ushort)(Input.GetSignalValue() << Shift.GetSignalValue());
        }
    }

    public class RightShiftGate : ShiftGate
    {
        public RightShiftGate(ICircuitElement input, ICircuitElement shift)
            : base(input, shift)
        { }

        public override ushort GetSignalValue()
        {
            return (ushort)(Input.GetSignalValue() >> Shift.GetSignalValue());
        }
    }
}
