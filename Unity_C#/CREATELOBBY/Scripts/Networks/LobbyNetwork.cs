using UnityEngine;

public class LobbyNetwork : MonoBehaviour
{

	void Start ()
    {
        print("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
        PhotonNetwork.autoJoinLobby = false;
    }

    private void OnConnectedToMaster()
    {
        print("connected to master!");
        // когда подключился к мастеру автоматом синхронизирует сцену со сценой мастера
        PhotonNetwork.automaticallySyncScene = false;        
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    private void OnJoinedLobby()
    {
        if (!PhotonNetwork.inRoom)  // если не в комнате - возврат на предыдущий канвас со списком лобби
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();

        print("joined loby!");
    }
}
