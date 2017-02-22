using AutoMerger.MergeActions;
using System;
using System.Reflection;

namespace AutoMerger {

    public static class AutoMerge {

        public static void Merge<T>(
            ref T destination,
            T source,
            PropertyInfo propInfo = null
        ) where T : class, new() {    

            ReferenceMerge mergeAction = new ReferenceMerge(destination, source, propInfo, typeof(T));
            mergeAction.Merge();
        }

    }

}