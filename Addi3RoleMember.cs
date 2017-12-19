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
    [Cmdlet(VerbsCommon.Add, "i3RoleMember")]
    public class Addi3RoleMember : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The Role to add a user to.")]
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
                    var role = roleConfigurationList.GetConfigurationList()[0];
                    role.PrepareForEdit();

                    try
                    {
                        role.Users.Value.Add(new ConfigurationId(UserId));
                    }
                    catch
                    {
                        WriteWarning("UserID does not exist");
                    }


                    role.Commit();
                }

                roleConfigurationList.StopCaching();
            }
        }
    }
}