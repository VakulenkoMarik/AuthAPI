using System;
using Data;
using Models;
using ServerConnection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class AuthUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField loginUsernameField;
        [SerializeField] private TMP_InputField loginPasswordField;
        
        [SerializeField] private TMP_InputField registrationUsernameField;
        [SerializeField] private TMP_InputField registrationPasswordField;
        [SerializeField] private TMP_InputField registrationConfirmPasswordField;

        [SerializeField] private TextMeshProUGUI errorText;

        private ServerRequestSender _serverRequestSender;

        private void Start() {
            _serverRequestSender = ServerRequestSender.Instance;
        }

        public void Register() {
            string username = registrationUsernameField.text;
            string password = registrationPasswordField.text;
            string confirmPassword = registrationConfirmPasswordField.text;
            
            _serverRequestSender.SendRegisterRequest(username, password, confirmPassword, HandleResponse);
        }
        
        public void Login() {
            string username = loginUsernameField.text;
            string password = loginPasswordField.text;
            
            _serverRequestSender.SendLoginRequest(username, password, HandleResponse);
        }

        private void HandleResponse(Dtos.AuthResponse response) { 
            if (response.Success) {
                SceneManager.LoadScene("DemoMenu");
                References.SetCurrentUser(response.User);
            }
            else {
                errorText.SetText(response.Message);
            }
        }
    }
}
