using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace WpfMagic.Xaml
{
	public class ResourceCache : ResourceDictionary
	{
		private const string BASE_URI_FORMAT = "pack://application:,,,/{0};component/{1}";

		private static IDictionary<Uri, ResourceDictionary> sharedDictionaries = new Dictionary<Uri, ResourceDictionary>();

		#region Referenced Assembly Property		
		
		private string _referencedAssembly;

		public string ReferencedAssembly
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_referencedAssembly))
					return Assembly.GetEntryAssembly().GetName().Name;

				return _referencedAssembly;
			}
			set { _referencedAssembly = value; }
		}

		#endregion		

		#region Dictionary Name Property

		public string DictionaryName
		{
			get { return null; }
			set
			{
				var dictionaryUri = new Uri(String.Format(BASE_URI_FORMAT, ReferencedAssembly, value));

				if (!sharedDictionaries.ContainsKey(dictionaryUri))
				{
					// If the dictionary is not yet loaded, load it by setting the source of the base class
					base.Source = dictionaryUri;

					// Add it to the cache
					sharedDictionaries.Add(dictionaryUri, this);
				}
				else
				{
					MergedDictionaries.Add(sharedDictionaries[dictionaryUri]);
				}
			}
		}

		#endregion
	}
}
