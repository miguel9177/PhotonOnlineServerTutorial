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
        //this will store the camera transform, so that we can move to where the camera is looking at
        private Transform mainCameraTransform = null;

        //this will get the character controller on this game object and store it on controller variable
        void Start()
        {
            //this will get the charaacter controller component
            controller = GetComponent<CharacterController>();
            //store the camera transform, so that we can move to where the camera is looking at
            mainCameraTransform = Camera.main.transform;
        }
        
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

            //this will get the forwaRD and the right of the camera
            Vector3 forward = mainCameraTransform.forward;
            Vector3 right = mainCameraTransform.right;

            //we put 0 to y, since we dont care about the tilting of the camera (looking up or down, doesnt matter in this case since we want to move to where the camera is looking)
            forward.y = 0f;
            right.y = 0f;
            
            //we normalize forward so that the vector keeps the same direction but its lenght is 1.0, and then we do the same for the right
            forward.Normalize();
            right.Normalize();

            //this calculation will make the character move to where the camera is facing
            Vector3 calculatedMovement = (forward * movement.z + right * movement.x).normalized;

            //if the calculated movemet isnt 0, rotate the object
            if(calculatedMovement != Vector3.zero)
                //this will rotate the object to where its moving
                transform.rotation = Quaternion.LookRotation(calculatedMovement);

            //move the object
            controller.SimpleMove(calculatedMovement * movementSpeed);
        }
    }

}
