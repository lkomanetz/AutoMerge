using System;
using System.Reflection;

namespace AutoMerger {

	namespace MergeActions {

		internal class ValueMerge : MergeAction {

			public ValueMerge() { }

			public override void Merge<T>(ref T destination, object source, PropertyInfo info = null) {
				if (source == null) {
					return;
				}

				if (info != null) {
					info.SetValue(destination, source);
				}
				else {
					destination = (T)source;
				}
			}

		}

	}

}