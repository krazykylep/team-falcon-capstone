﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PeregrineCreateDB.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PeregrineCreateDB.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [PeregrineDB]
        ///GO
        ////****** Object:  StoredProcedure [dbo].[GetStartStopMessagesWithProcessID]    Script Date: 02/13/2012 19:35:47 ******/
        ///SET ANSI_NULLS ON
        ///GO
        ///SET QUOTED_IDENTIFIER ON
        ///GO
        ///-- =============================================
        ///-- Author:		Kyle P. / Weixiong Lu
        ///-- Create date: 2012-02-04
        ///-- Description:	Retrieve process by ProcessID
        ///-- =============================================
        ///CREATE PROCEDURE [dbo].[GetStartStopMessagesWithProcessID] 
        ///	-- Add the parameters for the stored procedu [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateDatabaseSql {
            get {
                return ResourceManager.GetString("CreateDatabaseSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [msdb]
        ///GO
        ///
        ////****** Object:  Job [PeregrineDBCleanup]    Script Date: 02/18/2012 16:16:57 ******/
        ///IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N&apos;PeregrineDBCleanup&apos;)
        ///EXEC msdb.dbo.sp_delete_job @job_id=N&apos;63c25ee4-b1ed-4799-96f0-fc762f4e2ce3&apos;, @delete_unused_schedule=1
        ///GO
        ///
        ///USE [msdb]
        ///GO
        ///
        ////****** Object:  Job [PeregrineDBCleanup]    Script Date: 02/18/2012 16:16:57 ******/
        ///BEGIN TRANSACTION
        ///DECLARE @ReturnCode INT
        ///SELECT @ReturnCode = 0
        ////****** Object:  JobCategory [Data [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DropCreateCleanupJob {
            get {
                return ResourceManager.GetString("DropCreateCleanupJob", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USE [master]
        ///GO
        ///
        ////****** Object:  Database [PeregrineDB]    Script Date: 02/13/2012 19:30:04 ******/
        ///IF  EXISTS (SELECT name FROM sys.databases WHERE name = N&apos;PeregrineDB&apos;)
        ///DROP DATABASE [PeregrineDB]
        ///GO
        ///
        ///USE [master]
        ///GO
        ///
        ////****** Object:  Database [PeregrineDB]    Script Date: 02/13/2012 19:30:04 ******/
        ///CREATE DATABASE [PeregrineDB] ON  PRIMARY 
        ///( NAME = N&apos;PeregrineDB&apos;, FILENAME = N&apos;C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\PeregrineDB.mdf&apos; , SIZE = 2048KB , MAXSIZE  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DropCreateEmptyDatabaseSql {
            get {
                return ResourceManager.GetString("DropCreateEmptyDatabaseSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PeregrineDB.
        /// </summary>
        internal static string OldDatabaseName {
            get {
                return ResourceManager.GetString("OldDatabaseName", resourceCulture);
            }
        }
    }
}