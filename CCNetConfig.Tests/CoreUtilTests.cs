/*
 * Copyright (c) 2007-2008, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core;
using CCNetConfig.CCNet;
using CCNetConfig.Core.Components;
using System.Reflection;
using System.ComponentModel;
using System.Xml;

namespace CCNetConfig.Tests {
  [TestFixture]
  public class CoreUtilTests {
   
    [Test]
    public void TypeDescriptionProvidersTest ( ) {
      Util.RegisterTypeDescriptionProviders ( new Version ( "1.3" ) );
      Assert.IsNotNull ( Util.TypeDescriptionProviders, "TypeDescriptionProviders Is Null" );
    }

    [Test]
    public void PublisherTasksTest ( ) {
      Assert.IsNotNull ( Util.PublisherTasks, "PublisherTasks Is Null" );
    }

    [Test]
    public void SourceControlsTest ( ) {
      Assert.IsNotNull ( Util.SourceControls, "SourceControls Is Null" );
    }

    [Test]
    public void LabellersTest ( ) {
      Assert.IsNotNull ( Util.Labellers, "Labellers Is Null" );
    }

    [Test]
    public void TriggersTest ( ) {
      Assert.IsNotNull ( Util.Triggers, "Triggers Is Null" );
    }

    [Test]
    public void StatesTest ( ) {
      Assert.IsNotNull ( Util.States, "States Is Null" );
    }

    [Test]
    public void ProjectExtensionsTest ( ) {
      Assert.IsNotNull ( Util.ProjectExtensions, "ProjectExtensions Is Null" );
    }
    [Test]
    public void CurrentConfigurationVersionTest ( ) {
      Assert.IsNotNull ( Util.CurrentConfigurationVersion );
    }
    [Test]
    public void CheckRequiredTest ( ) {
      NullTask task = new NullTask ( );

      // check alienbrain
      try {
        Assert.IsNotNull ( Util.CheckRequired ( task, "alienbrainUri", new AlienbrainUri ( "ab://google.com" ) ) );
        Assert.IsNotNull ( Util.CheckRequired ( task, "alienbrainUri", new AlienbrainUri ( "alienbrain://google.com" ) ) );
      } catch ( Exception exception ) {
        Assert.Fail ( exception.Message );
      }

      try {
        Util.CheckRequired ( task, "alienbrainUri", new AlienbrainUri ( "http://google.com" ) );
        Assert.Fail ( "http scheme not allowed. Should have failed." );
      } catch ( Exception exception ) {
        Assert.IsAssignableFrom ( typeof ( UriFormatException ), exception );
      }

      // check valid values
      try {
        Assert.IsNotNull ( Util.CheckRequired ( task, "object", new object ( ) ) );
        Assert.IsNotEmpty ( Util.CheckRequired ( task, "string", "some string" ) );
        Assert.Greater ( Util.CheckRequired ( task, "DateTime", DateTime.Now ), DateTime.MinValue );
        HiddenPassword hp = new HiddenPassword ( );
        hp.Password = "word";
        Assert.IsNotNull ( Util.CheckRequired ( task, "HiddenPassword",  hp) );
      } catch ( Exception exception ) {
        Assert.Fail ( exception.Message );
      }

      //check invalid values
      try {
        Assert.IsNull ( Util.CheckRequired ( task, "NotificationType", ( Core.Enums.NotificationType? ) null ) );
        Assert.LessOrEqual ( Util.CheckRequired ( task, "CloneableList", new CloneableList<string> ( ) ).Count, 0 );

      } catch { }

    }
    [MinimumVersion ( "1.1" ), MaximumVersion ( "1.3" ), ExactVersion ( "1.2" )]
    public string GenericProperty { get { return "nothing"; } set { return; } }

    [Test]
    public void VersionAttributeTest ( ) {
      PropertyDescriptor pd = Util.GetPropertyDescriptor ( this.GetType ( ), "GenericProperty" );
      Assert.IsNotNull ( pd, "unable to get property descriptor" );
      Version vMax = Util.GetMaximumVersion ( pd );
      Assert.IsTrue ( vMax.CompareTo ( new Version ( "1.3" ) ) == 0 );
      Version vMin = Util.GetMinimumVersion ( pd );
      Assert.IsTrue ( vMin.CompareTo ( new Version ( "1.1" ) ) == 0 );
      Version vExact = Util.GetExactVersion ( pd );
      Assert.IsTrue ( vExact.CompareTo ( new Version ( "1.2" ) ) == 0 );

      Assert.IsTrue ( Util.IsInVersionRange ( vMin, vMax, new Version ( "1.2" ) ) );
      Assert.IsTrue ( Util.IsExactVersion ( vExact, new Version ( "1.2" ) ) );

    }

    [Test]
    public void BoolToIntTest ( ) {
      Assert.That ( Util.BooleanToInteger ( true ) == 1 );
      Assert.That ( Util.BooleanToInteger ( false ) == 0 );
    }
    [Test]
    public void EncodeDecodeTest ( ) {
      string _test = "CCNetConfig";
      string _encoded = Util.Encode ( _test );
      Assert.IsNotEmpty(_encoded);
      Assert.That ( String.Compare ( Util.Decode ( _encoded ), _test ) == 0,"Decode Failed, Strings do not match" );
    }

    [Test]
    public void ConfigTest ( ) {
      XmlDocument doc = new XmlDocument ( );
      doc.AppendChild ( doc.CreateElement ( "cruisecontrol" ) );

      XmlElement ele = doc.CreateElement ( "project" );
      ele.SetAttribute ( "name", "test" );
      doc.DocumentElement.AppendChild ( ele );
      XmlElement tele = doc.CreateElement ( "modificationDelaySeconds" );
      tele.InnerText = "20";
      ele.AppendChild ( tele );

      int i = int.Parse ( Util.GetElementOrAttributeValue ( "modificationDelaySeconds", ele ) );
      Assert.That ( i == 20, string.Format("i = {0} not 20",i) );

      string s = Util.GetElementOrAttributeValue("name",ele);
      Assert.IsNotEmpty ( s, "Unable to Get Element or Attribute" );
    }
  }
}
