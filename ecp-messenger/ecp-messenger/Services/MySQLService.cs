using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using ecp_messenger.Models;

namespace ecp_messenger.Services
{
    public class MySQLService
    {
        private static string host;
        private static string connectionString;
        private static MySQLService instance;

        private MySQLService(string ip)
        {
            host = ip;
            connectionString = "server=" + host + ";port=8888;user=ecp_messenger_user;password=user;database=ecp_messenger;";

        }

        public void updateHost(string host)
        {
            connectionString = "server=" + host + ";port=8888;user=ecp_messenger_user;password=user;database=ecp_messenger;";
        }

        public static MySQLService getInstance(string ip = "51.250.21.110")
        {
            if (instance == null)
                instance = new MySQLService(ip);
            return instance;
        }

        public async Task getAllUsers()
        {
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            var command = connection.CreateCommand();
            command.CommandText = "select * from user";
            var reader = command.ExecuteReader();

            List<User> users = new List<User>();
            while (reader.Read())
            {
                try
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32("userId"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("password")
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                System.Diagnostics.Debug.WriteLine(users[0].Id);
                System.Diagnostics.Debug.WriteLine(users[0].Username);
                System.Diagnostics.Debug.WriteLine(users[0].Password);
            }
            connection.Close();
        }

        public bool checkUserAccess(string username, string password)
        {
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            var command = connection.CreateCommand();
            command.CommandText = "select password from user where username = @name";
            command.Parameters.Add("@name", MySqlDbType.VarString).Value = username;
            var reader = command.ExecuteReader();
            string hash = "";
            while (reader.Read())
            {
                hash = reader.GetString("password");
            }
            try
            {
                if (BCrypt.Net.BCrypt.Verify(password, hash))
                    return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public List<Message> getAllMessagesMoreThan(string username, int id)
        {
            List<Message> messages = new List<Message>();
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            var command = connection.CreateCommand();
            command.CommandText = "select * from message join user u1 on u1.userId = message.fromId join user u2 on u2.userId = message.toId where ((u1.username = @username) or (u2.username = @username)) and message.messageId > @id;";
            command.Parameters.Add("@username", MySqlDbType.VarString).Value = username;
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                //Message message;
                try
                {
                    DateTime date = reader.GetDateTime("sendTime");
                    string dateStr = (date.Day < 10 ? "0" : "") + date.Day + "." + (date.Month < 10 ? "0" : "") + date.Month
                        + " " + (date.Hour < 10 ? "0" : "") + date.Hour + ":" + (date.Minute < 10 ? "0" : "") + date.Minute;
                    Message message = new Message
                    {
                        Id = reader.GetInt32("messageId"),
                        fromId = reader.GetInt32("fromId"),
                        toId = reader.GetInt32("toId"),
                        text = reader.GetString("text"),
                        sendTime = dateStr
                    };
                    System.Diagnostics.Debug.WriteLine(message.text + message.sendTime);
                    messages.Add(message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return messages;
        }

        public int findIdByUsername(string username)
        {
            User users = new User();
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            var command = connection.CreateCommand();
            command.CommandText = "select userId from user where username = @username";
            command.Parameters.Add("@username", MySqlDbType.VarString).Value = username;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                return reader.GetInt32("userId");
            }
            return -1;
        }

        public void insertMessage(Message message)
        {
            User users = new User();
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "insert into message(fromId, toId, text, sendTime) values(@fromId, @toId, @text, @sendTime);";
                command.Parameters.Add("@fromId", MySqlDbType.Int32).Value = message.fromId;
                command.Parameters.Add("@toId", MySqlDbType.Int32).Value = message.toId;
                command.Parameters.Add("@text", MySqlDbType.VarString).Value = message.text;
                command.Parameters.Add("@sendTime", MySqlDbType.DateTime).Value = message.sendTime;
                command.ExecuteReader();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public void insertUser(User user)
        {
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "insert into user(username, password) values (@username, @hash)";
                command.Parameters.Add("@username", MySqlDbType.VarString).Value = user.Username;
                command.Parameters.Add("@hash", MySqlDbType.VarString).Value = user.Password;
                command.ExecuteReader();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public User getUserById(int id)
        {
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            var command = connection.CreateCommand();
            try
            {
                command.CommandText = "select username from user where (userId = @id);";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var user = new User
                    {
                        Id = id,
                        Username = reader.GetString("username"),
                        Password = ""
                    };
                    return user;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
