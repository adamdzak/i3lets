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
    [Cmdlet(VerbsCommon.Get, "i3Workgroup")]
    public class Geti3Workgroup : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The connected Session with the Interaction Center server.")]
        [ValidateNotNull]
        public Session Session;

        [Parameter(Mandatory = false, HelpMessage = "Workgroup name to filter by.")]
        public string Workgroup;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ConfigurationManager manager = ConfigurationManager.GetInstance(Session);
            WorkgroupConfigurationList workgroupConfigurationList = new WorkgroupConfigurationList(manager);
            var querySettings = workgroupConfigurationList.CreateQuerySettings();
            querySettings.SetFilterDefinition(WorkgroupConfiguration.Property.Id, Workgroup, FilterMatchType.Exact);

            if (Workgroup != null)
            {
                querySettings.SetPropertiesToRetrieveToAll();
                workgroupConfigurationList.StartCaching(querySettings);

                foreach (var workgroup in workgroupConfigurationList.GetConfigurationList())
                {
                    WriteObject(workgroup, true);
                }
            }

            workgroupConfigurationList.StopCaching();
        }
    }
}
