using System;
using System.Reflection;

namespace AutoMerger.MergeActions {

	public class ValueMerge {

		public ValueMerge() { }

		public void Merge<T>(ref T destination, object source, PropertyInfo info = null) {
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