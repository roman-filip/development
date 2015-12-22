namespace AdventCalendar.Day7
{
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
}