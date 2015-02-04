
namespace WpfMagic.Contracts
{
    /// <summary>
    /// The base interface for all WpfMagic UI components. It allows you to assign a name to content area you are filling in
    /// </summary>
    public interface IContentAreaProvider
    {
        string ContentArea { get; }
    }
}
