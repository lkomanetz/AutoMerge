using AutoMerger.MergeActions;
using System;

namespace AutoMerger {

    public static class AutoMerge {

        public static void Merge<T>(ref T destination, T source) where T : class, new() {    
            ReferenceMerge mergeAction = new ReferenceMerge(typeof(T));
            mergeAction.Merge(ref destination, source);
        }

    }

}