using System.Reflection;

namespace AutoMerger.MergeActions {

	public interface IMergeAction {

		object Destination { get; }
		object Source { get; }
		PropertyInfo PropertyInfo { get; }

		void Merge(); 

	}

}
