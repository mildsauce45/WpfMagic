using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfMagic.Xaml
{
	public static class BindingHelpers
	{
		/// <summary>
		/// This attached property is used to bind to an object outside of an elements XAML namescope
		/// </summary>
		#region NestedBinding Attached Property

		public static readonly DependencyProperty NestedBindingProperty =
			DependencyProperty.RegisterAttached("NestedBinding", typeof(BindingOptions), typeof(BindingHelpers), new PropertyMetadata(OnNestedBindingChanged));

		public static BindingOptions GetNestedBinding(DependencyObject d)
		{
			return (BindingOptions)d.GetValue(NestedBindingProperty);
		}

		public static void SetNestedBinding(DependencyObject d, BindingOptions value)
		{
			d.SetValue(NestedBindingProperty, value);
		}

		private static void OnNestedBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs ea)
		{
			if (!(d is FrameworkElement))
				return;

			var bo = ea.NewValue as BindingOptions;

			// Create a dummy multi binding class to wrap this binding so that a single method can handle one or more bindings
			var mb = new MultiBinding();
			mb.Bindings.Add(bo);

			SetupBindings(d, mb);
		}

		#endregion

		/// <summary>
		/// This attached property is used to bind multiple properties on an element to an object outside it's XAML namescope
		/// </summary>
		#region MultiBinding Attached Property

		public static readonly DependencyProperty MultiBindingProperty =
			DependencyProperty.RegisterAttached("MultiBinding", typeof(MultiBinding), typeof(BindingHelpers), new PropertyMetadata(OnMultiBindingChanged));

		public static MultiBinding GetMultiBinding(DependencyObject d)
		{
			return (MultiBinding)d.GetValue(MultiBindingProperty);
		}

		public static void SetMultiBinding(DependencyObject d, MultiBinding value)
		{
			d.SetValue(MultiBindingProperty, value);
		}

		private static void OnMultiBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement))
				return;

			SetupBindings(d, e.NewValue as MultiBinding);
		}

		#endregion

		private static void SetupBindings(DependencyObject d, MultiBinding mb)
		{
			if (mb == null)
				return;

			FrameworkElement fe = d as FrameworkElement;
			fe.Loaded += (obj, ea) =>
			{
				foreach (BindingOptions bo in mb.Bindings)
				{
					if (!(obj is Visual || obj is Visual3D))
						return;

					DependencyObject parent = VisualTreeHelper.GetParent(obj as DependencyObject);

					while (parent != null && parent.GetValue(FrameworkElement.NameProperty).ToString() != bo.ElementName)
						parent = VisualTreeHelper.GetParent(parent);

					if (parent != null)
					{
						DependencyProperty dependencyProperty = null;

						FieldInfo fieldInfo = d.GetType().GetField(bo.DependencyProperty + "Property", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

						object field = fieldInfo.GetValue(d);
						if (field is DependencyProperty)
							dependencyProperty = field as DependencyProperty;

						Binding b = new Binding(bo.Path);
						b.Source = parent;
						b.Mode = bo.Mode;
						b.Converter = bo.Converter;
						b.ConverterParameter = bo.ConverterParameter;
						b.StringFormat = bo.StringFormat;
						b.NotifyOnValidationError = bo.NotifyOnValidationError;
						b.ValidatesOnExceptions = bo.ValidatesOnExceptions;

						fe.SetBinding(dependencyProperty, b);
					}
				}
			};

		}
	}

	public class BindingOptions
	{
		public string ElementName {get; set;}
		public string Path { get; set; }
		public string DependencyProperty { get; set; }
		public BindingMode Mode { get; set; }
		public IValueConverter Converter { get; set; }
		public object ConverterParameter { get; set; }
		public string StringFormat { get; set; }
		public bool NotifyOnValidationError { get; set; }
		public bool ValidatesOnExceptions { get; set; }

		public BindingOptions()
		{
			Mode = BindingMode.OneWay;
		}
	}

	[ContentProperty("Bindings")]
	public class MultiBinding
	{
		public List<BindingOptions> Bindings { get; set; }

		public MultiBinding()
		{
			Bindings = new List<BindingOptions>();
		}
	}
}
