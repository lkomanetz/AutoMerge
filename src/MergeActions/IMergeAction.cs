using System.Reflection;

namespace AutoMerger.MergeActions {

	public interface IMergeAction {

		void Merge<T>(ref T destination, object source, PropertyInfo info = null); 

	}

}
