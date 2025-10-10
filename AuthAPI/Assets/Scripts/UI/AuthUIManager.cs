using AuthServer.Services;
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
        
        private void Awake() {
            UserStore.Load();
        }
            
        public void Register() {
            if (TryCreateNewUser()) {
                SceneManager.LoadScene("DemoMenu");
            } else {
                errorText.text = "registration error";
            }
        }

        private bool TryCreateNewUser() {
            bool isPasswordTheSame = registrationPasswordField.text.Equals(registrationConfirmPasswordField.text);

            if (isPasswordTheSame) {
                return UserStore.TryAddUser(registrationUsernameField.text, registrationPasswordField.text);
            }

            return false;
        }

        public void Login() {
            bool isCorrectUserPasswordPair = UserStore.ValidateUser(loginUsernameField.text, loginPasswordField.text);
            
            if (isCorrectUserPasswordPair) {
                SceneManager.LoadScene("DemoMenu");
            } else {
                errorText.text = "login error";
            }
        }
    }
}
