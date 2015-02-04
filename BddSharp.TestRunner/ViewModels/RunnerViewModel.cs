using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using BddSharp.Engine.Attributes;
using BddSharp.Engine.Extensions;
using BddSharp.Engine.Tests;
using BddSharp.TestRunner.Converters;
using BddSharp.TestRunner.Models;
using Microsoft.Win32;
using WpfMagic;
using WpfMagic.Attributes;
using WpfMagic.Commands;
using WpfMagic.Mvvm;

namespace BddSharp.TestRunner.ViewModels
{
	public class RunnerViewModel : NotifyableObject
	{
		#region Private Variables

		private Assembly suiteAssembly;
		private bool allowTestRun;

		#endregion

		#region Properties

		public bool AllowTestRun
		{
			get { return allowTestRun; }
			set
			{
				allowTestRun = value;
				NotifyPropertyChanged(() => AllowTestRun);
			}
		}

		[CustomArea("FooterTemplate", "Footer")]
		public FooterViewModel FooterVM { get; private set; }

		[ItemsSource]
		public ObservableCollection<Test> Specifications { get; private set; }

		#endregion

		#region Commands

		[ToolbarAction]
		public ICommand LoadSuiteCommand
		{
			get { return new ExecutableCommand(() => LoadSuite()); }
		}

		[ToolbarAction]
		public ICommand RunSuiteCommand { get; private set; }

		[ToolbarAction]
		[ContentArea("RightToolbar")]
		public ICommand ExportResultsCommand { get; private set; }

		#endregion

		#region Constructors

		public RunnerViewModel()
		{
			Specifications = new ObservableCollection<Test>();
			FooterVM = new FooterViewModel();

			RunSuiteCommand = new ExecutableCommand(
				() => RunSuite(),
				() => AllowTestRun);

			ExportResultsCommand = new ExecutableCommand(
				() => ViewBinder.Instance.DisplayActive<TestActiveViewModel>(),
				() => AllowTestRun && FooterVM.AllTestsPassed.HasValue);
		}

		#endregion

		#region Command Handlers

		private void LoadSuite()
		{
			var ofd = new OpenFileDialog();
			ofd.Filter = "Dll Files|*.dll";
			ofd.Multiselect = false;

			if (!ofd.ShowDialog().GetValueOrDefault()) return;

			try
			{
				suiteAssembly = Assembly.LoadFile(ofd.FileName);

				if (suiteAssembly == null)
					return;

				Specifications.Clear();

				// Since we were able to load the assembly, check to make sure it has valid tests in it before we all the tests to run
				var specType = typeof(Specification);

				var tests = suiteAssembly.GetTypes().Where(t => specType.IsAssignableFrom(t) && t != specType).ToList();

				AllowTestRun = tests.Any();

				tests.ForEach(t =>
				{
					var attr = t.GetCustomAttribute<ScenarioAttribute>();

					var spec = Activator.CreateInstance(t) as Specification;
					if (spec == null)
						return;

					Specifications.Add(new Test(spec, attr != null ? attr.Description : string.Empty));
				});

				SetStatusBar(0, 0);
				NotifyPropertyChanged(() => Specifications);
			}
			catch
			{
			}
		}

		private void RunSuite()
		{
			Specifications.ForEach(s => s.Run());

			SetStatusBar(Specifications.SelectMany(s => s.Specification.TestResult.Outcomes).Count(), Specifications.SelectMany(s => s.Specification.TestResult.Outcomes).Count(o => !o.FirstAssertionFailure.HasValue));
		}

		#endregion

		#region Private Methods

		private void SetStatusBar(int tests, int passed)
		{
			FooterVM.StatusBarText = string.Format("Specifications: {0} Outcomes: {1} Passed: {2}", Specifications.Count, tests, passed);
			FooterVM.AllTestsPassed = tests == 0 ? (bool?)null : tests == passed;
		}

		#endregion
	}
}
