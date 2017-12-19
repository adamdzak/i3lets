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
    [Cmdlet(VerbsCommon.Set, "i3UserMailboxConfiguration")]
    public class Seti3UserMailboxConfiguration : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The User ID to set a mailbox configuration on")]
        [ValidateNotNullOrEmpty]
        public string UserId;

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The email address to assign the specified User ID")]
        [ValidateNotNullOrEmpty]
        public string EmailAddress;

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "The mailbox type that will be assigned to the user")]
        [ValidateSet("Exchange")]
        [ValidateNotNullOrEmpty]
        public string MailboxType;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if ((UserId != null) && (EmailAddress != null) && (MailboxType != null))
            {
                base.ProcessRecord();

                ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
                UserConfigurationList userConfigurationList = new UserConfigurationList(manager);

                //find the User by ID
                var querySettings = userConfigurationList.CreateQuerySettings();
                querySettings.SetFilterDefinition(UserConfiguration.Property.Id, UserId, FilterMatchType.Exact);
                querySettings.SetRightsFilterToAdmin();
                userConfigurationList.StartCaching(querySettings);

                if (userConfigurationList.GetConfigurationList().Count > 0)
                {
                    var user = userConfigurationList.GetConfigurationList()[0];
                    user.PrepareForEdit();

                    //define Exchange mailbox
                    if ((!String.IsNullOrEmpty(EmailAddress)) && (MailboxType.ToLower() == "exchange"))
                    {
                        //extract Domain Name from email address
                        string[] stringAtSeperator = new string[] { "@" };
                        string[] stringDotSeperator = new string[] { "." };
                        string[] domainNameAndTLDArray = EmailAddress.Split(stringAtSeperator, StringSplitOptions.None);
                        string[] domainNameArray = domainNameAndTLDArray[1].Split(stringDotSeperator, StringSplitOptions.None);

                        //define mailbox configuration
                        string mailboxDisplayName = user.PersonalInformation.GivenName.Value + " " + user.PersonalInformation.Surname.Value;
                        string mailboxDirectoryEntry = "x-inin-mail.ex.ews.directory:/d=" + domainNameArray[0] + "." + domainNameArray[1] + "/e=" + EmailAddress;
                        string mailboxMessageDelivery = EmailAddress;
                        string mailboxMessageRetrieval = "x-inin-mail.ex.ews.store:/s=" + EmailAddress;

                        //apply mailbox configuration to the specified user id
                        GenericProviderMailboxSettings mailboxSetting = new GenericProviderMailboxSettings(mailboxDisplayName, mailboxDirectoryEntry, mailboxMessageDelivery, mailboxMessageRetrieval);
                        user.Mailbox.ApplyMailboxSettings(mailboxSetting);
                    }

                    user.Commit();
                }
            }
        }
    }
}