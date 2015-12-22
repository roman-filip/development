namespace AdventCalendar.Day7
{
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
}