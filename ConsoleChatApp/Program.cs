using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleChatApp
{
    internal class Program
    {
        public static List<User> GetUsersFromJson()
        {
            List<User> users = new List<User>();

            using (StreamReader r = new StreamReader("../../../users.json"))
            {
                string json = r.ReadToEnd();
                users = JsonSerializer.Deserialize<List<User>>(json);
            }

            try
            {
            return users;
            }
            catch (FieldAccessException ex)
            {
                Console.WriteLine("Error with the users file\n\n" + ex.Message);
                return new List<User>();
            }
        }

        public static List<Message> GetMessagesFromJson()
        {
            List<Message> messages = new List<Message>();

            using (StreamReader r = new StreamReader("../../../messages.json"))
            {
                string json = r.ReadToEnd();
                messages = JsonSerializer.Deserialize<List<Message>>(json);
            }

            try
            {
                return messages;
            }
            catch (FieldAccessException ex)
            {
                Console.WriteLine("Error with the users file\n\n" + ex.Message);
                return new List<Message>();
            }
        }

        static int Main(string[] args)
        {
            List<User> users = GetUsersFromJson();
            List<Message> messages = GetMessagesFromJson();

            return 0;
        }
    }
}