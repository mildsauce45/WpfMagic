using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfMagic.Attributes;
using WpfMagic.Contracts;
using WpfMagic.Controls;
using WpfMagic.Extensions;

namespace WpfMagic.Bindings
{
    internal class ItemContainerBinder : IContentAreaBinder<ItemContainer>
    {
        public void BindContentArea(ItemContainer container, object viewModel, ViewBinder binder)
        {
            if (container == null || viewModel == null || binder == null)
                return;

            var vmType = viewModel.GetType();

            var vmProperties = binder.GetViewModelProperties(vmType);

            if (vmProperties == null)
                return;

            var itemsSources = vmProperties.Select(pi => new AttributeBinding<ItemsSourceAttribute>(pi.GetCustomAttribute<ItemsSourceAttribute>(), pi)).Where(a => a.Attr != null);
            var treeSources = vmProperties.Select(pi => new AttributeBinding<TreeSourceAttribute>(pi.GetCustomAttribute<TreeSourceAttribute>(), pi)).Where(a => a.Attr != null);

            BindItemSources(container, itemsSources, viewModel, binder);
            BindTreeViews(container, treeSources, viewModel, binder);
        }

        private void BindItemSources(ItemContainer container, IEnumerable<AttributeBinding<ItemsSourceAttribute>> itemsources, object viewModel, ViewBinder binder)
        {
            // We need to determine what property in the view model should be bound to this container

            // First check for any content area attributes on the marked up properties
            // Using the new attribute binding simplifies the linq in the next line
            var altBindings = itemsources.Select(isa => new AttributeBinding<ContentAreaAttribute>(isa.Property.GetCustomAttribute<ContentAreaAttribute>(), isa.Property));

            // Now check to see which of those has a content area that matches the name of this container
            var matchingBinding = altBindings.FirstOrDefault(ab => (string.IsNullOrWhiteSpace(container.ContentArea) && (ab.Attr == null || string.IsNullOrWhiteSpace(ab.Attr.ContentArea))) ||
                                                                   (ab.Attr != null && container.ContentArea == ab.Attr.ContentArea));

            // Switch back to dealing with the original attribute binding
            var itemsSource = itemsources.FirstOrDefault(isa => isa.Property == matchingBinding.Property);

            if (itemsSource == null)
                return;

            var collection = itemsSource.Property.GetValue(viewModel, null);

            if (collection == null)
                return;

            var control = binder.ResolveControlBinding(typeof(ItemsSourceAttribute));

            if (control == null)
                return;

            container.Content = control;

            // Get the template type of the object we're going to put inside the item container
            var templateType = collection.GetType().GetUnderlyingType();

            var template = itemsSource.Attr.Template;

            var dataTemplate = binder.GetDataTemplateBinding(templateType, template);
            if (dataTemplate != null)
                (control as ItemsControl).ItemTemplate = dataTemplate.Template;

            control.CreateBinding("ItemsSource", itemsSource.Property.Name);
        }

        private void BindTreeViews(ItemContainer container, IEnumerable<AttributeBinding<TreeSourceAttribute>> treesources, object viewModel, ViewBinder binder)
        {
            // We need to determine what property in the view model should be bound to this container

            // First check for any content area attributes on the marked up properties
            // Using the new attribute binding simplifies the linq in the next line
            var altBindings = treesources.Select(isa => new AttributeBinding<ContentAreaAttribute>(isa.Property.GetCustomAttribute<ContentAreaAttribute>(), isa.Property));

            // Now check to see which of those has a content area that matches the name of this container
            var matchingBinding = altBindings.FirstOrDefault(ab => (string.IsNullOrWhiteSpace(container.ContentArea) && (ab.Attr == null || string.IsNullOrWhiteSpace(ab.Attr.ContentArea))) ||
                                                                   (ab.Attr != null && container.ContentArea == ab.Attr.ContentArea));

            // Switch back to dealing with the original attribute binding
            var treeSource = treesources.FirstOrDefault(isa => isa.Property == matchingBinding.Property);

            if (treeSource == null)
                return;

            var collection = treeSource.Property.GetValue(viewModel, null);

            if (collection == null)
                return;

            var control = binder.ResolveControlBinding(typeof(TreeSourceAttribute));

            if (control == null)
                return;

            container.Content = control;

            control.CreateBinding("ItemsSource", treeSource.Property.Name);

            // Now we need to get the template type of teh objects we're going to put inside the tree view
            var templateType = collection.GetType().GetUnderlyingType();

            var ptOverride = treeSource.Attr.ParentTemplateOverride;

            var parentTemplate = binder.GetDataTemplateBinding(templateType, ptOverride ?? null);

            var treeView = control as TreeView;
            if (treeView != null)
                treeView.ItemTemplate = parentTemplate.Template;

            /// TODO: Make the child template stuff work appropriately without having to directly reference the child template from the parent template in XAML
            /// see HierarchichalTemplates.xaml for the example
        }
    }
}
