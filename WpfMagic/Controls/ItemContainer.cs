using System.Windows;
using System.Windows.Controls;
using WpfMagic.Contracts;

namespace WpfMagic.Controls
{
    public class ItemContainer : ContentControl, IContentAreaProvider
    {
        public string ContentArea { get; set; }        
    }
}
