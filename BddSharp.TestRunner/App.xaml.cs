using System.Windows;
using BddSharp.TestRunner.ViewModels;
using WpfMagic;
using WpfMagic.Attributes;
using WpfMagic.Mvvm;
using WpfMagic.Xaml;

namespace BddSharp.TestRunner
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			this.Startup += App_Startup;
		}

		void App_Startup(object sender, StartupEventArgs e)
		{
			var window = new MainWindow();

			ViewBinder.Instance.Display(new RunnerViewModel());

			this.MainWindow = window;
			this.MainWindow.Show();
		}
	}
}
