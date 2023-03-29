namespace PhoneBook.XUnit.Tests
{
    public class PhoneBookUnitTests
    {
        [Fact]
        public void UnitTestSample()
        {
            int a = 2;
            int b = 3;
            int result = a + b;
            Assert.Equal(5, result);
        }
    }
}