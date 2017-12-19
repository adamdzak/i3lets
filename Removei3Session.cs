using System.Management.Automation;
using ININ.IceLib.Connection;

namespace i3lets
{
    [Cmdlet(VerbsCommon.Remove, "i3Session")]
    public class Removei3Session : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The session id of the session you want to remove.", ValueFromPipeline = true)]
        [ValidateNotNull]
        public Session[] Session;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            foreach (Session session in Session)
            {
                StaticDataStore.RemoveSession(session);
                session.Disconnect();
            }
        }
    }
}