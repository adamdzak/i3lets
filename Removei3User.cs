using System;
using System.Management.Automation;
using ININ.IceLib.Configuration;

namespace i3lets
{
    [Cmdlet(VerbsCommon.Remove, "i3User", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public class Removei3User : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The UserConfiguration for the user to delete.", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public UserConfiguration[] Users;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (UserConfiguration user in Users)
            {
                string detail = String.Format("Removing \"{0}\" ({1})", user.ConfigurationId.DisplayName, user.ConfigurationId.Id);
                if (ShouldProcess(detail, detail, "Are you sure you want to remove this user?"))
                {
                    user.Delete();
                }
            }
        }
    }
}