using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotonManager : MonoBehaviourPunCallbacks {
    public Text info;
    private void Awake() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1";
    }

    public void ConnectToServer() {
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRandomRoom();
        }
        else {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1";
        }
    }

    private void UpdateText() {
        string txt = PhotonNetwork.CurrentRoom.Name;
        foreach (var player in PhotonNetwork.PlayerList) {
            txt += $"\nPlayerId: {player.UserId}";
        }

        info.text = txt;
    }

    #region Callbacks
    public override void OnJoinRandomFailed(short returnCode, string message) {
        var roomOptions = new RoomOptions {
            PublishUserId = true
        };
        PhotonNetwork.CreateRoom("R#" + Random.Range(0, 9999), roomOptions);
    }
    public override void OnJoinedRoom() {
        info.text = PhotonNetwork.CurrentRoom.Name;
        UpdateText();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("player joined "+ newPlayer.UserId);
        UpdateText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log("player left "+ otherPlayer.UserId);
        UpdateText();
        
    }
    #endregion
}
