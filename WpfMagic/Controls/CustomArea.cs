using System.Windows.Controls;
using WpfMagic.Contracts;

namespace WpfMagic.Controls
{
    public class CustomArea : ContentPresenter, IContentAreaProvider
    {
        public string ContentArea { get; set; }
    }
}
