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
using CCNetConfig.Exceptions;
using System.IO;
using System.Collections;
using CCNetConfig.Core.Enums;
using CCNetConfig.Core;
using System.Reflection;
//using Microsoft.NetEnterpriseServers;
using System.Windows.Forms;
using CCNetConfig.Components;
using System.Xml;
using CCNetConfig.Core.Components;
using System.ComponentModel;

namespace CCNetConfig {
  /// <summary>
  /// A Helper Static Class
  /// </summary>
  public static class Util {

    /// <summary>
    /// Finds the type of the node by.
    /// </summary>
    /// <param name="parenttn">The parenttn.</param>
    /// <param name="t">The t.</param>
    /// <returns></returns>
    public static TreeNode FindNodeByType ( TreeNode parenttn, Type t ) {
      foreach ( TreeNode tn in parenttn.Nodes ) {
        if ( tn.GetType () == t )
          return tn;
      }
      return null;
    }

    /*public static void MessageBoxEx( Exception ex, string title, ExceptionMessageBoxButtons buttons, ExceptionMessageBoxSymbol symbol ) {
      Microsoft.NetEnterpriseServers.ExceptionMessageBox emb = new Microsoft.NetEnterpriseServers.ExceptionMessageBox (ex, buttons, symbol, ExceptionMessageBoxDefaultButton.Button1, ExceptionMessageBoxOptions.None);
      emb.Caption = title;
      emb.Show (null);
    }*/

    /// <summary>
    /// Creates an instance of a <see cref="System.Type"/>.
    /// </summary>
    /// <param name="t">The type.</param>
    /// <returns></returns>
    public static object CreateInstanceOfType ( Type t ) {
      Assembly asm = t.Assembly;
      return asm.CreateInstance ( t.FullName, true );
    }

    /// <summary>
    /// Confirms the delete.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="itemName">Name of the item.</param>
    /// <returns></returns>
    public static DialogResult ConfirmDelete ( Form owner, string itemName ) {
      return MessageBox.Show ( owner, string.Format ( Properties.Resources.ConfirmDelete, itemName ), Properties.Resources.ConfirmDeleteTitle,
        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
    }

    /// <summary>
    /// Confirms the delete.
    /// </summary>
    /// <param name="itemName">Name of the item.</param>
    /// <returns></returns>
    public static DialogResult ConfirmDelete ( string itemName ) {
      return ConfirmDelete ( null, itemName );
    }

    /// <summary>
    /// Confirms the delete.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static DialogResult ConfirmDelete ( Type type ) {
      return ConfirmDelete ( null, type );
    }

    /// <summary>
    /// Confirms the delete.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static DialogResult ConfirmDelete ( Form owner, Type type ) {
      return ConfirmDelete ( owner, type.Name );
    }


    /// <summary>
    /// Gets the a unique name.
    /// </summary>
    /// <param name="cruiseControl">The cruise control.</param>
    /// <param name="pjct">The PJCT.</param>
    /// <returns></returns>
    internal static string GetUniqueName ( CruiseControl cruiseControl, Project pjct ) {
      for ( int x = 0; x < int.MaxValue; x++ ) {
        string newKey = string.Format ( Properties.Strings.CopyOfProject, pjct.Name, x == 0 ? "" : string.Format ( " {0}", x ) );
        if ( !cruiseControl.Projects.Contains ( newKey ) )
          return newKey;
      }
      throw new ArgumentOutOfRangeException ( "No more projects can be created from this copy." );
    }


    /// <summary>
    /// Gets the CC net versions.
    /// </summary>
    /// <returns></returns>
    internal static CloneableList<Version> GetCCNetVersions ( ) {
      CloneableList<Version> versions = new CloneableList<Version> ( );
      XmlDocument doc = new XmlDocument ();
      FileInfo file = new FileInfo ( System.IO.Path.Combine ( Application.StartupPath, Program.Configuration["CCNetVersions"].Path ) );
      if ( file.Exists ) {
        doc.Load ( file.FullName );
        foreach ( XmlElement ele in doc.DocumentElement.SelectNodes ( "Version" ) )
          versions.Add ( new Version ( ele.InnerText ) );
      }
      return versions;
    }
  }


}
