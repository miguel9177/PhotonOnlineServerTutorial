using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonTutorial.Weapons
{
    public class Weapon : MonoBehaviourPun
    {
        //this will store how fast the bullet moves
        [SerializeField] private float projectileSpeed = 5f;
        //this will store the projectile that is going to be shooted from the gun
        [SerializeField] private GameObject projectile = null;
        //this will store where the bullets will spawn
        [SerializeField] private Transform spawnPoint = null;

        private void Update()
        {
            //if this is my photon view (if this is my character and not any other players character) call the function that thakes input
            if (photonView.IsMine)
            {
                //call the function that handles the input
                TakeInput();
            }
        }

        private void TakeInput()
        {
            //if the player hasnt clicked mouse button, leave this function
            if(!Input.GetMouseButtonDown(0)) { return; }
            //this will call the function FireProjectile, on every pc on the server
            photonView.RPC("FireProjectile", RpcTarget.All);
        }

        //this will run the code on the function bellow run on every computer of the server (inm this case we want to spawn this bullet in every pc, and move it accordingly
        [PunRPC]
        private void FireProjectile()
        {
            //this is going to spawn the bullet on every computer (since we are using PunRPC)
            GameObject projectileInstance = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            //this is going to add velocity to where the spawn bullet is facing, in this case is from the spawn point
            projectileInstance.GetComponent<Rigidbody>().velocity = projectileInstance.transform.forward * projectileSpeed;
        }
    }
}