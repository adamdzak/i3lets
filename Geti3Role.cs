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
    [Cmdlet(VerbsCommon.Get, "i3Role")]
    public class Geti3Role : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Mandatory = false, HelpMessage = "Role name to filter by.")]
        public string Role;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
            RoleConfigurationList roleConfigurationList = new RoleConfigurationList(manager);
            var querySettings = roleConfigurationList.CreateQuerySettings();
            querySettings.SetFilterDefinition(RoleConfiguration.Property.Id, Role, FilterMatchType.Exact);

            if (Role != null)
            {
                querySettings.SetPropertiesToRetrieveToAll();
                roleConfigurationList.StartCaching(querySettings);

                foreach (var user in roleConfigurationList.GetConfigurationList())
                {
                    WriteObject(user, true);
                }
            }

            roleConfigurationList.StopCaching();
        }
    }
}
