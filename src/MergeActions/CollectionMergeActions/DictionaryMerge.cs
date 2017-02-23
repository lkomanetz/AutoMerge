using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMerger.MergeActions {

	internal class DictionaryMerge : CollectionMerge {

		public DictionaryMerge() {}

		public override void Merge<T>(ref T destination, object source, PropertyInfo info = null) {
			TypeInfo typeInfo = info.PropertyType.GetTypeInfo();
			typeof(DictionaryMerge).GetTypeInfo()
				.GetMethod("MergeDictionary", this.BindingFlags)
				.MakeGenericMethod(typeInfo.GetGenericArguments())
				.Invoke(this, new object[] { destination, source });
		}

		private void MergeDictionary<TKey, TValue>(
			IDictionary<TKey, TValue> destination,
			IDictionary<TKey, TValue> source
		) {
			foreach (var kvp in source) {
				if (destination.ContainsKey(kvp.Key)) {
					destination[kvp.Key] = kvp.Value;
				}
				else {
					destination.Add(kvp);
				}
			}
		}

	}

}