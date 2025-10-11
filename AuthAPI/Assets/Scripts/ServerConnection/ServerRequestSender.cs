using System;
using System.Collections;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace ServerConnection
{
    public class ServerRequestSender : MonoBehaviour
    {
        private const string ServerURL = "http://localhost:5107";

        public static ServerRequestSender Instance;

        private void Awake() {
            Instance = this;
        }
        
        public void SendRegisterRequest(string username, string password, string confirmPassword, Action<Dtos.AuthResponse> handleResponse) {
            var request = new Dtos.RegisterRequest(username, password, confirmPassword);

            StartCoroutine(SendAuthRequest("/register", request, handleResponse));
        }
        
        public void SendLoginRequest(string username, string password, Action<Dtos.AuthResponse> handleResponse) {
            var request = new Dtos.LoginRequest(username, password);

            StartCoroutine(SendAuthRequest("/login", request, handleResponse));
        }

        private IEnumerator SendAuthRequest<T>(string command, T requestData, Action<Dtos.AuthResponse> callback) {
            string json = JsonConvert.SerializeObject(requestData);

            using UnityWebRequest www = new UnityWebRequest(ServerURL + command, "POST");
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
    }
}