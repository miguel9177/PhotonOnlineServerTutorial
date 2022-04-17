using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonTutorial.Menus
{
    //we use MonoBehaviourPunCallbacks, this is basically a "multiplayer Monobehaviour"
    public class MainMenuPrivateRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        //this is the main panel with the join or create options
        private GameObject joinOrCreateRoomPanel = null;
        [SerializeField]
        //this is the panel with the status, it show the user what hes doing (joining room etc)
        private GameObject waitingStatusPanel = null;
        [SerializeField]
        private TextMeshProUGUI waitingStatusText = null;

        [SerializeField]
        //this will store the input field with the create room name
        private TMP_InputField createRoomInputText;
        [SerializeField]
        //this will store the input field with the join room name
        private TMP_InputField joinRoomInputText;

        [SerializeField]
        //this will store the button with the create room 
        private Button createRoomButton;
        [SerializeField]
        //this will store the button with the join room 
        private Button joinRoomButton;

        //this will store if we are trying to connect to anything
        private bool isConnecting = false;
        //this bool, will let me control if we are creating a room, or just joining, so that i can call the function create room (or join, if this is false), when we connect to the master
        private bool isCreatingRoom=false;

        //this will store in which version this game is, so that we dont matchmake with different versions
        private const string GameVersion = "0.1";
        //this will store the maximum allowed players in a single room
        private const int MaxPlayersPerRoom = 2;

       
        private void Awake()
        {
            //this will make it that if a player changes scene everyone on the game changes scene aswell, so that if we are on a looby and the game starts we tell go to the scene, and every player will go to that scene
            PhotonNetwork.AutomaticallySyncScene = true;
            //this will make the buttons invisible on the beginning, since the text is empty
            StopInteractingWithButtons();
        }

        //this is called from the button Join room on the join/create room panel
        public void JoinRoomByName()
        {
            //tell the code that we are connecting
            isConnecting = true;
            //tell the code that we are joining a room and not creating
            isCreatingRoom = false;

            //this will make the find opponent panel invisible, and then show the waiting status panel instead
            joinOrCreateRoomPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            //show the player that we are seacrhing for a game
            waitingStatusText.text = "Searching Room...";

            //if we are not connected, connect us to the photon network
            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                //join the room with the correct name
                PhotonNetwork.JoinRoom(joinRoomInputText.text);
            }
         

        }

        //this is called when we click the button create room, and it will create a rooom
        public void CreateRoom()
        {
            //tell the code that we are connecting
            isConnecting = true;
            //tell the code that we are creating a room and not joining
            isCreatingRoom = true;

            //this will make the find opponent panel invisible, and then show the waiting status panel instead
            joinOrCreateRoomPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            //show the player that we are Creating a lobby
            waitingStatusText.text = "Creating room...";

            //if we are not connected, connect to the photon network
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                //create a room and give it a name, and tell the limit of players allowed on this room, (in this case 2)
                PhotonNetwork.CreateRoom(createRoomInputText.text, new RoomOptions { MaxPlayers = MaxPlayersPerRoom, IsVisible=false });
            }
           
          

        }

        IEnumerator ReturnToMenuAfterTimePassed(float time)
        {
            //wait some time and then do the code below
            yield return new WaitForSeconds(time);

            //this will make the join Or Create Room Panel visible, and then hide the waiting status panel 
            joinOrCreateRoomPanel.SetActive(true);
            waitingStatusPanel.SetActive(false);
        }

        //this will be called, everytime a text box changes
        public void StopInteractingWithButtons()
        {
            //if the text from the join room is empty, dont let the button be interacted
            joinRoomButton.interactable = !string.IsNullOrEmpty(joinRoomInputText.text);
            //if the text from the create room is empty, dont let the button be interacted
            createRoomButton.interactable = !string.IsNullOrEmpty(createRoomInputText.text);

          
        }

        //This is used when the client is connected to the master server
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");

            //if we are connecting and trying to create a room (not joining one)
            if(isConnecting && isCreatingRoom)
            {
                //we call the create room function (probably for the second time, since it was already called before, when we clicked the button create room)
                //we call it so that we can create a room, now that we are connected to the master
                CreateRoom();
            }

            //if we are connecting and trying to join a room (not creating one)
            else if (isConnecting && isCreatingRoom == false)
            {
                //we call the join room function (probably for the second time, since it was already called before, when we clicked the button join room)
                //we call it so that we can join a room, now that we are connected to the master
                JoinRoomByName();
            }
        }

        //if we desconnect from the client
        public override void OnDisconnected(DisconnectCause cause)
        {
            //hide the waiting status panel and show the findopponent panel
            waitingStatusPanel.SetActive(false);
            joinOrCreateRoomPanel.SetActive(true);

            Debug.Log($"Disconnected due to: {cause}");
        }

        //this will be called if we cant join the room, it can be because the room is full or other problem
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Couldnt join the room, maybe the code entered is wrong");
            //show the user that we couldnt join the room
            waitingStatusText.text = "Couldn't Join the room, maybe the room name is wrong.";
            StartCoroutine(ReturnToMenuAfterTimePassed(2));
        }

        //this will be called when the player has joined a room
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            //get the number of players in this room
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            //if the party is not full, say that we are waiting for an opponent
            if(playerCount!=MaxPlayersPerRoom)
            {
                waitingStatusText.text = "Waiting For Opponent";
                Debug.Log("Client is waiting for an opponent");
            }
            //if the party is full, the match is ready to begin
            else
            {
                waitingStatusText.text = "Opponent found";
                Debug.Log("Match is ready to begin");
            }
        }

        //if another player entered the room
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //if the current room is full
            if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                //tell the photon network, that this room is not open
                PhotonNetwork.CurrentRoom.IsOpen = false;

                waitingStatusText.text = "Opponent found";
                Debug.Log("Match is ready to begin");

                //since the match is ready to begin, we load the "Scene_Main" level
                PhotonNetwork.LoadLevel("Scene_Main_PrivateMatch");
            }
        }
    }
}

