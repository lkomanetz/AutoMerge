using System.Reflection;

namespace AutoMerger {

	namespace MergeActions {

		internal interface IMergeAction {

			void Merge<T>(ref T destination, object source, PropertyInfo info = null); 

		}

	}

}
