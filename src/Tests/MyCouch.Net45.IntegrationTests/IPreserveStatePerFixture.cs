namespace MyCouch.IntegrationTests
{
    /// <summary>
    /// Simple marker interface to indicate that a Integration test extending
    /// do not want to clear state between every test.
    /// </summary>
    public interface IPreserveStatePerFixture {}
}