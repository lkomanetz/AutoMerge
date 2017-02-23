using System;
using System.Reflection;

namespace AutoMerger.MergeActions {

	public abstract class CollectionMerge : MergeAction {

		public CollectionMerge() {}

		protected BindingFlags BindingFlags => BindingFlags.NonPublic | BindingFlags.Instance;

		public override void Merge<T>(ref T destination, T source, PropertyInfo info = null) {}

	}

}