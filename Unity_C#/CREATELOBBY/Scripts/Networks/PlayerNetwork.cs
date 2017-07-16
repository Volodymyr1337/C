using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }
    private PhotonView PhotonView;
    private int PlayersInGame = 0;

    private void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        PlayerName = "android#" + Random.Range(100, 999);

        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GunFight")
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }

    private void MasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
        
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
    }
   
    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);       
    }

    [PunRPC]
    private void RPC_LoadedGameScene()  // вызывается только у мастера и показывает сколько игроков в игре
    {
        PlayersInGame++;
        
        if (PlayersInGame == PhotonNetwork.playerList.Length)
        {
            print("all players in the geme scene");
            PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }
    [PunRPC]
    private void RPC_CreatePlayer()
    {
        Vector3 spawnPos = new Vector3(1f, -5f, 0f);
        PhotonNetwork.Instantiate("User", spawnPos, Quaternion.identity, 0);
    }
}
