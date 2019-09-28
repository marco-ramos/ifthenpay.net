namespace Ifthenpay.Domain
{
    public sealed class Configuration
    {
        public string Entity { get; private set; }
        public string SubEntity { get; private set; }

        public Configuration(string entity, string subEntity)
        {
            Entity = entity;
            SubEntity = subEntity;
        }
    }
}
