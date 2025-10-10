using AuthServer.Models;
using Newtonsoft.Json;

namespace AuthServer.Services
{
    public static class UserStore
    {
        private static string FilePath
        {
            get
            {
                string localLow = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "AuthServer"
                );
                Directory.CreateDirectory(localLow);
                return Path.Combine(localLow, "users.json");
            }
        }

        private static List<UserData> _users = new();

        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                _users = JsonConvert.DeserializeObject<List<UserData>>(json) ?? new List<UserData>();
            }
            else
            {
                _users = new List<UserData>();
            }
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(_users, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static bool TryAddUser(string username, string password)
        {
            if (_users.Any(u => u.Username == username))
                return false;

            _users.Add(new UserData { Username = username, Password = password });
            Save();
            return true;
        }

        public static UserData? ValidateUser(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
