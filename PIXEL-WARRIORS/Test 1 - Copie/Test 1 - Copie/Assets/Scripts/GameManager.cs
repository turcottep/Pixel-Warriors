using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.MyCompany.MyGame
{

    public class GameManager : Photon.PunBehaviour
    {

        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;


        #region Photon Messages


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                LoadArena();
            }
        }


        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                LoadArena();
            }
        }


        #endregion


        #region Public Methods

       

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion


        #region Private Methods

        //LA MAP DEPEND DU NOMBRE DE JOUEUR LIVE MAIS JVEUX PAS,JVEUX QUE CA DEPENDE DE CQUON CHOISIT
        // Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
        //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.room.PlayerCount);
        void LoadArena()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            GameObject manager = GameObject.FindGameObjectWithTag("Manager");
            int mapNumber = manager.GetComponent<MainMenu>().getMapNumber();
            Debug.Log("PhotonNetwork : Loading Level : " + mapNumber);
            PhotonNetwork.LoadLevel("MAP " + mapNumber.ToString());

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {

                if (Player.LocalPlayerInstance == null)
                {
                    Debug.Log("We are Instantiating LocalPlayer from ??? ");
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.Log("Ignoring scene load for &!& ");
                }
            }
        }

        private void Start()
        {
            
        }




        #endregion


    }
}
