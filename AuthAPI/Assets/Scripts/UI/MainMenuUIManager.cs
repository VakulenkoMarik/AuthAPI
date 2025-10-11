using Data;
using Models;
using ServerConnection;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI userNameText;
        [SerializeField] private TextMeshProUGUI coinsCountText;

        private UserData _currentUser;

        private void Start() {
            UserData currentUser = References.GetCurrentUser();

            userNameText.SetText(currentUser.Username);
            coinsCountText.SetText(currentUser.Coins + "");

            _currentUser = currentUser;
        }

        public void AddCoin() {
            _currentUser.Coins++;
            coinsCountText.SetText(_currentUser.Coins + "");
        }
        
        private void OnApplicationQuit() {
            ServerRequestSender.Instance.SendUpdateCoinsRequest(_currentUser.Username, _currentUser.Coins, null);
        }
    }
}
