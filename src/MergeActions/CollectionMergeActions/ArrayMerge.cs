using System;
using System.Reflection;

namespace AutoMerger.MergeActions {

	public class ArrayMerge : CollectionMerge {

		public ArrayMerge() {}

		public override void Merge<T>(ref T destination, T source, PropertyInfo info = null) {
			typeof(ArrayMerge).GetTypeInfo()
				.GetMethod("MergeArray", this.BindingFlags)
				.Invoke(this, new object[] { destination, source });
		}


		private void MergeArray(Array destination, Array source) {
			if (destination == null) {
				destination = source;
			}

			for (short i = 0; i < source.Length; ++i) {
				destination.SetValue(source.GetValue(i), i);
			}
		}

	}

}