using System;
using System.Management.Automation;
using ININ.IceLib.Connection;

namespace i3lets
{
    [Cmdlet(VerbsCommon.New, "i3Session")]
    public class Newi3Session : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Host name of the Interaction Center server.")]
        [ValidateNotNullOrEmpty]
        public string Host;

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "Optional Interaction Center user name. Windows authentication will be used if not provided.")]
        public string UserName;

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "Optional Interaction Center password. Windows authentication will be used if not provided.")]
        public string Password;

        #endregion Parameters

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            Session s = new Session();

            SessionSettings sessionSettings = new SessionSettings();
            sessionSettings.ClassOfService = ClassOfService.General;
            sessionSettings.IsoLanguage = "en-US";
            sessionSettings.ApplicationName = "i3lets";

            HostSettings hostSettings = new HostSettings();
            hostSettings.HostEndpoint = new HostEndpoint(Host, HostEndpoint.DefaultPort);

            AuthSettings authSettings;
            if (String.IsNullOrEmpty(UserName) || Password == null)
            {
                authSettings = new WindowsAuthSettings();
            }
            else
            {
                authSettings = new ICAuthSettings(UserName, Password);
            }

            StationSettings stationSettings = new StationlessSettings();

            s.Connect(sessionSettings, hostSettings, authSettings, stationSettings);

            StaticDataStore.AddSession(s);

            WriteObject(s);
        }
    }
}