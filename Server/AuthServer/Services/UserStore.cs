using Newtonsoft.Json;

namespace Server.Services
{
    public static class UserStore
    {
        private static string FilePath
        {
            get
            {
                string folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "AuthServer"
                );

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                return Path.Combine(folder, "users.json");
            }
        }

        private static Dictionary<string, string> _users = new();

        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                _users = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                         ?? new Dictionary<string, string>();
            }
            else
            {
                _users = new Dictionary<string, string>();
            }
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(_users, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static bool TryAddUser(string username, string password)
        {
            if (!_users.TryAdd(username, password))
                return false;

            Save();
            return true;
        }

        public static bool ValidateUser(string username, string password)
        {
            return _users.TryGetValue(username, out string storedPassword) && storedPassword == password;
        }
    }
}
