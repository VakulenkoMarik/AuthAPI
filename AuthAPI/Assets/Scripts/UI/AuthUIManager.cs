using System.Collections;
using Data;
using Models;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
        
        private IEnumerator SendAuthRequest<T>(string url, T requestData, System.Action<Dtos.AuthResponse> callback) {
            string json = JsonConvert.SerializeObject(requestData);

            using UnityWebRequest www = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success) {
                var response = JsonConvert.DeserializeObject<Dtos.AuthResponse>(www.downloadHandler.text);
                callback?.Invoke(response);
            }
            else {
                callback?.Invoke(new Dtos.AuthResponse(false, "Server error", new UserData()));
            }
        }
        
        public void Register() {
            var request = new Dtos.RegisterRequest(
                registrationUsernameField.text,
                registrationPasswordField.text,
                registrationConfirmPasswordField.text
            );

            StartCoroutine(SendAuthRequest("http://localhost:5107/register", request, HandleResponse));
        }
        
        public void Login() {
            var request = new Dtos.LoginRequest(
                loginUsernameField.text,
                loginPasswordField.text
            );

            StartCoroutine(SendAuthRequest("http://localhost:5107/login", request, HandleResponse));
        }

        private void HandleResponse(Dtos.AuthResponse response) { 
            if (response.Success) {
                SceneManager.LoadScene("DemoMenu");
                References.SetCurrentUser(response.User);
                
                Debug.Log(References.GetCurrentUser().Username);
                Debug.Log(References.GetCurrentUser().Coins);
            }
            else {
                errorText.text = response.Message;
            }
        }
    }
}
