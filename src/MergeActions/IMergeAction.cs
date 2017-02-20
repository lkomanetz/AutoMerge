using System;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public interface IMergeAction {

		void Merge<T>(ref T destination, T source, PropertyInfo propInfo = null) where T : class, new(); 

	}

}
