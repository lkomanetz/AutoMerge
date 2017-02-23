using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace AutoMerger {

	namespace MergeActions {

		internal class ReferenceMerge : MergeAction {

			public ReferenceMerge(Type type = null) :
				base(type) {
				
				LoadPropertyList(type);
			}

			public override void Merge<T>(ref T destination, object source, PropertyInfo info = null) {
				if (source == null) {
					return;
				}

				if (destination == null) {
					destination = (T)source;
					return;
				}

				foreach (var property in _properties) {
					object sourceValue = property.GetValue(source);
					object destinationValue = null;
					if (destination != null) {
						destinationValue = property.GetValue(destination);
					}

					Type propertyType = property.PropertyType;
					TypeInfo typeInfo = propertyType.GetTypeInfo();

					bool isEnumerable = typeInfo.GetInterfaces().Any(x => x == typeof(IEnumerable));
					if (isEnumerable &&
						propertyType != typeof(String) &&
						propertyType != typeof(Object)) {

						if (typeInfo.IsArray) {
							new ArrayMerge().Merge(ref destinationValue, sourceValue, property);
							continue;
						}

						if (typeInfo.GetInterface("IDictionary`2") != null) {
							new DictionaryMerge().Merge(ref destinationValue, sourceValue, property);
						}
						else if (typeInfo.GetInterface("IList`1") != null) {
							new ListMerge().Merge(ref destinationValue, sourceValue, property);
						}

						continue;
					}

					if (!typeInfo.IsValueType && propertyType != typeof(String)) {
						new ReferenceMerge(propertyType).Merge(ref destinationValue, sourceValue, property);
						property.SetValue(destination, destinationValue);
						continue;
					}

					if (sourceValue != null) {
						new ValueMerge().Merge(ref destination, sourceValue, property);
					}
				}

			}

		}

	}

}