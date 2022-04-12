using UnityEngine;

namespace PhotonTutorial.Utilities
{
    //this script will make the gameobject rotate to where the camera is facing
    public class Billboard : MonoBehaviour
    {
        //this will get the transform of the maincamera
        private Transform mainCameraTransform;

        private void Start()
        {
            //this will get the transform of the maincamera
            mainCameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            //rotate the object to where the camera is facing
            transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
                mainCameraTransform.rotation * Vector3.up);
        }
    }

}
