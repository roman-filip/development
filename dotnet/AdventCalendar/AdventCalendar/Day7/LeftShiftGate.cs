namespace AdventCalendar.Day7
{
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
}