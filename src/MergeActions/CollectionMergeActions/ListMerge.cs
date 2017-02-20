using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ListMerge : CollectionMerge {

		public override void Merge<T>(ref T destination, T source, PropertyInfo propInfo = null) {
			throw new NotImplementedException();
		}

	}

}