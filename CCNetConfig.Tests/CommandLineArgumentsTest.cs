using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CCNetConfig.Updater.Core;

namespace CCNetConfig.Tests {
	[TestFixture]
	public class CommandLineArgumentsTest {
		[Test]
		public void TestBasicArguments () {
			string[] s = "/foo=bar /test /blah=false".Split ( new char[] { ' ' } );
			CommandLineArguments args = new CommandLineArguments ( s );

			Assert.IsTrue ( args.ContainsParam ( "foo" ) );
			Assert.IsTrue ( args.ContainsParam ( "test" ) );
			Assert.IsTrue ( args.ContainsParam ( "blah" ) );
			Assert.AreEqual ( args[ "foo" ], "bar" );
			Assert.IsFalse ( args.ContainsParam ( "noarg" ) );

		}
	}
}
