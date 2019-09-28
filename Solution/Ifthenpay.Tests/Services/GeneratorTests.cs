using Xunit;

namespace Ifthenpay.Tests.Services
{
    public class GeneratorTests
    {
        [Theory(DisplayName = "Test generation of valid references")]
        [InlineData(1, 10, "222 000 150")]
        [InlineData(10, 10.05, "222 001 079")]
        [InlineData(100, 99.99, "222 010 018")]
        public void ConfirmValidReferencesGenerated(int identifier, decimal amount, string reference)
        {
            var result = Ifthenpay.Services.Generator.GetReference(Global.Configuration, identifier, amount);
            Assert.Equal(result, reference);
        }

        [Theory(DisplayName = "Test generation of invalid references")]
        [InlineData(1, 10, "222 015 091")]
        [InlineData(10, 10.05, "222 000 151")]
        [InlineData(100, 99.99, "222 000 151")]
        public void ConfirmInvalidReferencesGenerated(int identifier, decimal amount, string reference)
        {
            var result = Ifthenpay.Services.Generator.GetReference(Global.Configuration, identifier, amount);
            Assert.NotEqual(result, reference);
        }
    }
}
