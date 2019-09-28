namespace Ifthenpay.Domain
{
    public sealed class Payment
    {
        private readonly Configuration _configuration;
        public string Entity => _configuration?.Entity;
        public int Identifer { get; private set; }
        public decimal Amount { get; private set; }
        private string _reference = null;
        public string Reference
        {
            get
            {
                if (string.IsNullOrEmpty(_reference))
                {
                    _reference = Services.Generator.GetReference(this._configuration, this.Identifer, this.Amount);
                }
                return _reference;
            }
        }

        public Payment(Configuration configuration, int identifer, decimal amount)
        {
            _configuration = configuration;
            Identifer = identifer;
            Amount = amount;
        }
    }
}
