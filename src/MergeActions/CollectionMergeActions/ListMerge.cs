using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMerger.MergeActions {

	public class ListMerge : CollectionMerge {

		public ListMerge() {}

		public override void Merge<T>(ref T destination, object source, PropertyInfo info = null) {
			TypeInfo typeInfo = info.PropertyType.GetTypeInfo();
			typeof(ListMerge).GetTypeInfo()
				.GetMethod("MergeList", this.BindingFlags)
				.MakeGenericMethod(typeInfo.GetGenericArguments())
				.Invoke(this, new object[] { destination, source });
		}

		private void MergeList<T>(
			IList<T> destination,
			IList<T> source
		) {
			if (destination.Count == 0) {
				for (short i = 0; i < source.Count; ++i) {
					destination.Add(source[i]);
				}
				return;
			}

			for (short i = 0; i < source.Count; ++i) {
				destination[i] = source[i];
			}
		}

	}

}