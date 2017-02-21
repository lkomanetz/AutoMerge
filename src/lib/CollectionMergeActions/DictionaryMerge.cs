using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class DictionaryMerge : CollectionMerge {

		public DictionaryMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info) {}

		public override void Merge() {
			TypeInfo typeInfo = this.PropertyInfo.PropertyType.GetTypeInfo();
			typeof(DictionaryMerge).GetTypeInfo()
				.GetMethod("MergeDictionary", BindingFlags.NonPublic)
				.MakeGenericMethod(typeInfo.GetGenericArguments())
				.Invoke(null, new object[] { this.Destination, this.Source });
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