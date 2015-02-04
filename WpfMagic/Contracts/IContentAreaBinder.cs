
namespace WpfMagic.Contracts
{
    /// <summary>
    /// An interface that allows you to define how a particular content area provider is filled in at runtime.
    /// </summary>    
    public interface IContentAreaBinder<T> where T : IContentAreaProvider
    {
        void BindContentArea(T contentArea, object viewModel, ViewBinder binder);
    }
}
