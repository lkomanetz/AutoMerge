using System;
using System.Reflection;

namespace AutoMerger {

	namespace MergeActions {

		internal abstract class CollectionMerge : MergeAction {

			internal CollectionMerge() {}

			protected BindingFlags BindingFlags => BindingFlags.NonPublic | BindingFlags.Instance;

			public override void Merge<T>(ref T destination, object source, PropertyInfo info = null) {}

		}

	}

}