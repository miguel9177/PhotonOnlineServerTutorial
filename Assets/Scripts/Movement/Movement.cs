using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonTutorial.Movement
{
    //make it mandatory that this gameobject have a character controller
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviourPun
    {
        [SerializeField] private float movementSpeed = 0f;

        //this will store this object character controller
        private CharacterController controller = null;

        //this will get the character controller on this game object and store it on controller variable
        void Start() => controller = GetComponent<CharacterController>();
        
        // Update is called once per frame
        void Update()
        {
            //if this is my photon view (if this is my character and not any other players character) call the function that thakes input and moves the object
            if(photonView.IsMine)
            {
                //call the function that gets the input and moves the object
                TakeInput();
            }
        }

        //this will take the input and move the object, this is only called if this object is mine and not from another online player
        private void TakeInput()
        {
            //create a vector that is going to get the movement of the character by getting the values from the arrow keys
            Vector3 movement = new Vector3
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = 0f,
                z = Input.GetAxisRaw("Vertical"),
            }.normalized;
            //move the object
            controller.SimpleMove(movement * movementSpeed);
        }
    }

}
