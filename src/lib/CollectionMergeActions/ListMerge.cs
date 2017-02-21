using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ListMerge : CollectionMerge {

		public ListMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info) {}

		public override void Merge() {
			TypeInfo typeInfo = this.PropertyInfo.PropertyType.GetTypeInfo();
			typeof(ListMerge).GetTypeInfo()
				.GetMethod("MergeList", BindingFlags.NonPublic)
				.MakeGenericMethod(typeInfo.GetGenericArguments())
				.Invoke(null, new object[] { this.Destination, this.Source });
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