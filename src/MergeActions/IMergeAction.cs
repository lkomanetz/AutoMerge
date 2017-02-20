using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public interface IMergeAction {

		object Destination { get; }
		object Source { get; }
		PropertyInfo PropertyInfo { get; }

		void Merge(); 

	}

}
