using AutoMerger;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConsoleApplication {

    public class AutoMergeTests {

		[Fact]
        public void AutoMerge_ChangingSimpleValuesSucceeds() {
            TestObject previousObj = BuildObjectStructure();
            TestObject currentObj = BuildObjectStructure();

            currentObj.TestString = "NewString";
            currentObj.TestValue = 2;
            currentObj.TestBool = !currentObj.TestBool;

            AutoMerge.Merge(ref previousObj, currentObj);
            bool isEqual = (
                previousObj.TestString.Equals(currentObj.TestString) &&
                previousObj.TestValue == currentObj.TestValue &&
                previousObj.TestBool == currentObj.TestBool
            );

            Assert.True(isEqual, "previousObj values do not equal currentObj values.");
        }

        [Fact]
		public void AutoMerge_ReferenceTypeChangesSucceeds()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();

			currentObj.TestReference.TestNullableValue = 100;
			currentObj.TestReference.TestCollectionOfValues.Reverse();
			currentObj.TestReference.TestArrayOfValue[0] = 100;

			AutoMerge.Merge(ref previousObj, currentObj);
			bool isEqual = (
				previousObj.TestReference.TestNullableValue == currentObj.TestReference.TestNullableValue &&
				Enumerable.SequenceEqual(previousObj.TestReference.TestArrayOfValue, currentObj.TestReference.TestArrayOfValue) &&
				Enumerable.SequenceEqual(previousObj.TestReference.TestCollectionOfValues, currentObj.TestReference.TestCollectionOfValues)
			);

			Assert.True(isEqual, "previousObj values do not equal currentObj values.");
		}

		[Fact]
		public void AutoMerge_DestinationEnumerableIsUntouchedWhenSourceIsEmpty()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();
			int countPriorToChange = currentObj.TestCollectionOfReferences.Count;
			int arrayCountPriorToChange = currentObj.TestArrayOfReferences.Length;
			previousObj.TestCollectionOfReferences = new List<AnotherTestObject>();
			previousObj.TestArrayOfReferences = new AnotherTestObject[0];

			AutoMerge.Merge(ref currentObj, previousObj);
			bool countsAreNotChanged = (
				currentObj.TestCollectionOfReferences.Count == countPriorToChange &&
				currentObj.TestArrayOfReferences.Length == arrayCountPriorToChange
			);

			Assert.True(
				countsAreNotChanged,
				$"Collection count inaccurate.  Expected {countPriorToChange}\nActual {currentObj.TestCollectionOfReferences.Count}"
			);
		}

		[Fact]
		public void AutoMerge_NullValueIsIgnored()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();

			currentObj.TestReference.TestNullableValue = null;
			AutoMerge.Merge(ref previousObj, currentObj);

			int? value = previousObj.TestReference.TestNullableValue;
			Assert.True(
				value != null && value.Value == 1,
				"Nullable value type was overridden."
			);
		}

		[Fact]
		public void AutoMerge_CollectionMergesSuccessfully()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();

			previousObj.TestReference.TestCollectionOfValues = new List<int>();
			AutoMerge.Merge(ref previousObj, currentObj);
			Assert.True(
				Enumerable.SequenceEqual(previousObj.TestReference.TestCollectionOfValues, currentObj.TestReference.TestCollectionOfValues),
				"Collections are not equal."
			);
		}

		[Fact]
		public void AutoMerge_NullSourceValueSucceeds()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();

			currentObj.TestReference = null;
			AutoMerge.Merge(ref previousObj, currentObj);

			Assert.True(previousObj.TestReference != null);
		}

		[Fact]
		public void AutoMerge_NullDestinationValueSucceeds()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();

			previousObj.TestReference = null;
			AutoMerge.Merge(ref previousObj, currentObj);

			Assert.True(
                previousObj.TestReference != null && currentObj.TestReference != null,
                "previousObj.TestReference is still null or not equal to currentObj.TestReference"
            );
		}

		[Fact]
		public void AutoMerge_CollectionOfReferencesSucceeds()
		{
			int? newNullableValue = 1000;
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();

			currentObj.TestCollectionOfReferences[0].TestNullableValue = newNullableValue;
			AutoMerge.Merge(ref previousObj, currentObj);

			Assert.True(
				previousObj.TestCollectionOfReferences[0].TestNullableValue == newNullableValue,
				$"Expected {newNullableValue}\nWas {previousObj.TestCollectionOfReferences[0].TestNullableValue}"
			);
		}

		[Fact]
		public void AutoMerge_ArrayOfReferencesSucceeds()
		{
			int? newNullableValue = 1000;
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();

			previousObj.TestArrayOfReferences[0].TestNullableValue = newNullableValue;
			AutoMerge.Merge(ref currentObj, previousObj);

			Assert.True(
				currentObj.TestArrayOfReferences[0].TestNullableValue == newNullableValue,
				$"Expected {newNullableValue}\nActual {currentObj.TestArrayOfReferences[0].TestNullableValue}."
			);
		}

		[Fact]
		public void AutoMerge_CollectionOfDifferentLengthsSucceeds()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();
			int countBeforeMerge = currentObj.TestCollectionOfReferences.Count;

			previousObj.TestCollectionOfReferences.RemoveAt(countBeforeMerge - 1);
			previousObj.TestCollectionOfReferences[0].TestNullableValue = null;

			AutoMerge.Merge(ref currentObj, previousObj);
			bool isTheSame = (
				currentObj.TestCollectionOfReferences.Count == countBeforeMerge &&
				currentObj.TestCollectionOfReferences[0].TestNullableValue == null
			);

			Assert.True(isTheSame, "TestCollectionOfReferences count is incorrect.");
		}

		[Fact]
		public void AutoMerge_ArrayOfDifferentLengthsSucceeds()
		{
			TestObject previousObj = BuildObjectStructure();
			TestObject currentObj = BuildObjectStructure();
			int countBeforeMerge = currentObj.TestArrayOfReferences.Length;

			AnotherTestObject obj = previousObj.TestArrayOfReferences[0];
			obj.TestNullableValue = null;
			previousObj.TestArrayOfReferences = new AnotherTestObject[] { obj };

			AutoMerge.Merge(ref currentObj, previousObj);
			bool isSuccessfulTest = (
				currentObj.TestArrayOfReferences.Length == countBeforeMerge &&
				currentObj.TestArrayOfReferences[0].TestNullableValue == null
			);

			Assert.True(
				isSuccessfulTest,
				$"Expected array size {countBeforeMerge}\nActual {currentObj.TestArrayOfReferences.Length}"
			);
		}

        private TestObject BuildObjectStructure() {
            return new TestObject() {
                TestString = "TestString",
                TestValue = 1,
                TestBool = true,
                TestReference = new AnotherTestObject() {
                    TestNullableValue = 1,
                    TestCollectionOfValues = new List<int>() { 1, 2, 3 },
                    TestArrayOfValue = new int[] { 1, 2, 3 }
                },
                TestArrayOfReferences = new AnotherTestObject[] {
                    new AnotherTestObject() {
                        TestNullableValue = 2,
                        TestCollectionOfValues = new List<int>() { 4, 5, 6 },
                        TestArrayOfValue = new int[] { 4, 5, 6 }
                    },
                    new AnotherTestObject() {
                        TestNullableValue = 2,
                        TestCollectionOfValues = new List<int>() { 4, 5, 6 },
                        TestArrayOfValue = new int[] { 4, 5, 6 }
                    }
                },
                TestCollectionOfReferences = new List<AnotherTestObject>() {
                    new AnotherTestObject() {
                        TestNullableValue = 3,
                        TestCollectionOfValues = new List<int>() { 7, 8, 9 },
                        TestArrayOfValue = new int[] { 7, 8, 9 }
                    },
                    new AnotherTestObject() {
                        TestNullableValue = 3,
                        TestCollectionOfValues = new List<int>() { 7, 8, 9 },
                        TestArrayOfValue = new int[] { 7, 8, 9 }
                    }
                },
                TestDictionary = new Dictionary<int, string>() {
                    { 1, "Test" },
                    { 2, "AnotherTest" }
                }
            };
        }

    }

    internal class TestObject {
        internal string TestString { get; set; }
        internal int TestValue { get; set; }
        internal bool TestBool { get; set; }
        internal AnotherTestObject TestReference { get; set; }
        internal AnotherTestObject[] TestArrayOfReferences { get; set; }
        internal List<AnotherTestObject> TestCollectionOfReferences { get; set; }
        internal Dictionary<int, string> TestDictionary { get; set; }
    }

    internal class AnotherTestObject {
        internal int? TestNullableValue { get; set; }
        internal List<int> TestCollectionOfValues { get; set; }
        internal int[] TestArrayOfValue { get; set; }

        public static bool operator ==(AnotherTestObject a, AnotherTestObject b) {
            if (ReferenceEquals(a, b)) {
                return true;
            }

            if (((object)a == null) || ((object)b == null)) {
                return false;
            }

            bool isEqual = a.TestCollectionOfValues.Count == b.TestCollectionOfValues.Count;
            if (!isEqual) {
                return false;
            }

            for (short i = 0; i < a.TestCollectionOfValues.Count; ++i) {
                isEqual = a.TestCollectionOfValues[i] == b.TestCollectionOfValues[i];
                if (!isEqual) {
                    break;
                }
            }

            if (!isEqual) {
                return false;
            }

            for (short i = 0; i < a.TestArrayOfValue.Length; ++i) {
                isEqual = a.TestArrayOfValue[i] == b.TestArrayOfValue[i];
                if (!isEqual) {
                    break;
                }
            }

            return isEqual;
        }

        public static bool operator !=(AnotherTestObject a, AnotherTestObject b) {
            return !(a == b);
        }

		public override bool Equals(object a) {
			AnotherTestObject tempA = (AnotherTestObject)a;
			return this == tempA;
		}

        public override int GetHashCode() {
            if (TestNullableValue.HasValue) {
                return base.GetHashCode() ^ TestNullableValue.Value;
            }

            return base.GetHashCode();
        }

    }

}