namespace Bogus
{
    /// <summary>
    /// Marker interface for datasets that are locale aware.
    /// </summary>
    public interface ILocaleAware
    {
        string Locale { get; set; }
    }
}
