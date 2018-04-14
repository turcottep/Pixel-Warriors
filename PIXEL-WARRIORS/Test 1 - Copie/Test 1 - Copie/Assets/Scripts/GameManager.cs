using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.MyCompany.MyGame
{
    public class GameManager : Photon.PunBehaviour
    {


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
            Debug.Log("PhotonNetwork : Loading Level : " + "1");
            PhotonNetwork.LoadLevel("MAP " + "1");
        }




        #endregion


    }
}
