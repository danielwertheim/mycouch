namespace MyCouch.IntegrationTests
{
    /// <summary>
    /// Simple marker interface to indicate that a Integration test extending
    /// <see cref="IntegrationTestsOf{T}"/>, do not want to clear state between
    /// every test.
    /// </summary>
    public interface IPreserveStatePerFixture {}
}