using System;
using System.Collections.Generic;
using System.Management.Automation;
using ININ.IceLib.Connection;
using ININ.IceLib.Configuration;
using ININ.IceLib.Configuration.DataTypes;
using ININ.IceLib.Configuration.Mailbox;
using ININ.IceLib.Configuration.Mailbox.Utility;

namespace i3lets
{
    [Cmdlet(VerbsCommon.Remove, "i3UserLicense")]
    public class Removei3UserLicense : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The UserID to remove licenses from.")]
        [ValidateNotNullOrEmpty]
        public string UserId;

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "A list of the licenses that will be removed from the user")]
        [ValidateSet("Client", "InteractionScripter", "AccessAnalyzer", "AccessTracker", "iPadSupervisor", "HistoricalReportSupervisor", "DialerSupervisor", "ReportAssistantSupervisor", "RecorderAccess", "RecorderClient", "SystemStatusSupervisor", "WorkgroupSupervisor", "SalesforceStandardUser", "SalesforceBusinessUser")]
        [ValidateNotNullOrEmpty]
        public string[] Licenses;

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "When used, the disable value is the only accepted value which removes ACD access from the specified user.")]
        [ValidateNotNullOrEmpty]
        [ValidateSet("disable")]
        public string ACDAccess;

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
                                    if (user.License.HasClientAccess.Value == false)
                                    {
                                        WriteWarning("User has no client access license to remove!");
                                    }
                                    else
                                    {
                                        user.License.HasClientAccess.Value = false;
                                    }
                                }
                                if (license.ToLower() == "accessanalyzer")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_ANALYZER"))))
                                    {
                                        WriteWarning("User has no Access Analyzer license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_ANALYZER"));
                                    }
                                }
                                if (license.ToLower() == "accesstracker")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_TRACKER"))))
                                    {
                                        WriteWarning("User has no Access Track license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_TRACKER"));
                                    }
                                }
                                if (license.ToLower() == "interactionscripter")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_INTERACTION_SCRIPTER_ADDON"))))
                                    {
                                        WriteWarning("User has no Interaction Scripter license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_INTERACTION_SCRIPTER_ADDON"));
                                    }
                                }
                                if (license.ToLower() == "ipadsupervisor")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_IPAD_USER_SUPERVISOR"))))
                                    {
                                        WriteWarning("User has no iPad supervisor license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_IPAD_USER_SUPERVISOR"));
                                    }
                                }
                                if (license.ToLower() == "historicalreportsupervisor")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_HISTORICAL_REPORT_SUPERVISOR_PLUGIN"))))
                                    {
                                        WriteWarning("User has no Historical Report Supervisor license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_HISTORICAL_REPORT_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "dialersupervisor")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_DIALER_SUPERVISOR_PLUGIN"))))
                                    {
                                        WriteWarning("User no Dialer Supervisor license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_DIALER_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "reportassistantsupervisor")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_REPORT_ASSISTANT_SUPERVISOR_PLUGIN"))))
                                    {
                                        WriteWarning("User has no Report Assistant Supervisor license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_REPORT_ASSISTANT_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "recorderaccess")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_RECORDER"))))
                                    {
                                        WriteWarning("User has no Recorder license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_RECORDER"));
                                    }
                                }
                                if (license.ToLower() == "recorderclient")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_RECORDER_CLIENT"))))
                                    {
                                        WriteWarning("User has no Recorder Client license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_RECORDER_CLIENT"));
                                    }
                                }
                                if (license.ToLower() == "systemstatussupervisor")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_SYSTEM_STATUS_SUPERVISOR_PLUGIN"))))
                                    {
                                        WriteWarning("User has no Access System Status Supervisor license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_SYSTEM_STATUS_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "workgroupsupervisor")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_ACCESS_WORKGROUP_SUPERVISOR_PLUGIN"))))
                                    {
                                        WriteWarning("User has no Workgroup Supervisor license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_ACCESS_WORKGROUP_SUPERVISOR_PLUGIN"));
                                    }
                                }
                                if (license.ToLower() == "salesforcebusinessuser")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_LICENSE_SALESFORCE_BUSINESSUSER"))))
                                    {
                                        WriteWarning("User has no Salesforce Business User license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_LICENSE_SALESFORCE_BUSINESSUSER"));
                                    }
                                }
                                if (license.ToLower() == "salesforcestandarduser")
                                {
                                    if (!(user.License.AdditionalLicenses.Value.Contains(new ConfigurationId("I3_LICENSE_SALESFORCE"))))
                                    {
                                        WriteWarning("User no Salesforce Standard User license to remove!");
                                    }
                                    else
                                    {
                                        user.License.AdditionalLicenses.Value.Remove(new ConfigurationId("I3_LICENSE_SALESFORCE"));
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

                    if (ACDAccess == "disable")
                    {
                        if (user.License.MediaLevel.Value == MediaLevel.None)
                        {
                            WriteWarning("User has no ACD license to remove!");
                        }
                        else
                        {
                            user.License.MediaLevel.Value = MediaLevel.None;
                            user.License.LicenseActive.Value = true;
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