# WpfMagic
A library minimizes the amount of XAML required to create a modern WPF application

##Sample Application

The BDD Sharp application contained in this repo is Behavior Driven Development application written in WPF and using the WpfMagic
framework. It's a simple application designed to showcase key features of the WpfMagic library. Here's an example view

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
