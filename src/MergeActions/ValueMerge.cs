using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ValueMerge : IMergeAction {

		public void Merge<T>(ref T destination, T source, PropertyInfo propInfo = null) where T : class, new() {
			if (destination != source) {
				destination = source;
			}
		}

	}

}