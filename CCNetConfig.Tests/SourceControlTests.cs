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
using CCNetConfig.CCNet;
using System.Xml;

namespace CCNetConfig.Tests {
  [TestFixture]
  public class SourceControlTests {
    [Test]
    public void ExternalSourceControlTest ( ) {
      ExternalSourceControl esc = new ExternalSourceControl ( );
      EnvironmentVariable ev = new EnvironmentVariable ( );
      ev.Name = "Foo";
      ev.Value = "Blah";
      esc.Environment.Add ( ev );
      ev = new EnvironmentVariable ( );
      ev.Name = "Bar";
      ev.Value = "Water";
      esc.Environment.Add ( ev );
      esc.Arguments = "/f /b";
      esc.Executable = "cmd.exe";
      XmlElement node = esc.Serialize ( );
      Assert.IsNotNull ( node, "Serialize failed for ExternalSourceControl" );
      Assert.AreEqual ( node.SelectSingleNode ( "executable" ).InnerText, esc.Executable );
      Assert.AreEqual ( node.SelectSingleNode ( "args" ).InnerText, esc.Arguments );

      esc = new ExternalSourceControl ( );
      esc.Deserialize ( node );

      Assert.AreEqual ( node.SelectSingleNode ( "executable" ).InnerText, esc.Executable );
      Assert.AreEqual ( node.SelectSingleNode ( "args" ).InnerText, esc.Arguments );
    }

    [Test]
    public void AccuRevSourceControlTest ( ) {
      AccuRevSourceControl arsc = new AccuRevSourceControl ( );
      arsc.AutoGetSource = true;
      arsc.Executable = "arsc.exe";
      arsc.HomeDirectory = "\\";
      arsc.LabelOnSuccess = null;
      arsc.Login = true;
      arsc.Password = new CCNetConfig.Core.HiddenPassword ( );
      arsc.Password.Password = "test";
      arsc.Principal = "principal";
      arsc.Timeout = null;
      arsc.Workspace = "\\";
      XmlElement node = arsc.Serialize ( );
      Assert.IsNotNull ( node, "Serialize failed for AccuRevSourceControl" );
      Assert.AreEqual ( node.SelectSingleNode ( "executable" ).InnerText, arsc.Executable );
      Assert.AreEqual ( node.SelectSingleNode ( "homeDir" ).InnerText, arsc.HomeDirectory );
      Assert.AreEqual ( node.SelectSingleNode ( "password" ).InnerText, arsc.Password.GetPassword ( ) );
      Assert.AreEqual ( node.SelectSingleNode ( "principal" ).InnerText, arsc.Principal );

      arsc = new AccuRevSourceControl ( );
      arsc.Deserialize ( node );

      Assert.AreEqual ( node.SelectSingleNode ( "executable" ).InnerText, arsc.Executable );
      Assert.AreEqual ( node.SelectSingleNode ( "homeDir" ).InnerText, arsc.HomeDirectory );
      Assert.AreEqual ( node.SelectSingleNode ( "password" ).InnerText, arsc.Password.GetPassword ( ) );
      Assert.AreEqual ( node.SelectSingleNode ( "principal" ).InnerText, arsc.Principal );
    }
    [Test]
    public void AlienbrainSourceControlTest ( ) {

    }
    [Test]
    public void BazaarSourceControlTest ( ) {

    }
    [Test]
    public void BitKeeperSourceControlTest ( ) {

    }
    [Test]
    public void CodePlexSourceControlTest ( ) {

    }
    [Test]
    public void CvsSourceControlTest ( ) {

    }
    [Test]
    public void FileSystemSourceControlTest ( ) {

    }
    [Test]
    public void FilteredSourceControlTest ( ) {

    }
    [Test]
    public void JediVcsSourceControlTest ( ) {

    }
    [Test]
    public void MksSourceIntegritySourceControlTest ( ) {

    }
    [Test]
    public void MultiSourceControlTest ( ) {

    }
    [Test]
    public void NullSourceControlTest ( ) {

    }
    [Test]
    public void PerforceSourceControlTest ( ) {

    }
    [Test]
    public void PvcsSourceControlTest ( ) {

    }
    [Test]
    public void RationalClearCaseSourceControlTest ( ) {

    }
    [Test]
    public void SeapineSurroundSourceControlTest ( ) {

    }
    [Test]
    public void SourceGearVaultSourceControlTest ( ) {
    
    }
    [Test]
    public void StarTeamSourceControlTest ( ) {

    }
    [Test]
    public void SubversionSourceControlTest ( ) {

    }
    [Test]
    public void TelelogicSynergySourceControlTest ( ) {

    }
    [Test]
    public void VisualSourceSafeSourceControlTest ( ) {

    }
    [Test]
    public void VSTeamFoundationServerSourceControl ( ) {

    }
  }
}
