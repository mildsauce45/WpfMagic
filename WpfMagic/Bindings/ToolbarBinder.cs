using System;
using System.Linq;
using System.Windows;
using WpfMagic.Attributes;
using WpfMagic.Contracts;
using WpfMagic.Controls.Layout;
using WpfMagic.Extensions;
using System.Windows.Data;

namespace WpfMagic.Bindings
{
    internal class ToolbarBinder : IContentAreaBinder<Toolbar>
    {
        public void BindContentArea(Toolbar toolbar, object viewModel, ViewBinder binder)
        {
            if (toolbar == null || viewModel == null || binder == null)
                return;

            var vmType = viewModel.GetType();

            var vmProperties = binder.GetViewModelProperties(vmType);

            if (vmProperties == null)
                return;

            var toolbarActions = vmProperties.Select(pi => new AttributeBinding<ToolbarActionAttribute>(pi.GetCustomAttribute<ToolbarActionAttribute>(), pi)).Where(a => a.Attr != null);

            if (toolbarActions.IsNullOrEmpty())
                return;

            var attrType = typeof(ToolbarActionAttribute);

            toolbarActions.ForEach(tba =>
            {
                var contentAreaAttribute = tba.Property.GetCustomAttribute<ContentAreaAttribute>();

                // For every toolbar action in this view model we need to check to see if it should be added to this toolbar
                if ((string.IsNullOrWhiteSpace(toolbar.ContentArea) && (contentAreaAttribute == null || string.IsNullOrWhiteSpace(contentAreaAttribute.ContentArea))) ||
                    (contentAreaAttribute != null && toolbar.ContentArea == contentAreaAttribute.ContentArea))
                {
                    var controlBinding = tba.Attr.ControlTypeOverride != null ? new ControlTypeBinding(tba.Attr.ControlTypeOverride) : binder.GetControlBinding(attrType);

                    var control = controlBinding.ControlType.SafeCreate<FrameworkElement>();

                    if (controlBinding.IsCommandSource || controlBinding.HasCommandProperty)
                        control.CreateBinding("Command", tba.Property.Name);

                    var text = !string.IsNullOrWhiteSpace(tba.Attr.Name) ? tba.Attr.Name : tba.Property.Name.Replace("Command", string.Empty).SeparateOnCamelCase();

                    // Now not every button type in the world is going to use the content property to display the unique content inside the button. Check for a content mapper
                    // to determine what property to set in order to add the actual content. If no mapper can be found, assume we're using the content property
                    var contentProperty = "Content";

                    var mapper = binder.GetContentMapper(controlBinding.ControlType);
                    if (mapper != null)
                        contentProperty = mapper.ContentProperty;

                    control.SetProperty(contentProperty, text);

                    // Now check and see if there is a visibility attribute
                    var vizAttr = tba.Property.GetCustomAttribute<VisibilityAttribute>();
                    if (vizAttr != null)
                    {
                        var converter = vizAttr.ConverterType.SafeCreate<IValueConverter>();
                        control.CreateBinding("Visibility", vizAttr.Path ?? tba.Property.Name, converter: converter, converterParameter: vizAttr.ConverterParameter);
                    }

                    // Now that we're all setup go ahead and add the control to the toolbar
                    toolbar.Children.Add(control);
                }
            });
        }
    }
}
