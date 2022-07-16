using System;
using System.Collections.Generic;
using System.Text;
using ecp_messenger.Services;

namespace ecp_messenger.Models
{
    public class UserSession
    {
        private static UserSession instance;
        public string username;
        public int id;
        public int lastMessageId;
        public List<Message> messages;
        public List<Dialog> dialogs;

        private UserSession()
        {

        }

        public static UserSession getInstance()
        {
            if (instance == null)
                instance = new UserSession();
            return instance;
        }

        public List<Message> getMessagesByUsername(string username)
        {
            foreach (Dialog dialog in dialogs)
                if (dialog.User.Username == username)
                    return dialog.messages;
            return null;
        }

        public List<Message> updateMessages(string username)
        {
            List<Message> mesRet = new List<Message>();
            try
            { 
                List<Message> mes = MySQLService.getInstance().getAllMessagesMoreThan(this.username, lastMessageId);
                foreach (Message m in mes)
                {
                    lastMessageId = m.Id;
                    bool newPerson = true;
                    foreach (Dialog d in dialogs)
                    {
                        if (d.User.Id == (m.fromId == id ? m.toId : m.fromId))
                        {
                            newPerson = false;
                            d.messages.Add(m);
                            if (d.User.Username == username && m.fromId != id)
                                mesRet.Add(m);
                            break;
                        }
                    }
                    if (newPerson)
                    {
                        User user = MySQLService.getInstance().getUserById(m.fromId == id ? m.toId : m.fromId);
                        Dialog dialog = new Dialog
                        {
                            User = user,
                            messages = new List<Message>(),
                        };
                        dialog.messages.Add(m);
                        dialogs.Add(dialog);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return mesRet;
        }

        public void initDialogs()
        {
            dialogs = new List<Dialog>();
            try
            {
                List<Message> mes = MySQLService.getInstance().getAllMessagesMoreThan(username, -1);
                foreach (Message m in mes)
                {
                    lastMessageId = m.Id;
                    bool newPerson = true;
                    foreach (Dialog d in dialogs)
                    {
                        if (d.User.Id == (m.fromId == id ? m.toId : m.fromId))
                        {
                            newPerson = false;
                            d.messages.Add(m);
                            break;
                        }
                    }
                    if (newPerson)
                    {
                        User user = MySQLService.getInstance().getUserById(m.fromId == id ? m.toId : m.fromId);
                        Dialog dialog = new Dialog
                        {
                            User = user,
                            messages = new List<Message>(),
                        };
                        dialog.messages.Add(m);
                        dialogs.Add(dialog);
                    }
                }
            } catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void debugShowDialogs()
        {
            foreach (Dialog dialog in dialogs)
            {
                System.Diagnostics.Debug.WriteLine(dialog.User.Username);
                foreach(Message m in dialog.messages)
                {
                    System.Diagnostics.Debug.WriteLine(m.fromId);
                    System.Diagnostics.Debug.WriteLine(m.text);
                }
            }
        }
    }
}
