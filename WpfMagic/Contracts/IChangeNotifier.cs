using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfMagic.Contracts
{
	internal interface IChangeNotifier
	{
		void NotifyPropertyChange(string name);
	}
}
