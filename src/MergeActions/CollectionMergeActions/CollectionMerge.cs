using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public abstract class CollectionMerge : MergeAction {

		public CollectionMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info) {}

		protected BindingFlags BindingFlags => BindingFlags.NonPublic | BindingFlags.Instance;

		public override void Merge() {}

	}

}