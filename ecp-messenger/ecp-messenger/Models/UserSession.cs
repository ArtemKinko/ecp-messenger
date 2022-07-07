using System;
using System.Collections.Generic;
using System.Text;

namespace ecp_messenger.Models
{
    public class UserSession
    {
        private static UserSession instance;
        public string username;
        public int id;
        public List<Message> messages;

        private UserSession()
        {

        }

        public static UserSession getInstance()
        {
            if (instance == null)
                instance = new UserSession();
            return instance;
        }

        async public void updateMessages()
        {

        }
    }
}
