using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WpfMagic.Attributes;
using WpfMagic.Bindings;
using WpfMagic.Contracts;
using WpfMagic.Extensions;
using WpfMagic.Mappers;
using WpfMagic.Mvvm;

namespace WpfMagic
{
    public class ViewBinder
    {
        #region Type Caches

        private IList<Type> loadedTypes;
        private IDictionary<Type, IList<PropertyInfo>> viewModelProperties;

        private IDictionary<Type, ControlTypeBinding> controlBindings;
        private IDictionary<Type, ContentMapper> contentMappers;

        private IDictionary<Type, IList<DataTemplateBinding>> dataBindings;
        private IDictionary<string, DataTemplate> untypedTemplates;

        #endregion

        #region Properties

        internal IRootView ViewRoot { get; set; }

        #endregion

        #region Singleton Implementation

        private static ViewBinder me;
        public static ViewBinder Instance
        {
            get
            {
                if (me == null)
                    me = new ViewBinder();

                return me;
            }
        }

        #endregion

        #region Constructors

        private ViewBinder()
        {
            loadedTypes = new List<Type>();
            viewModelProperties = new Dictionary<Type, IList<PropertyInfo>>();
            controlBindings = new Dictionary<Type, ControlTypeBinding>();
            contentMappers = new Dictionary<Type, ContentMapper>();
            dataBindings = new Dictionary<Type, IList<DataTemplateBinding>>();
            untypedTemplates = new Dictionary<string, DataTemplate>();

            InitializeTypes();
        }

        #endregion

        #region Display

		public FrameworkElement DisplayActive<T>() where T : NotifyableObject
		{
			var viewModel = ActiveViewModelFactory.CreateInstance<T>();

			return Display(viewModel);
		}

        public FrameworkElement Display(object viewModel)
        {
            var view = GetView(viewModel);

            if (view == null)
                return null;

            return Display(view, viewModel);
        }

        public FrameworkElement Display(FrameworkElement view, object viewModel)
        {
            if (viewModel == null)
                return null;

            view.DataContext = viewModel;

            BindContentAreas(view, viewModel);

            if (ViewRoot != null)
                ViewRoot.Content = view;

            return view;
        }

        private void BindContentAreas(FrameworkElement view, object viewModel)
        {
            // Get the list of controls that our binder cares about, specifically anything that implements IContentAreaProvider
            var contentAreas = view.LogicalChildrenOfType<IContentAreaProvider>();

            var cabType = typeof(IContentAreaBinder<>);

            // Now we need to get a list of any contentareabinders that we can use to actually bind the list content areas we've found in the view
            var possibleBinderTypes = loadedTypes.Where(t => t.Implements(cabType) && t != cabType);

            foreach (var ca in contentAreas)
            {
                // Get the binder that implements the proper interface
                var binderType = possibleBinderTypes.FirstOrDefault(t => t.GetInterface("IContentAreaBinder`1").GetUnderlyingType() == ca.GetType());
                if (binderType != null)
                {
                    var binder = Activator.CreateInstance(binderType);

                    var mi = binderType.GetMethod("BindContentArea");
                    if (mi != null)
                        mi.Invoke(binder, new[] { ca, viewModel, this });
                }
            }
        }

        /// <summary>
        /// This method allows you to switch out the type of control that is used when encountering particular attributes in the view model
        /// </summary>
        /// <typeparam name="T">The control binding that you'd like to switch out. e.g. ToolbarActionAttribute</typeparam>
        /// <typeparam name="R">The control you'd like to use. e.g. Your own custom button instead of the WPF Button</typeparam>
        public void RebindControlType<T, R>()
            where T : IControlBinding
            where R : UIElement
        {
            var controlType = typeof(T);

            if (!controlBindings.ContainsKey(controlType))
                controlBindings.Add(controlType, new ControlTypeBinding(typeof(R)));
            else
                controlBindings[controlType] = new ControlTypeBinding(typeof(R));
        }

        #endregion

        #region Initialization

        private void InitializeTypes()
        {
            loadedTypes.AddRange(AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()));

            BindControlTypes();
            InitializeMappers();
            InitializeDataTemplateBindings();
        }

        private void BindControlTypes()
        {
            controlBindings.Add(typeof(ToolbarActionAttribute), new ControlTypeBinding(typeof(Button), true, true));
            controlBindings.Add(typeof(ItemsSourceAttribute), new ControlTypeBinding(typeof(ItemsControl), false, false));
            controlBindings.Add(typeof(TreeSourceAttribute), new ControlTypeBinding(typeof(TreeView), false, false));
        }

