namespace AdventCalendar.Day7
{
    public class Wire : ICircuitElement
    {
        private ushort? _inputSignalValue;

        private readonly ICircuitElement _input;

        public string Identifier { get; private set; }

        public Wire(string identifier, ICircuitElement input)
        {
            Identifier = identifier;
            _input = input;
        }

        public ushort GetSignalValue()
        {
            // We have to cache the value otherwise it is unbelievably slow
            if (_inputSignalValue == null)
            {
                _inputSignalValue = _input.GetSignalValue();
            }

            return _inputSignalValue.Value;
        }
    }
}