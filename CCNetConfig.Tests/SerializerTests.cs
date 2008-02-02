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
      HiddenPassword hp = new HiddenPassword();
      hp.Password = "myPassW0rd";
      ep.MailHostPassword = hp;
      ep.MailHostUserName = "user";
      User u = new User();
      u.Address ="foo@bar.com";
      u.Name= "foo bar";
      ep.Users.Add ( u );

      XmlElement ele = ser.Serialize ( ep );
      Assert.IsNotNull ( ele );
      Console.WriteLine ( ele.OuterXml );
      
    }
  }
}
