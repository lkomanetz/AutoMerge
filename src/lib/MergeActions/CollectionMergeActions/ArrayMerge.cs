using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ArrayMerge : CollectionMerge {

		public ArrayMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info) {}

		public override void Merge() {
			Array tempDestination = (Array)this.Destination;
			Array tempSource = (Array)this.Source;
			typeof(ArrayMerge).GetTypeInfo()
				.GetMethod("MergeArray", this.BindingFlags)
				.Invoke(this, new object[] { tempDestination, tempSource });
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