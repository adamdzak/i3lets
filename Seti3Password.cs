using System.Management.Automation;
using ININ.IceLib.Configuration;

namespace i3lets
{
    [Cmdlet(VerbsCommon.Set, "i3Password", SupportsShouldProcess = true)]
    public class Seti3Password : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The new password to set for the specified users.")]
        public string NewPassword;

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The UserConfiguration for the user to delete.", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public UserConfiguration[] Users;

        [Parameter(HelpMessage = "If set to true force the change, otherwise return password policy violation errors.")]
        public SwitchParameter Force;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (UserConfiguration user in Users)
            {
                user.SetPassword(NewPassword, Force.IsPresent);
            }
        }
    }
}