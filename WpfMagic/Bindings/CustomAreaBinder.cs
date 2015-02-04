using System.Linq;
using WpfMagic.Attributes;
using WpfMagic.Contracts;
using WpfMagic.Controls;
using WpfMagic.Extensions;

namespace WpfMagic.Bindings
{
    internal class CustomAreaBinder : IContentAreaBinder<CustomArea>
    {
        public void BindContentArea(CustomArea customArea, object viewModel, ViewBinder binder)
        {
            if (customArea == null || viewModel == null || binder == null)
                return;

            var vmType = viewModel.GetType();

            var vmProperties = binder.GetViewModelProperties(vmType);

            if (vmProperties == null)
                return;

            var propertyContentAreaBindings = vmProperties.Select(pi => new AttributeBinding<CustomAreaAttribute>(pi.GetCustomAttribute<CustomAreaAttribute>(), pi)).Where(a => a.Attr != null);            

            // Property bindings always take precedence over class level bindings
            var propertyBinding = propertyContentAreaBindings.Where(a => a.Attr.ContentArea == customArea.ContentArea).FirstOrDefault();
            if (propertyBinding != null)
            {
                customArea.ContentTemplate = binder.GetUntypedTemplate(propertyBinding.Attr.Template);
                customArea.Content = propertyBinding.Property.GetValue(viewModel, null);
            }
            else
            {
                // Since no properties matched this custom area we need to check the view model itself to see if it should be bound to the custom area
                var classContentAreaBindings = vmType.GetCustomAttributes(typeof(CustomAreaAttribute), false).OfType<CustomAreaAttribute>();
                
                var contentArea = classContentAreaBindings.Where(caa => caa.ContentArea == customArea.ContentArea);
                if (contentArea == null)
                    return;

                // The view model needs to be bound to the custom area so now lets figure out what template to use
                var customAreaAttr = vmType.GetCustomAttribute<CustomAreaAttribute>();
                if (customAreaAttr == null)
                    return;

                customArea.ContentTemplate = binder.GetUntypedTemplate(customAreaAttr.Template);
                customArea.Content = viewModel;
            }
        }
    }
}
