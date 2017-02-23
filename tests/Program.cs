using System;

namespace ConsoleApplication {

	public class Program {
		public static void Main(string[] args) {
			AutoMergeTests testSuite = new AutoMergeTests();
			testSuite.AutoMerge_NullDestinationValueSucceeds();
		}

	}

}