using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfMagic.Attributes;
using WpfMagic.Mvvm;

namespace BddSharp.TestRunner.ViewModels
{
	public class TestActiveViewModel : NotifyableObject
	{
		[Active]
		public virtual string DisplayName { get; set; }		

		public TestActiveViewModel()
		{
			DisplayName = "foo";
		}
	}
}
