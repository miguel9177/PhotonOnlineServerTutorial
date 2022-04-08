using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonTutorial
{
    public class PlayerSpawner : MonoBehaviour
    {
        //this will store wich player this script will spawn
        [SerializeField] private GameObject playerPrefab = null;
        private void Start()
        {
            //Spawn "online" the player prefab, and assign the photon view to us, with the instatiate keyword this object will be assigned to us, and we are the ones who can move it
            //if we do the if photonView.IsMine, and will spawn it on 000 positionm and 000 rotation
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }

}

