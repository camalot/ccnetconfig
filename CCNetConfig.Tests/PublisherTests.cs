/*
 * Copyright (c) 2007-2009, Ryan Conrad. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * 
 * - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 * 
 * - Neither the name of the Camalot Designs nor the names of its contributors may be used to endorse or promote products derived from this software 
 *    without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
 * DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CCNetConfig.CCNet;
using System.Xml;

namespace CCNetConfig.Tests {
	[TestFixture]
	public class PublisherTests {

		[Test]
		public void SourceGearFortressPublisherTest () {
			SourceGearFortressPublisher sgfp = new SourceGearFortressPublisher ();
			sgfp.UseLabel = true;
			sgfp.Repository = "$";
			sgfp.Host = "myserver";
			sgfp.Username = "user";
			sgfp.Password.Password = "test";

			XmlElement node = sgfp.Serialize ();
			Assert.IsNotNull ( node, "Serialize failed for SourceGearFortressPublisher" );
			Assert.AreEqual ( node.SelectSingleNode ( "useLabel" ).InnerText, sgfp.UseLabel.ToString() );
			Assert.AreEqual ( node.SelectSingleNode ( "password" ).InnerText, sgfp.Password.GetPassword () );
			Assert.AreEqual ( node.SelectSingleNode ( "username" ).InnerText, sgfp.Username );
			Assert.AreEqual ( node.SelectSingleNode ( "repository" ).InnerText, sgfp.Repository );

			/*sgfp = new SourceGearFortressPublisher ();
			sgfp.Deserialize ( node );

			Assert.AreEqual ( node.SelectSingleNode ( "useLabel" ).InnerText, sgfp.UseLabel.ToString () );
			Assert.AreEqual ( node.SelectSingleNode ( "password" ).InnerText, sgfp.Password.GetPassword () );
			Assert.AreEqual ( node.SelectSingleNode ( "username" ).InnerText, sgfp.Username );
			Assert.AreEqual ( node.SelectSingleNode ( "repository" ).InnerText, sgfp.Repository );*/
		}
	}
}
