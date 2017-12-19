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
    [Cmdlet(VerbsCommon.Remove, "i3RoleMember")]
    public class Removei3RoleMember : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The Role remove a user from")]
        [ValidateNotNullOrEmpty]
        public string Role;

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The UserID to remove from the role.")]
        [ValidateNotNullOrEmpty]
        public string UserId;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if ((Role != null) && (UserId != null))
            {
                ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
                RoleConfigurationList roleConfigurationList = new RoleConfigurationList(manager);
                var querySettings = roleConfigurationList.CreateQuerySettings();

                querySettings.SetFilterDefinition(RoleConfiguration.Property.Id, Role, FilterMatchType.Exact);
                querySettings.SetRightsFilterToAdmin();
                querySettings.SetPropertiesToRetrieveToAll();

                roleConfigurationList.StartCaching(querySettings);

                if (roleConfigurationList.GetConfigurationList().Count > 0)
                {
                    var roleToMod = roleConfigurationList.GetConfigurationList()[0];
                    roleToMod.PrepareForEdit();

                    roleToMod.Users.Value.Remove(new ConfigurationId(UserId));


                    roleToMod.Commit();
                }

                roleConfigurationList.StopCaching();
            }
        }
    }
}