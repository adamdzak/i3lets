using System.Collections.Generic;
using ININ.IceLib.Connection;

namespace i3lets
{
    internal class StaticDataStore
    {
        private static readonly List<Session> _Sessions = new List<Session>();

        internal static void AddSession(Session session)
        {
            _Sessions.Add(session);
        }

        internal static Session GetSession(long id)
        {
            foreach (Session session in _Sessions)
            {
                if (session.SessionId == id)
                {
                    return session;
                }
            }
            return null;
        }

        internal static Session[] GetAllSessions()
        {
            return _Sessions.ToArray();
        }

        internal static void RemoveSession(Session session)
        {
            _Sessions.Remove(session);
        }
    }
}