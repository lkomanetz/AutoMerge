using System;
using System.Reflection;

namespace AutoMerge {

    public static class AutoMerge {

        public static void Merge<T>(
            ref T destination,
            T source,
            PropertyInfo propInfo = null
        ) where T : class, new() {    

        }

    }

}