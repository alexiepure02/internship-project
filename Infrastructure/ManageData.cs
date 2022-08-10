using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure
{
    public sealed class ManageData
    {
        private static readonly object locker = new object();  
        private static ManageData _instance = null;

        private ManageData()
        {

        }

        public static ManageData Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new ManageData();
                        }
                    }
                }
                return _instance;
            }
            private set { }
        }

        public T GetItemsFromJson<T>(string jsonName) where T : new()
        {
            T items = new T();

            using (StreamReader r = new StreamReader($"../../../../Infrastructure/jsonFiles/{jsonName}.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<T>(json);
            }

            return items;
        }

        public void PutItemsIntoJson<T>(T items, string jsonName)
        {
            string jsonString = JsonSerializer.Serialize(items, new JsonSerializerOptions() { WriteIndented = true });

            using (StreamWriter outputFile = new StreamWriter($"../../../../Infrastructure/jsonFiles/{jsonName}.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }
    }
}
