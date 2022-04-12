using Photon.Pun;
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
            //since the user clicked the shooting button, call the function fire projectile, that is going to spawn a bullet on every player pc
            FireProjectile();
        }

    
        private void FireProjectile()
        {
            //this is going to spawn the bullet on every computer (since we are using PunRPC)
            GameObject projectileInstance = PhotonNetwork.Instantiate(projectile.name, spawnPoint.position, spawnPoint.rotation);
            //this is going to add velocity to where the spawn bullet is facing, in this case is from the spawn point
            projectileInstance.GetComponent<Rigidbody>().velocity = projectileInstance.transform.forward * projectileSpeed;
        }
    }
}