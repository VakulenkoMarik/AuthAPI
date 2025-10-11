using Data;
using Models;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI userNameText;
        [SerializeField] private TextMeshProUGUI coinsCountText;

        private void Start() {
            UserData currentUser = References.GetCurrentUser();

            userNameText.SetText(currentUser.Username);
            coinsCountText.SetText(currentUser.Coins + "");
        }
    }
}
