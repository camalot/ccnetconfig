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

namespace CCNetConfig.BugTracking {
  /// <summary>
  /// Status code of adding an item
  /// </summary>
  public enum AddItemStatusCode : int {
    /// <summary>
    /// Invalid value
    /// </summary>
    Invalid = -1,
    /// <summary>
    /// Success
    /// </summary>
    Success = 0,
    /// <summary>
    /// Failed
    /// </summary>
    Failure = 1,
    /// <summary>
    /// Not implemented
    /// </summary>
    NotImplemented = 2,
    /// <summary>
    /// Invalid Data
    /// </summary>
    InvalidData = 3,
  }

  /// <summary>
  /// Bug Submission General errors
  /// </summary>
  public enum GeneralErrors : int {
    /// <summary>
    /// Incorrect Password
    /// </summary>
    IncorrectPassword = 1001,
    /// <summary>
    /// Invalid user
    /// </summary>
    InvalidUser = 1002,
    /// <summary>
    /// User not authenticated
    /// </summary>
    NotAuthenticated = 1003,
    /// <summary>
    /// Session timedout
    /// </summary>
    SessionTimeout = 1004,
    /// <summary>
    /// User is inactive
    /// </summary>
    InactiveUser = 1005,
    /// <summary>
    /// Invalid Authentication Token
    /// </summary>
    InvalidAuthToken = 1006,
    /// <summary>
    /// Invalid Smtp Server
    /// </summary>
    InvalidSmtpServer = 1007,
    /// <summary>
    /// Invalid Credentials
    /// </summary>
    InvalidCredentials = 1008,
    /// <summary>
    /// Permission to the project is denied
    /// </summary>
    ProjectPermissionDenied = 1009
  }

  /// <summary>
  /// Bug submission database errors
  /// </summary>
  public enum DatabaseErrors : int {
    /// <summary>
    /// connection error
    /// </summary>
    Connection = 1101,
    /// <summary>
    /// Error instering
    /// </summary>
    Insert = 1102,
    /// <summary>
    /// Error deleting
    /// </summary>
    Delete = 1103,
    /// <summary>
    /// Transaction Error
    /// </summary>
    Transaction = 1104,
    /// <summary>
    /// Reader Error
    /// </summary>
    Reader = 1105,
    /// <summary>
    /// Error while updating
    /// </summary>
    Update = 1106,
    /// <summary>
    /// Error Deleting System Item
    /// </summary>
    DeleteSystemItem = 1107,
    /// <summary>
    /// Naming Conflict
    /// </summary>
    NamingConflict = 1108
  }

  /// <summary>
  /// Specific Errors
  /// </summary>
  public enum SpecificErrors : int {
    /// <summary>
    /// Project Not Found
    /// </summary>
    ProjectIdNotFound = 1407,
    /// <summary>
    /// Database File Stream Error
    /// </summary>
    DBFileStreamError = 1501,
    /// <summary>
    /// Invalid File Stream
    /// </summary>
    InvalidFileStream = 1502,
    /// <summary>
    /// Database Attachment Insert Error
    /// </summary>
    DBAttachmentInsert = 1503
  }
}
