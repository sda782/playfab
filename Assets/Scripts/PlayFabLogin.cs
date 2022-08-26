using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoginResult = PlayFab.ClientModels.LoginResult;
using PlayFabError = PlayFab.PlayFabError;

public class PlayFabLogin : MonoBehaviour {
    public Text infoText;
    private string playFabPlayerIdCache;

    public void LoginCustomId(string customId, bool createAccount) {
        var request = new LoginWithCustomIDRequest {
            CustomId = customId,
            CreateAccount = createAccount
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnFailure);
    }
    
    public void RegisterPlayFabUser(string username, string email, string password) {
        var request = new RegisterPlayFabUserRequest {
            Username = username,
            Email = email,
            Password = password
        };
        PlayFabClientAPI.RegisterPlayFabUser(request,OnAccountCreationSuccess, OnFailure);
    }
    
    public void LoginEmail(string email, string password) {
        var request = new LoginWithEmailAddressRequest {
            Email = email,
            Password =password
        };            
        PlayFabClientAPI.LoginWithEmailAddress(request,OnLoginSuccess, OnFailure);
    }

    public void SignOut() {
        PlayFabClientAPI.ForgetAllCredentials();
        infoText.text = "Signed out";
    }
    
    private void RequestPhotonToken(LoginResult obj) {
        playFabPlayerIdCache = obj.PlayFabId;
        
        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest() {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
        }, AuthenticateWithPhoton, OnFailure);
    }

    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj) {
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        customAuth.AddAuthParameter("username", playFabPlayerIdCache);    // expected by PlayFab custom auth service
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        PhotonNetwork.AuthValues = customAuth;
        SceneManager.LoadScene(1);
    }
    
    #region Callback
    private void OnAccountCreationSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Account was created successfully");
        infoText.text = result.PlayFabId;
    }
    
    private void OnLoginSuccess(LoginResult result) {
        Debug.Log("Anonymous Login successful");
        Debug.Log(result.PlayFabId);
        infoText.text = result.PlayFabId;
        RequestPhotonToken(result);
    }

    private void OnFailure(PlayFabError error) {
        Debug.LogWarning("Something went wrong");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        infoText.text = error.ErrorMessage;
    }
    #endregion
    
}
