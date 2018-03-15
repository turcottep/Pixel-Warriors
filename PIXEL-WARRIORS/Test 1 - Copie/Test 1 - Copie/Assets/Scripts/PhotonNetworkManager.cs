using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//J'ai pas créer de FPS controller, c'est tu IMPORTANT, ctu iportant aussi la LOBBY camera;
public class PhotonNetworkManager : Photon.MonoBehaviour {

    [SerializeField] private Text connectText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lobbyCamera;
    [SerializeField] private Transform spawnPoint;


    // Use this for initialization
    private void Start()
    {
        //Doit avoir cette version pour se connecter
        PhotonNetwork.ConnectUsingSettings("0.1");

    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("We have now joined the lobby");
        // RoomOptions roomOptions = new RoomOptions();
   
        //Join a room if it exists or create one
        PhotonNetwork.JoinOrCreateRoom("new", null, null);
    }

    public virtual void OnJoinedRoom()
    {
        //Spawn le joueur
        PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation, 0);
        //Deactivate the lobby camera
        // J'EN AI PAS MIS
       // lobbyCamera.SetActive(false);
    }

	// Update is called once per frame
	private void Update ()
    {
        //NOTE: FOR TESTING ONLY
        connectText.text = PhotonNetwork.connectionStateDetailed.ToString();
	}
}
