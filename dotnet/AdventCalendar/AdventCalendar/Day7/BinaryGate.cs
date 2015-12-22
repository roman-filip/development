namespace AdventCalendar.Day7
{
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
}