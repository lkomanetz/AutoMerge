using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public abstract class CollectionMerge : IMergeAction {

		public abstract void Merge<T>(ref T destination, T source, PropertyInfo propInfo = null) where T : class, new();

	}

}