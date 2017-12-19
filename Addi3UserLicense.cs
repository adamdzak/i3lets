﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using ININ.IceLib.Connection;
using ININ.IceLib.Configuration;
using ININ.IceLib.Configuration.DataTypes;
using ININ.IceLib.Configuration.Mailbox;
using ININ.IceLib.Configuration.Mailbox.Utility;

namespace i3lets
{
    [Cmdlet(VerbsCommon.Add, "i3UserLicense")]
    public class Addi3UserLicense : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The UserID to modify.")]
        [ValidateNotNullOrEmpty]
        public string UserId;

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "A list of the licenses that will be assigned to the user")]
        [ValidateSet("Client", "InteractionScripter", "AccessAnalyzer", "AccessTracker", "iPadSupervisor", "HistoricalReportSupervisor", "DialerSupervisor", "ReportAssistantSupervisor", "RecorderAccess", "RecorderClient", "SystemStatusSupervisor", "WorkgroupSupervisor", "SalesforceStandardUser", "SalesforceBusinessUser")]
        [ValidateNotNullOrEmpty]
        public string[] Licenses;

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The optional ACD license representing which ACD level the user should be assigned. Possible values are Media1, Media2, or Media3.")]
        [ValidateSet("Media1", "Media2", "Media3")]
        [ValidateNotNullOrEmpty]
        public string ACDLevel;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (UserId != null)
            {
                ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
                UserConfigurationList userConfigurationList = new UserConfigurationList(manager);
                var querySettings = userConfigurationList.CreateQuerySettings();

                querySettings.SetFilterDefinition(UserConfiguration.Property.Id, UserId, FilterMatchType.Exact);
                querySettings.SetRightsFilterToAdmin();
                querySettings.SetPropertiesToRetrieve(
                    new[] {
                        UserConfiguration.Property.License_HasClientAccess,
                        UserConfiguration.Property.License_AdditionalLicenses,
                        UserConfiguration.Property.License_MediaLevel
                    });

                userConfigurationList.StartCaching(querySettings);

                if (userConfigurationList.GetConfigurationList().Count > 0)
                {
                    var user = userConfigurationList.GetConfigurationList()[0];
                    user.PrepareForEdit();

                    #region define User Licensing
                    if (Licenses != null)
                    {
                        foreach (string license in Licenses)
                        {
                            try
                            {
                                if (license.ToLower() == "client")
                                {
                                    if (user.License.HasClientAccess.Value == true)
                                    {
                                        WriteWarning("User is already assigned client access license.");
                                    }
                                    else
                                    {
                                        user.License.HasClientAccess.Value = true;
                                    }
                                }
                                if (license.ToLower() == "accessanalyzer")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_ANALYZER")))
                                    {
                                        WriteWarning("User already has an Access Analyzer license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_ANALYZER"));
                                    }
                                }
                                if (license.ToLower() == "accesstracker")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_TRACKER")))
                                    {
                                        WriteWarning("User already has an Access Tracker license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_TRACKER"));
                                    }
                                }
                                if (license.ToLower() == "interactionscripter")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_INTERACTION_SCRIPTER_ADDON")))
                                    {
                                        WriteWarning("User already has an Interaction Scripter license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_INTERACTION_SCRIPTER_ADDON"));
                                    }  
                                }
                                if (license.ToLower() == "ipadsupervisor")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_IPAD_USER_SUPERVISOR")))
                                    {
                                        WriteWarning("User already has an iPad Supervisor license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_IPAD_USER_SUPERVISOR"));
                                    }  
                                }
                                if (license.ToLower() == "historicalreportsupervisor")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_HISTORICAL_REPORT_SUPERVISOR_PLUGIN")))
                                    {
                                        WriteWarning("User already has a Historical Report Supervisor license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_HISTORICAL_REPORT_SUPERVISOR_PLUGIN"));
                                    } 
                                }
                                if (license.ToLower() == "dialersupervisor")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_DIALER_SUPERVISOR_PLUGIN")))
                                    {
                                        WriteWarning("User already has a Dialer Supervisor license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_DIALER_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "recorderaccess")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_RECORDER")))
                                    {
                                        WriteWarning("User already has a Recorder Access license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_RECORDER"));
                                    }
                                }
                                if (license.ToLower() == "recorderclient")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_RECORDER_CLIENT")))
                                    {
                                        WriteWarning("User already has a Recorde Client license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_RECORDER_CLIENT"));
                                    }
                                }
                                if (license.ToLower() == "reportassistantsupervisor")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_REPORT_ASSISTANT_SUPERVISOR_PLUGIN")))
                                    {
                                        WriteWarning("User already has a Report Assistant Supervisor license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_REPORT_ASSISTANT_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "systemstatussupervisor")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_SYSTEM_STATUS_SUPERVISOR_PLUGIN")))
                                    {
                                        WriteWarning("User already has a Access System Status Supervisor license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_SYSTEM_STATUS_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "workgroupsupervisor")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_WORKGROUP_SUPERVISOR_PLUGIN")))
                                    {
                                        WriteWarning("User already has a Workgroup Supervisor license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_ACCESS_WORKGROUP_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "salesforcebusinessuser")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_LICENSE_SALESFORCE_BUSINESSUSER")))
                                    {
                                        WriteWarning("User already has a Salesforce Business User license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_LICENSE_SALESFORCE_BUSINESSUSER"));
                                    }  
                                }
                                if (license.ToLower() == "salesforcestandarduser")
                                {
                                    if (user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_LICENSE_SALESFORCE")))
                                    {
                                        WriteWarning("User already has a Salesforce Standard User license");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Add(new ConfigurationId("I3_LICENSE_SALESFORCE"));
                                    }
                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                WriteError(new ErrorRecord(
                                ex,
                                "License of" + license + "Could not be Assigned",
                                ErrorCategory.InvalidOperation,
                                license));
                                continue;
                            }
                        }

                        //apply licenses
                        user.License.LicenseActive.Value = true;
                    }

                    if (ACDLevel != null)
                    {
                        if (ACDLevel == "Media1")
                        {
                            if (user.License.MediaLevel.Value == MediaLevel.Media1)
                            {
                                WriteWarning("User is already assigned ACD Level Media 1");
                            }
                            else
                            {
                                user.License.MediaLevel.Value = MediaLevel.Media1;
                                user.License.LicenseActive.Value = true;
                            }
                        }
                        if (ACDLevel == "Media2")
                        {
                            if (user.License.MediaLevel.Value == MediaLevel.Media2)
                            {
                                WriteWarning("User is already assigned ACD Level Media 2");
                            }
                            else
                            {
                                user.License.MediaLevel.Value = MediaLevel.Media2;
                                user.License.LicenseActive.Value = true;
                            }
                        }
                        if (ACDLevel == "Media3")
                        {
                            if (user.License.MediaLevel.Value == MediaLevel.Media3)
                            {
                                WriteWarning("User is already assigned ACD Level Media 3");
                            }
                            else
                            {
                                user.License.MediaLevel.Value = MediaLevel.Media3;
                                user.License.LicenseActive.Value = true;
                            }
                        }

                    }
                    #endregion

                    user.Commit();
                }

                userConfigurationList.StopCaching();
            }
        }
    }
}