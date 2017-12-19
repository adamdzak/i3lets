using System;
using System.Management.Automation;
using ININ.IceLib.Configuration;
using System.Collections.Generic;
using ININ.IceLib.Configuration.DataTypes;
using ININ.IceLib.Configuration.Mailbox;
using ININ.IceLib.Configuration.Mailbox.Utility;
using ININ.IceLib.Connection;

namespace i3lets
{
    [Cmdlet(VerbsCommon.Set, "i3User")]
    public class Seti3User : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The UserID to modify.")]
        [ValidateNotNullOrEmpty]
        public string UserId;

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The user's given name.")]
        [ValidateNotNullOrEmpty]
        public string GivenName;

        [Parameter(Position = 3, Mandatory = false, HelpMessage = "The user's surname.")]
        [ValidateNotNullOrEmpty]
        public string Surname;

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The user's new NT Domain Name.")]
        [ValidateNotNullOrEmpty]
        public string NTDomainName;

        [Parameter(Position = 5, Mandatory = false, HelpMessage = "The user's Department name.")]
        [ValidateNotNullOrEmpty]
        public string Department;

        [Parameter(Position = 6, Mandatory = false, HelpMessage = "The user's Job Title.")]
        [ValidateNotNullOrEmpty]
        public string JobTitle;

        [Parameter(Position = 7, Mandatory = false, HelpMessage = "The user's email address.")]
        [ValidateNotNullOrEmpty]
        public string EmailAddress;

        [Parameter(Position = 8, Mandatory = false, HelpMessage = "The user's extension. This must be a valid 4 digit extension.")]
        public string Extension;

        [Parameter(Position = 9, Mandatory = false, HelpMessage = "The user's Outbound ANI. This must be a valid 10 digit phone number.")]
        public string OutbandANI;

        [Parameter(Position = 10, Mandatory = false, HelpMessage = "The user's default workstation. The workstation must exist in the system already. This will not create a new workstation.")]
        public string DefaultWorkstation;

        [Parameter(Position = 11, Mandatory = false, HelpMessage = "Enable or disable fax capability for the user")]
        [ValidateSet("Enable", "Disable")]
        public string FaxCapability;

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

                userConfigurationList.StartCaching(querySettings);

                if (userConfigurationList.GetConfigurationList().Count > 0)
                {
                    var user = userConfigurationList.GetConfigurationList()[0];
                    user.PrepareForEdit();

                    #region define User Properties
                    //define first and last name for user profile
                    if (!String.IsNullOrEmpty(GivenName))
                    {
                        user.PersonalInformation.GivenName.Value = GivenName;
                    }
                    if (!String.IsNullOrEmpty(Surname))
                    {
                        user.PersonalInformation.Surname.Value = Surname;
                    }

                    //define NT Username
                    if (!String.IsNullOrEmpty(UserId))
                    {
                        user.NtDomainUser.Value = (NTDomainName);
                    }

                    //define extension
                    if (!String.IsNullOrEmpty(Extension))
                    {
                        user.Extension.Value = Extension;
                        user.OutboundAni.Value = OutbandANI;
                    }

                    //define outband ani
                    if (!String.IsNullOrEmpty(OutbandANI))
                    {
                        user.OutboundAni.Value = OutbandANI;
                    }

                    //define default workstation
                    if (!String.IsNullOrEmpty(DefaultWorkstation))
                    {
                        user.DefaultWorkstation.Value = new ConfigurationId(DefaultWorkstation);
                    }

                    //define Department name
                    if (!String.IsNullOrEmpty(Department))
                    {
                        user.PersonalInformation.DepartmentName.Value = Department;
                    }

                    //define Job Title
                    if (!String.IsNullOrEmpty(JobTitle))
                    {
                        user.PersonalInformation.Title.Value = JobTitle;
                    }

                    //define fax capability
                    if (!String.IsNullOrEmpty(FaxCapability))
                    {
                        if (FaxCapability.ToLower() == "enable")
                        {
                            user.FaxCapability.Value = true;
                        }
                        //define fax capability
                        if (FaxCapability.ToLower() == "disable")
                        {
                            user.FaxCapability.Value = false;
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