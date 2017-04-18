using AutoMerger;
using System.Collections.Generic;
using Xunit;

namespace AutoMerger.UnitTests {

	public class DictionaryTests {

		[Fact]
		public void DictionaryWithValuesSucceeds() {
			Assert.True(true);
		}

		[Theory]
		[InlineDataAttribute(0,0)]
		[InlineDataAttribute(1,0)]
		[InlineDataAttribute(2,1)]
		[InlineDataAttribute(3,500)]
		public void DictionaryWithPrimitiveSucceeds(int key, int value) {
			DictionaryClass testA = new DictionaryClass();
			DictionaryClass testB = new DictionaryClass();
			int iterations = 10;

			for (int i = 0; i < iterations; ++i) {
				testA.PrimitiveDictionary.Add(key++, value++);
				testB.PrimitiveDictionary.Add(key++, value++);
			}

			AutoMerge.Merge(ref testA, testB);

			foreach (var kvp in testA.PrimitiveDictionary) {
				int valueB = testB.PrimitiveDictionary[kvp.Key];
				Assert.True(kvp.Value == valueB);
			}
		}

	}

	internal class DictionaryClass {

		public DictionaryClass() {
			this.PrimitiveDictionary = new Dictionary<int, int>();
			this.StringDictionary = new Dictionary<int, string>();
		}

		public IDictionary<int, int> PrimitiveDictionary { get; set; }
		public IDictionary<int, string> StringDictionary { get; set; }

	}

}