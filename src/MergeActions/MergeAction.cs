using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class MergeAction : IMergeAction {

		public MergeAction(object source, object destination, PropertyInfo info) {
			this.Source = source;
			this.Destination = destination;
			this.PropertyInfo = info;
		}

		public object Source { get; private set; }
		public object Destination { get; internal set; }
		public PropertyInfo PropertyInfo { get; internal set; }

		public virtual void Merge() {}
	}
}