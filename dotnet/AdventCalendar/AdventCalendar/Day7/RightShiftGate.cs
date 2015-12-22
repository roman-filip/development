namespace AdventCalendar.Day7
{
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