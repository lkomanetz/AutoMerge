using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ArrayMerge : CollectionMerge {

		public ArrayMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info) {}

		public override void Merge() {
			throw new NotImplementedException();
		}

	}

}