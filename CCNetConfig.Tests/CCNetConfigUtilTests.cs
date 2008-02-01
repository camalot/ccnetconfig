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


/* When the CCNetConfig Project is added as a refrence i get the following error 
  Error	1	The "ResolveNativeReference" task failed unexpectedly.
System.ArgumentNullException: Parameter "metadataValue" cannot be null.
   at Microsoft.Build.Shared.ErrorUtilities.VerifyThrowArgumentNull(Object parameter, String parameterName)
   at Microsoft.Build.Utilities.TaskItem.SetMetadata(String metadataName, String metadataValue)
   at Microsoft.Build.Tasks.ResolveNativeReference.ExtractFromManifest(ITaskItem taskItem, String path, Hashtable containingReferenceFilesTable, Hashtable containedPrerequisiteAssembliesTable, Hashtable containedComComponentsTable, Hashtable containedTypeLibrariesTable, Hashtable containedLooseTlbFilesTable, Hashtable containedLooseEtcFilesTable)
   at Microsoft.Build.Tasks.ResolveNativeReference.Execute()
   at Microsoft.Build.BuildEngine.TaskEngine.ExecuteTask(ExecutionMode howToExecuteTask, Hashtable projectItemsAvailableToTask, BuildPropertyGroup projectPropertiesAvailableToTask, Boolean& taskClassWasFound)	C:\Windows\Microsoft.NET\Framework\v2.0.50727\Microsoft.Common.targets	1202	9	CCNetConfig.Tests
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Windows.Forms;
//using CCNetConfig.Components;

namespace CCNetConfig.Tests {
  [TestFixture]
  public class CCNetConfigUtilTests {
    /*[Test]
    public void FindNodeByTypeTest ( ) {
      TreeView tv = new TreeView ( );
      TreeNode root = new TreeNode("root");

      TriggersTreeNode ttn = new TriggersTreeNode ( -1 );
      root.Nodes.Add ( ttn );
      TasksTreeNode ttn1 = new TasksTreeNode ( -1 );
      root.Nodes.Add ( ttn1 );
      PublishersTreeNode ttn2 = new PublishersTreeNode ( -1 );
      root.Nodes.Add ( ttn2 );
      PrebuildTreeNode ttn3 = new PrebuildTreeNode ( -1 );
      root.Nodes.Add ( ttn3 );
      tv.Nodes.Add(root);
      Assert.IsInstanceOfType ( typeof ( TriggersTreeNode ), Util.FindNodeByType ( root, typeof ( TriggersTreeNode ) ) );
      Assert.IsInstanceOfType ( typeof ( TasksTreeNode ), Util.FindNodeByType ( root, typeof ( TasksTreeNode ) ) );
      Assert.IsInstanceOfType ( typeof ( PublishersTreeNode ), Util.FindNodeByType ( root, typeof ( PublishersTreeNode ) ) );
      Assert.IsInstanceOfType ( typeof ( PrebuildTreeNode ), Util.FindNodeByType ( root, typeof ( PrebuildTreeNode ) ) );
    }*/
  }
}
