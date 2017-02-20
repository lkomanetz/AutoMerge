using AutoMerge.MergeActions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMerge {

    public static class AutoMerge {

        public static void Merge<T>(
            ref T destination,
            T source,
            PropertyInfo propInfo = null
        ) where T : class, new() {    

            IList<MergeAction> mergeActions = new List<MergeAction>();
            Type objType = (propInfo != null) ? propInfo.PropertyType : typeof(T);
            TypeInfo typeInfo = objType.GetTypeInfo();

            if (source == null) {
                return;
            }

            if (destination == null || typeInfo.IsValueType) {
                var action = new ValueMerge(destination, source, null);
                action.Merge();
                return;
            }

            IList<PropertyInfo> properties = typeInfo.GetProperties(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static
            );

            foreach (PropertyInfo property in properties) {
                object sourceValue = property.GetValue(source);
                object destinationValue = null;

                if (destination != null) {
                    destinationValue = property.GetValue(destination);
                }

                Type propertyType = property.GetType();
                TypeInfo propTypeInfo = propertyType.GetTypeInfo();
                if (propTypeInfo.IsValueType ||
                    propertyType == typeof(String)) {

                    mergeActions.Add(new ValueMerge(destinationValue, sourceValue, property));
                    continue;
                }

                bool isEnumerable = typeInfo.GetInterfaces()
                    .Any(x => x == typeof(IEnumerable));

                /*
                 * The thing to keep in mind is that since IDictionary and IList are of type
                 * System.Object, I want to make sure the enumerable is in fact one of those two.
                 * Also, the String object is considered an enumerable since it is a char[].  I
                 * handle strings like value types.
                 */
                if (isEnumerable &&
                    propertyType != typeof(String) &&
                    propertyType != typeof(object)) {

                    if (propTypeInfo.IsArray) {
                        mergeActions.Add(new ArrayMerge(destinationValue, sourceValue, property));
                        continue;
                    }

                    if (propTypeInfo.GetInterface("IDictionary`2") != null) {
                        mergeActions.Add(new DictionaryMerge(destinationValue, sourceValue, property));
                    }
                    else if (propTypeInfo.GetInterface("IList`1") != null) {
                        mergeActions.Add(new ListMerge(destinationValue, sourceValue, property));
                    }
                    
                    continue;
                }

                if (!propTypeInfo.IsValueType && propertyType != typeof(String)) {
                    mergeActions.Add(new ReferenceMerge(destinationValue, sourceValue, property));
                    continue;
                }
            }

            foreach (MergeAction action in mergeActions) {
                action.Merge();
            }
        }

    }

}