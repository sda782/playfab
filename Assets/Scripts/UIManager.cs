using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public PlayFabLogin playFabLogin;
    
    public InputField usernameInputField;
    public InputField emailInputField;
    public InputField passwordInputField;

    private void Awake() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1";
    }

    public void OnClick_LoginEmail() {
        playFabLogin.LoginEmail(emailInputField.text, passwordInputField.text);
    }

    public void OnClick_RegisterAccount() {
        playFabLogin.RegisterPlayFabUser(usernameInputField.text,emailInputField.text,passwordInputField.text);
    }

    public void OnClick_LoginCustom() {
        playFabLogin.LoginCustomId("AE#"+Time.time.GetHashCode(),true);
    }

    public void OnClick_SignOut() {
        playFabLogin.SignOut();
    }
}