        private void InitializeMappers()
        {
            var contentMapperType = typeof(ContentMapper);

            var contentMapperTypes = loadedTypes.Where(t => contentMapperType.IsAssignableFrom(t) && t != contentMapperType).ToList();

            foreach (var mapperType in contentMapperTypes)
            {
                var mapper = mapperType.SafeCreate<ContentMapper>();

                contentMappers.Add(mapper.ControlType, mapper);
            }
        }

        private void InitializeDataTemplateBindings()
        {
            dataBindings = new Dictionary<Type, IList<DataTemplateBinding>>();

            var list = new List<DataTemplateBinding>();

            // Add the raw datatemplates in the App.xaml Resource Dictionary
            AddDataTemplates(Application.Current.Resources, list);

            // Now add all the data templates from any merged resource dictionaries
            foreach (var rd in Application.Current.Resources.MergedDictionaries)
                AddDataTemplates(rd, list);

            // Group the templates by type so we can add them into the static dictionary
            var groupedTemplates = from dtb in list
                                   group dtb by dtb.Type into g
                                   select g;

            foreach (var g in groupedTemplates)
                dataBindings.Add(g.Key, g.ToList());
        }

        private void AddDataTemplates(ResourceDictionary rd, List<DataTemplateBinding> results)
        {
            foreach (var key in rd.Keys)
            {
                var dt = rd[key] as DataTemplate;

                if (dt == null)
                    continue;

                if (dt.DataType != null)
                    results.Add(new DataTemplateBinding(dt.DataType as Type, key is string ? (string)key : (string)null, dt));
                else if (key is string)
                {
                    var stringKey = (string)key;

                    if (untypedTemplates.ContainsKey(stringKey))
                        untypedTemplates[stringKey] = dt;
                    else
                        untypedTemplates.Add(stringKey, dt);
                }
            }
        }

        #endregion

        #region Helper Methods

        private FrameworkElement GetView(object viewModel)
        {
            if (viewModel == null)
                return null;

            var type = viewModel.GetType();

			if (type.IsActiveViewModel())
				type = type.BaseType;

            // The View attribute will override the convention we have below for auto binding views to view models
            // This way similarly named view models can point at the correct view without any crazy logic
            var attr = type.GetCustomAttribute<ViewAttribute>();

            if (attr != null)
            {
                var matches = loadedTypes.Where(t => t.Name == attr.Name);

                if (!matches.IsNullOrEmpty())
                {
                    if (matches.Count() == 1)
                        return matches.First().SafeCreate<FrameworkElement>();
                    else
                        // We can try more betterer matching somehow, but for now throw the exception
                        throw new AmbiguousViewTypeException(string.Format("More than one type of view with the name {0}", attr.Name));
                }
            }

            // Out of the box we have a convention that finds the view to bind to the given view model
            // The convention is the View's name will be named the same except it will be missing the ViewModel suffix
            // (e.g. MainWindow should bind to MainWindowViewModel)
            var conventionMatches = loadedTypes.Where(t => t.Name == type.Name.Replace("ViewModel", string.Empty));

            if (conventionMatches != null && conventionMatches.Any())
            {
                if (conventionMatches.Count() == 1)
                    return conventionMatches.First().SafeCreate<FrameworkElement>();
                else
                    throw new AmbiguousViewTypeException(string.Format("More than one type view with the name {0}.", type.Name.Replace("ViewModel", string.Empty)));
            }

            return null;
        }

        internal FrameworkElement ResolveControlBinding(Type type)
        {
            if (!controlBindings.ContainsKey(type))
                return null;

            var controlBinding = this.controlBindings[type];

            return controlBinding.ControlType.SafeCreate<FrameworkElement>();
        }

        #endregion

        #region Internal Binder Cache Accessors

        internal IList<PropertyInfo> GetViewModelProperties(Type vmType)
        {
            if (vmType == null)
                return null;

            if (!viewModelProperties.ContainsKey(vmType))
                viewModelProperties.Add(vmType, vmType.GetProperties().ToList());

            return viewModelProperties[vmType];
        }

        internal ControlTypeBinding GetControlBinding(Type type)
        {
            if (controlBindings.ContainsKey(type))
                return controlBindings[type];

            return null;
        }

        internal DataTemplate GetUntypedTemplate(string template)
        {
            return untypedTemplates.ContainsKey(template) ? untypedTemplates[template] : null;
        }

        internal DataTemplateBinding GetDataTemplateBinding(Type templateType, string template)
        {
            return dataBindings[templateType].First(dtb => string.IsNullOrWhiteSpace(template) ? dtb.IsDefault : dtb.Key == template);
        }

        internal ContentMapper GetContentMapper(Type type)
        {
            if (contentMappers.ContainsKey(type))
                return contentMappers[type];

            return null;
        }

        #endregion
    }
}
