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
    [Cmdlet(VerbsCommon.Add, "i3WorkgroupMember")]
    public class Addi3WorkgroupMember : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The workgroup to remove a user from.")]
        [ValidateNotNullOrEmpty]
        public string Workgroup;

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The UserID to remove from the workgroup.")]
        [ValidateNotNullOrEmpty]
        public string UserId;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if ((Workgroup != null) && (UserId != null))
            {
                ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
                WorkgroupConfigurationList workgroupConfigurationList = new WorkgroupConfigurationList(manager);
                var querySettings = workgroupConfigurationList.CreateQuerySettings();

                querySettings.SetFilterDefinition(WorkgroupConfiguration.Property.Id, Workgroup, FilterMatchType.Exact);
                querySettings.SetRightsFilterToAdmin();
                querySettings.SetPropertiesToRetrieveToAll();

                workgroupConfigurationList.StartCaching(querySettings);

                if (workgroupConfigurationList.GetConfigurationList().Count > 0)
                {
                    var workgroup = workgroupConfigurationList.GetConfigurationList()[0];
                    workgroup.PrepareForEdit();

                    try
                    {
                        workgroup.Members.Value.Add(new ConfigurationId(UserId));
                    }
                    catch
                    {
                        WriteWarning("UserID does not exist");
                    }

                    workgroup.Commit();
                }

                workgroupConfigurationList.StopCaching();
            }
        }
    }
}