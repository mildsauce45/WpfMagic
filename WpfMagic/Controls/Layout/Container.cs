using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using WpfMagic.Contracts;

namespace WpfMagic.Controls.Layout
{
    public class Container : ContentControl, IRootView
    {
        public Container()
        {
            ViewBinder.Instance.ViewRoot = this;
        }
    }
}
