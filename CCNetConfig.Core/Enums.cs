/*
 * Copyright (c) 2006 - 2009, Ryan Conrad. All rights reserved.
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
using System.ComponentModel;
using CCNetConfig.Core.Components;

namespace CCNetConfig.Core.Enums {

  /// <summary>
  /// Denotes the type of update it is
  /// </summary>
  public enum UpdateMode {
    /// <summary>
    /// A Beta release
    /// </summary>
    Beta = 0,
    /// <summary>
    /// A stable / production release
    /// </summary>
    Stable = 1,
    /// <summary>
    /// Reserved
    /// </summary>
    UNKNOWN = -1
  }

  /// <summary>
  /// Denotes the type of update feed to check. Default is Release
  /// </summary>
  public enum UpdateCheckType {
    /// <summary>
    /// Get only the beta builds
    /// </summary>
    BetaBuilds = 0,
    /// <summary>
    /// Get only the release builds
    /// </summary>
    ReleaseBuilds = 1,
    /// <summary>
    /// Get all build types
    /// </summary>
    AllBuilds = 2,
  }
  
  /// <summary>
  /// The conditions in which a build should be attempted.
  /// </summary>
  public enum BuildCondition : int {
    /// <summary>
    /// Do not build
    /// </summary>
    NoBuild = 0,
    /// <summary>
    /// Only build if the files have been modified since the last build
    /// </summary>
    IfModificationExists,
    /// <summary>
    /// Build always
    /// </summary>
    ForceBuild
  }

  /// <summary>
  /// The logical operator to apply to the results of the nested triggers. (Added in CCNet 1.1)
  /// </summary>
  public enum AndOr : int {
    /// <summary>
    /// Operator to indicate to use multiple <see cref="CCNetConfig.Core.Trigger">trigger</see>s
    /// </summary>
    And = 0,
    /// <summary>
    /// Operator to indicate to use one <see cref="CCNetConfig.Core.Trigger">trigger</see> or another
    /// </summary>
    Or
  }


  /// <summary>
  /// <para>Determines when to send email to this group.</para>
  /// </summary>
  public enum NotificationType : int {
    /// <summary>
    /// Send email when any build occurs
    /// </summary>
    Always = 0,
    /// <summary>
    /// Send email when the status of the build changes (e.g. from 'passed' to 'failed').
    /// </summary>
    Change,
    /// <summary>
    /// Send email when the build fails.
    /// </summary>
    Failed,
    /// <summary>
    /// send email when the build succeeds 
    /// </summary>
    [MinimumVersion("1.3")]
    Success,
    /// <summary>
    /// send mail when the status of the build changes from "Failed" to "Success". (available from version 1.3.0.2966) 
    /// </summary>
    [MinimumVersion ( "1.4" )]
    Fixed
  }

  /// <summary>
  /// The condition determining whether or not the remoting call should be made.
  /// </summary>
  public enum IntegrationStatus : int {
    /// <summary>
    /// The specified build will be forced if the current build was successful.
    /// </summary>
    Success = 0,
    /// <summary>
    /// The specified build will be forced if the current build had an exception.
    /// </summary>
    Exception,
    /// <summary>
    /// The specified build will be forced if the current build failed.
    /// </summary>
    Failure
  }

  /// <summary>
  /// The type of build you want to execute using the devenv executable
  /// </summary>
  public enum VSBuildType : int {
    /// <summary>
    /// Build the solution
    /// </summary>
    Build = 0,
    /// <summary>
    /// Rebuild the solution
    /// </summary>
    Rebuild,
    /// <summary>
    /// Perform a clean build
    /// </summary>
    Clean
  }

  /// <summary>
  /// The measurement of time to wait.
  /// </summary>
  public enum TimeoutUnit : int{
    /// <summary>
    /// Milliseconds
    /// </summary>
    Millis = 0,
    /// <summary>
    /// Seconds
    /// </summary>
    Seconds,
    /// <summary>
    /// Minutes
    /// </summary>
    Minutes,
    /// <summary>
    /// Hours
    /// </summary>
    Hours
  }

  /// <summary>
  /// The modification date that retrieved source files will have.
  /// </summary>
  public enum SourceControlSetFileTime : int {
    /// <summary>
    /// The date/time the file was checked in
    /// </summary>
    CheckIn = 0,
    /// <summary>
    /// The date/time the file was retrieved from Vault
    /// </summary>
    Current,
    /// <summary>
    /// The date/time the file was last modified
    /// </summary>
    Modification
  }

  /// <summary>
  /// The build conditions that can trigger the publisher
  /// </summary>
  public enum PublishBuildCondition : int {
    /// <summary>
    /// Only when there are modifications
    /// </summary>
    IfModificationExists = 0,
    /// <summary>
    /// Only when the build is triggered by a force build
    /// </summary>
    ForceBuild,
    /// <summary>
    /// Any type of build condition.
    /// </summary>
    AllBuildConditions
  }
}
