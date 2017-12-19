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
    [Cmdlet(VerbsCommon.Get, "i3User")]
    public class Geti3User : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Mandatory = false, HelpMessage = "User id to filter by.")]
        public string UserId;

        [Parameter(Mandatory = false, HelpMessage = "Optional display name to filter by.")]
        public string DisplayName;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
            UserConfigurationList userConfigurationList = new UserConfigurationList(manager);
            var querySettings = userConfigurationList.CreateQuerySettings();

            if (UserId != null)
            {
                querySettings.SetFilterDefinition(UserConfiguration.Property.Id, UserId, FilterMatchType.Exact);
                querySettings.SetPropertiesToRetrieveToAll();
                userConfigurationList.StartCaching(querySettings);

                foreach (var user in userConfigurationList.GetConfigurationList())
                {
                    WriteObject(user, true);
                }
            }
            if (DisplayName != null)
            {
                querySettings.SetFilterDefinition(UserConfiguration.Property.DisplayName, DisplayName, FilterMatchType.Contains);
                querySettings.SetPropertiesToRetrieveToAll();
                querySettings.SetRightsFilterToAdmin();
                userConfigurationList.StartCaching(querySettings);

                foreach (var user in userConfigurationList.GetConfigurationList())
                {
                    WriteObject(user, true);
                }
            }

            userConfigurationList.StopCaching();

        }
    }
}
