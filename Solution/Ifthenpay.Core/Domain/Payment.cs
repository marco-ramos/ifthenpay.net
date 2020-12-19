namespace Ifthenpay.Core.Domain
{
    public sealed class Payment
    {
        private readonly Configuration _configuration;
        public string Entity => _configuration?.Entity;
        public int Identifier { get; private set; }
        public decimal Amount { get; private set; }
        private string _reference = null;
        public string Reference
        {
            get
            {
                if (string.IsNullOrEmpty(_reference))
                {
                    _reference = Services.Generator.GetReference(this._configuration, this.Identifier, this.Amount);
                }
                return _reference;
            }
        }

        public Payment(Configuration configuration, int identifier, decimal amount)
        {
            _configuration = configuration;
            Identifier = identifier;
            Amount = amount;
        }
    }
}
