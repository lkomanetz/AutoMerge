using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public abstract class MergeAction : IMergeAction {

		protected IList<PropertyInfo> _properties;

		public MergeAction(object destination, object source, PropertyInfo info) {
			this.Source = source;
			this.Destination = destination;
			this.PropertyInfo = info;

			LoadPropertyList(info);
		}

		public object Source { get; private set; }
		public object Destination { get; protected set; }
		public PropertyInfo PropertyInfo { get; internal set; }

		public abstract void Merge();

		private void LoadPropertyList(PropertyInfo info) {
			TypeInfo typeInfo = (info != null) ? info.PropertyType.GetTypeInfo() : this.Source.GetType().GetTypeInfo();
			_properties = typeInfo.GetProperties(
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static
			);
		}

	}

}