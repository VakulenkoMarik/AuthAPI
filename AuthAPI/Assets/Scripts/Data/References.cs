using Models;

namespace Data
{
    public static class References
    {
        private static UserData _currentUser;
        
        public static UserData GetCurrentUser() {
            return _currentUser;
        }
        
        public static void SetCurrentUser(UserData newCurrentUser) {
            _currentUser = newCurrentUser;
        }
    }
}