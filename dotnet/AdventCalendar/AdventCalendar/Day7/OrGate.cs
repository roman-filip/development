namespace AdventCalendar.Day7
{
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
}