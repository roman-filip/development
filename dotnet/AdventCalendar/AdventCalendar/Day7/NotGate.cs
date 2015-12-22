namespace AdventCalendar.Day7
{
    public class NotGate : ICircuitElement
    {
        private readonly ICircuitElement _input;

        public NotGate(ICircuitElement input)
        {
            _input = input;
        }

        public ushort GetSignalValue()
        {
            return (ushort)~_input.GetSignalValue();
        }
    }
}