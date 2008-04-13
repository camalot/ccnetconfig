/*
 * Copyright (c) 2006, Ryan Conrad. All rights reserved.
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
using CCNetConfig.Core.Serialization;
using CCNetConfig.CCNet;
using System.Xml;

namespace CCNetConfig.Tests {
  [TestFixture]
  public class SerializerTests {
    [Test]
    public void EmailPublisherSerializerTest ( ) {
      Serializer<EmailPublisher> ser = new Serializer<EmailPublisher> ( );
      EmailPublisher ep = new EmailPublisher ( );
      ep.From = "no-email@to-spam.com";
      ep.MailHost = "fake.mail.host.com";
      HiddenPassword hp = new HiddenPassword ( );
      hp.Password = "myPassW0rd";
      ep.MailHostPassword = hp;
      ep.MailHostUserName = "user";
      User u = new User ( );
      u.Address = "foo@bar.com";
      u.Name = "foo bar";
      ep.Users.Add ( u );

      XmlElement ele = ser.Serialize ( ep );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );

    }
    [Test]
    public void RssPublisherSerializerTest ( ) {
      Serializer<RssPublisher> ser = new Serializer<RssPublisher> ( );
      RssPublisher rp = new RssPublisher ( );
      rp.FileName = "foo";
      XmlElement ele = ser.Serialize ( rp );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void DateLabellerSerializerTest ( ) {
      Serializer<DateLabeller> ser = new Serializer<DateLabeller> ( );
      DateLabeller dl = new DateLabeller ( );
      XmlElement ele = ser.Serialize ( dl );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void BrekiLabellerSerializerTest ( ) {
      Serializer<BrekiLabeller> ser = new Serializer<BrekiLabeller> ( );
      BrekiLabeller bl = new BrekiLabeller ( );
      bl.MajorNumber = 1;
      bl.MinorNumber = 0;
      bl.ReleaseStartDate = DateTime.Now;
      XmlElement ele = ser.Serialize ( bl );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void DefaultLabellerSerializerTest ( ) {
      Serializer<DefaultLabeller> ser = new Serializer<DefaultLabeller> ( );
      DefaultLabeller dl = new DefaultLabeller ( );
      dl.Prefix = "myApplication-";
      XmlElement ele = ser.Serialize ( dl );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void FileLabellerSerializerTest ( ) {
      Serializer<FileLabeller> ser = new Serializer<FileLabeller> ( );
      FileLabeller fl = new FileLabeller ( );
      fl.Prefix = "myApplication-";
      fl.LabelFilePath = @"c:\ci\woot.txt";
      XmlElement ele = ser.Serialize ( fl );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void IterationLabellerSerializerTest ( ) {
      Serializer<IterationLabeller> ser = new Serializer<IterationLabeller> ( );
      IterationLabeller il = new IterationLabeller ( );
      il.Duration = 1;
      il.Prefix = "1.0";
      il.Separator = ".";
      il.ReleaseStartDate = DateTime.Now.AddDays ( -25 );
      XmlElement ele = ser.Serialize ( il );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void LastChangeLabellerSerializerTest ( ) {
      Serializer<LastChangeLabeller> ser = new Serializer<LastChangeLabeller> ( );
      LastChangeLabeller ll = new LastChangeLabeller ( );
      ll.Prefix = "1.0";
      XmlElement ele = ser.Serialize ( ll );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void LastChangeVersionLabellerSerializerTest ( ) {
      Serializer<LastChangeVersionLabeller> ser = new Serializer<LastChangeVersionLabeller> ( );
      LastChangeVersionLabeller ll = new LastChangeVersionLabeller ( );
      ll.Major = 2;
      ll.IncrementOnFailure = true;
      XmlElement ele = ser.Serialize ( ll );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void MSProjectLabellerSerializerTest ( ) {
      Serializer<MSProjectLabeller> ser = new Serializer<MSProjectLabeller> ( );
      MSProjectLabeller ml = new MSProjectLabeller ( );

      ml.Major = 2;
      ml.Minor = 1;
      ml.ReleaseDate = DateTime.Now.AddDays ( -25 );

      XmlElement ele = ser.Serialize ( ml );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }


    [Test]
    public void RemoteProjectLabellerSerializerTest ( ) {
      Serializer<RemoteProjectLabeller> ser = new Serializer<RemoteProjectLabeller> ( );
      RemoteProjectLabeller rl = new RemoteProjectLabeller ( );

      rl.Project = "foo";
      rl.ServerUri = new Uri ( "http://foobar" );

      XmlElement ele = ser.Serialize ( rl );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }


    [Test]
    public void StateFileLabellerSerializerTest ( ) {
      Serializer<StateFileLabeller> ser = new Serializer<StateFileLabeller> ( );
      StateFileLabeller sl = new StateFileLabeller ( );

      sl.Project = "foo";

      XmlElement ele = ser.Serialize ( sl );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }

    [Test]
    public void SubversionSourceControlSerializerTest ( ) {
      Serializer<SubversionSourceControl> ser = new Serializer<SubversionSourceControl> ( );
      SubversionSourceControl ssc = new SubversionSourceControl ( );

      ssc.Executable = "svn.exe";
      //ssc.TrunkUrl = new Uri ( "svn://foobar/trunk" );

      XmlElement ele = ser.Serialize ( ssc );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
    }
  }
}
