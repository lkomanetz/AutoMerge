using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class MergeAction : IMergeAction {

		protected IList<PropertyInfo> _properties;

		public MergeAction(object source, object destination, PropertyInfo info) {
			this.Source = source;
			this.Destination = destination;
			this.PropertyInfo = info;

			LoadPropertyList(info);
		}

		public object Source { get; private set; }
		public object Destination { get; internal set; }
		public PropertyInfo PropertyInfo { get; internal set; }

		public virtual void Merge() {
			if (this.Source == null) {
				return;
			}

			if (this.Destination == null) {
				this.Destination = this.Source;
			}
		}

		private void LoadPropertyList(PropertyInfo info) {
			TypeInfo typeInfo = (info != null) ? info.PropertyType.GetTypeInfo() : this.Source.GetType().GetTypeInfo();
			_properties = typeInfo.GetProperties(
				BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static
			);
		}

	}

}