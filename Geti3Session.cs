using System.Management.Automation;
using ININ.IceLib.Connection;

namespace i3lets
{
    [Cmdlet(VerbsCommon.Get, "i3Session")]
    public class Geti3Session : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The session id of the session you want to get, it is preferable to store new-pssession into a variable and then pass that variable into this command.")]
        public long SessionId = -1;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (SessionId >= 0)
            {
                WriteObject(StaticDataStore.GetSession(SessionId));
            }
            else
            {
                foreach (Session session in StaticDataStore.GetAllSessions())
                {
                    WriteObject(session, true);
                }
            }
        }
    }
}