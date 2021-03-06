using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMerger {

	namespace MergeActions {

		internal abstract class MergeAction : IMergeAction {

			protected IList<PropertyInfo> _properties;
			private BindingFlags propertyFlags =
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

			internal MergeAction(Type type = null) { }

			public abstract void Merge<T>(ref T destination, object source, PropertyInfo info = null);

			protected void LoadPropertyList(Type type) {
				_properties = type.GetTypeInfo().GetProperties(propertyFlags);
			}

		}

	}

}