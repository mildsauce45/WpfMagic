# WpfMagic
A library minimizes the amount of XAML required to create a modern WPF application

##Sample Application

The BDD Sharp application contained in this repo is Behavior Driven Development application written in WPF and using the WpfMagic
framework. It's a simple application designed to showcase key features of the WpfMagic library. 

Here's an example view (Runner.xaml)

    <UserControl x:Class="BddSharp.TestRunner.Views.Runner"
                 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                 xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
                 xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                 xmlns:wm="http://schemas.wpfmagic.com/2013/xaml"
                 mc:Ignorable="d" d:DesignHeight="300" d:DesignWidth="300">	

      <Grid Background="#FFF9F9F9">
          <Grid.RowDefinitions>
              <RowDefinition Height="Auto"/>
              <RowDefinition Height="*"/>
              <RowDefinition Height="Auto"/>
          </Grid.RowDefinitions>

          <Border BorderThickness="0,0,0,2" BorderBrush="#FFD9D9D9" Background="#FFB5DCF7" Padding="20,10">
              <Grid>
                  <wm:Toolbar HorizontalAlignment="Left"/>
                  <wm:Toolbar ContentArea="RightToolbar" HorizontalAlignment="Right"/>
              </Grid>
          </Border>

          <ScrollViewer Grid.Row="1" HorizontalScrollBarVisibility="Auto" VerticalScrollBarVisibility="Auto" Margin="0,10">
              <wm:ItemContainer Margin="20,1"/>
          </ScrollViewer>

          <wm:CustomArea ContentArea="Footer" Grid.Row="2"/>
      </Grid>
  </UserControl>

Not a lot a XAML and you'll notice there are no bindings anywhere nor are there data templates of any kind. However this xaml produces the following control at runtime.

![Test Runner](/img/TestRunnerView.png)

Not pretty (I'm not a designer) but this a working application with a couple of moving parts. Most of the magic occurs because of a few simple attributes in the view model.

##The model behind the view (RunnerViewModel.cs - the pertinent parts)

    #region Properties
    
    private bool allowTestRun
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
	
## Breaking it down

Lets deal with the view one tiny piece at a time. In the view there are 3 parts: the toolbar at the top, the test/result area, and the footer. We'll deal with styling later.

#### The Toolbar

The border in the first XAML grid row contains two WpfMagic controls, both are of type Toolbar; the second of which has a defined ContentArea called "RightToolbar". In the view model are three properties of type ICommand all of which are marked up with the ToolbarAction attribute. This tells WpfMagic that these properties need to go into a Toolbar, but which one? The first two, LoadSuiteCommand and RunSuiteCommand, have no other attributes; this tells the system to use the default Toolbar in the current view. This is the Toolbar without the ContentArea defined.

The third command, ExportResultsCommand, has an additional attribute (ContentArea) telling WpfMagic to place it in the toolbar marked "RightToolbar"

#### The Test/Results Area

The main area of the view has another WpfMagic control wrapped in a ScrollViewer, an ItemContainer. This is main items control for WpfMagic. In the view model there is a single property marked with ItemsSource attribute. This tells the system to bind the contents of this property to the ItemsSource of the ItemContainer which produces a WPF ItemsControl. Simple enough, but why pull the binding out of the view just for this? There are two reasons. The main reason is the ItemContainer can figure out the DataTemplate it needs to display automatically. More on this later. The other reason is the ItemContainer control can switch  the type of control it displays based on the attribute that feeds it. If you'd like to display the tests and results in a WPF TreeView, switch the attribute to TreeSourceAttribute and you have a new control.

#### The Footer

The final area in the test runner is the footer. This contains yet another WpfMagic control, the CustomArea. For all intents and purpose, this is a simple wrapper around a Windows ContentControl that can be driven with the WpfMagic attributes. You can see that above by looking at the FooterVM property. It uses the CustomArea attribute to pick how and where it's displayed.
