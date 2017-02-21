using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ValueMerge : MergeAction {

		public ValueMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info)
		{ }

		public override void Merge() {
			if (this.Source == null) {
				return;
			}

			if (this.Destination == null || this.Destination != this.Source) {
				this.PropertyInfo?.SetValue(this.Destination, this.Source);
			}
		}

	}

}