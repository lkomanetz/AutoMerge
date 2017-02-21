using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoMerge.MergeActions {

	public class ReferenceMerge : MergeAction {

		private IList<MergeAction> _mergeActions;

		public ReferenceMerge(object destination, object source, PropertyInfo info) :
			base(destination, source, info) {
			
			_mergeActions = new List<MergeAction>();
		}

		public override void Merge() {
			base.Merge();

			foreach (var property in _properties) {
				object sourceValue = property.GetValue(this.Source);
				object destinationValue = null;
				if (this.Destination != null) {
					destinationValue = property.GetValue(this.Destination);
				}

				Type propertyType = property.PropertyType;
				TypeInfo typeInfo = propertyType.GetTypeInfo();

				if (typeInfo.IsValueType || propertyType == typeof(String)) {
					_mergeActions.Add(new ValueMerge(destinationValue, sourceValue, property));
					continue;
				}

				bool isEnumerable = typeInfo.GetInterfaces().Any(x => x == typeof(IEnumerable));
				if (isEnumerable &&
					propertyType != typeof(String) &&
					propertyType != typeof(Object)) {

					if (typeInfo.IsArray) {
						_mergeActions.Add(new ArrayMerge(destinationValue, sourceValue, property));
						continue;
					}

					if (typeInfo.GetInterface("IDictionary`2") != null) {
						_mergeActions.Add(new DictionaryMerge(destinationValue, sourceValue, property));
					}
					else if (typeInfo.GetInterface("IList`1") != null) {
						_mergeActions.Add(new ListMerge(destinationValue, sourceValue, property));
					}

					continue;
				}

				if (!typeInfo.IsValueType && propertyType != typeof(String)) {
					_mergeActions.Add(new ReferenceMerge(destinationValue, sourceValue, property));
					continue;
				}
			}

			foreach (var action in _mergeActions) {
				action.Merge();
			}
		}

	}

}