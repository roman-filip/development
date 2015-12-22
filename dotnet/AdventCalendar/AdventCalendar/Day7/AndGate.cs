namespace AdventCalendar.Day7
{
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
}