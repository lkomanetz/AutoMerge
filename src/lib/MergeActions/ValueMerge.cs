using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ValueMerge : MergeAction {

		public ValueMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info)
		{ }

		public override void Merge() {
			if (this.Destination != this.Source) {
				this.Destination = this.Source;
			}
		}

	}

}