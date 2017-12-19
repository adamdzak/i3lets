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
    [Cmdlet(VerbsCommon.New, "i3User")]
    public class Newi3User : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The new user's given name. This is the user's first name as defined by Active Directory.")]
        [ValidateNotNullOrEmpty]
        public string GivenName;

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The new user's surname. This is the user's last name as defined by Active Directory.")]
        [ValidateNotNullOrEmpty]
        public string Surname;

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The new UserID to set.")]
        [ValidateNotNullOrEmpty]
        public string UserId;

        [Parameter(Position = 4, Mandatory = false, HelpMessage = "The new NT Domain Name to set.")]
        [ValidateNotNullOrEmpty]
        public string NTDomainName;

        [Parameter(Position = 5, Mandatory = false, HelpMessage = "The new user's Department name. This is the user's department name as defined by Active Directory.")]
        [ValidateNotNullOrEmpty]
        public string Department;

        [Parameter(Position = 6, Mandatory = false, HelpMessage = "The new user's Job Title. This is the user's job title name as defined by Active Directory.")]
        [ValidateNotNullOrEmpty]
        public string JobTitle;

        [Parameter(Position = 7, Mandatory = false, HelpMessage = "The new user's extension. This must be a valid 4 digit extension.")]
        [ValidateNotNullOrEmpty]
        public string Extension;

        [Parameter(Position = 8, Mandatory = false, HelpMessage = "The new user's Outband ANI. This must be a valid ten digit phone number.")]
        [ValidateNotNullOrEmpty]
        public string OutbandANI;

        [Parameter(Position = 9, Mandatory = false, HelpMessage = "Enable fax capability for the user")]
        [ValidateSet("Enable")]
        public string FaxCapability;

        [Parameter(Position = 10, Mandatory = false, HelpMessage = "The new user's default workstation. The workstation must exist in the system already. This will not create a new workstation.")]
        public string DefaultWorkstation;

        [Parameter(Position = 11, Mandatory = false, HelpMessage = "The password policies that will be applied to the user.")]
        [ValidateNotNullOrEmpty]
        public string [] PasswordPolicy;


        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
            UserConfigurationList userConfigurationList = new UserConfigurationList(manager);
            UserConfiguration user = userConfigurationList.CreateObject();
            user.PrepareForEdit();

            #region define User Properties
            //define i3 UserId
            user.SetConfigurationId(UserId);

            //define first and last name for user profile
            user.PersonalInformation.GivenName.Value = GivenName;
            user.PersonalInformation.Surname.Value = Surname;

            //define NT Username
            if (!String.IsNullOrEmpty(NTDomainName))
            {
                user.NtDomainUser.Value = (NTDomainName);
            }

            //define extension
            if (!String.IsNullOrEmpty(Extension))
            {
                user.Extension.Value = Extension;
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
            }

            //define password policy
            if (PasswordPolicy != null)
            {
                foreach (string pwPolicy in PasswordPolicy)
                {
                    user.PasswordPolicies.Value.Add(new ConfigurationId(pwPolicy));
                }
            }
            #endregion

            user.Commit();
        }
    }
}