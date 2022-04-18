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
    public class MainMenuServerList : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        //this will store the pannel that will let the user create a server
        private GameObject createServerPanel = null;

        [SerializeField]
        //this will store the pannel that will appear while entering a room or create one
        private GameObject waitingStatusPanel = null;

        [SerializeField]
        //this will store the text that will be displayed while joining a room or creating one
        private TMP_Text waitingStatusText = null;

        [SerializeField]
        //this will store the input field with the create room name
        private TMP_InputField createRoomInputText;

        [SerializeField]
        //this will store the button with the create room 
        private Button createRoomButton;
        

        //this will store if we are trying to connect to anything
        private bool isConnecting = false;
        //this will store if we are trying to create a room, or just joining
        private bool isCreatingRoom = false;

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

        private void Update()
        {
            //Debug.Log(PhotonNetwork.CountOfRooms);
        }

        //this is called from the button Join room on the join/create room panel
        public void JoinRoomByName()
        {
            //tell the code that we are connecting
            isConnecting = true;
            //tell the code that we are not creating a room, we are instead joining one
            isCreatingRoom = false;
            

            //show the player that we are seacrhing for a game
            waitingStatusText.text = "Searching Room...";

            //if we are not connected, connect us to the photon network
            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
         
         

        }

        //this is called when we click the button create room, and it will create a rooom
        public void CreateRoom()
        {
            //tell the code that we are connecting
            isConnecting = true;
            //tell the code that we are creating a room and not joining one
            isCreatingRoom = true;

            //show the waiting status panel, this panel will display information for the user
            createServerPanel.SetActive(false);
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
                PhotonNetwork.CreateRoom(createRoomInputText.text, new RoomOptions { MaxPlayers = MaxPlayersPerRoom, IsVisible=true, IsOpen = true}, new TypedLobby("FreeAll", LobbyType.Default));
         
            }
        }

     

        //this will be called, everytime a text box changes
        public void StopInteractingWithButtons()
        {
            //if the text from the create room is empty, dont let the button be interacted
            createRoomButton.interactable = !string.IsNullOrEmpty(createRoomInputText.text);
        }

        //This is used when the client is connected to the master server
        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");
            
            //if we are connecting to a room and we are creating a room, not joining one, call the create a room function again, since we already are connected to the master
            if(isConnecting==true && isCreatingRoom)
            {
                CreateRoom();
                isCreatingRoom = false;
                isConnecting = false;
            }
        }

        //if we desconnect from the client
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected due to: {cause}");
        }

        //this will be called if we cant join the room, it can be because the room is full or other problem
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("Couldnt join the room, maybe the code entered is wrong");
        }

        //this will be called when the player has joined a room
        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");
            //tell the photon network, that this room is not open
            PhotonNetwork.CurrentRoom.IsOpen = true;

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
            Debug.Log(PhotonNetwork.CountOfRooms);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {

            Debug.Log("Room List updated, caching...");
            Debug.Log("Room Count: " + roomList.Count);

            //currentRoomList = roomList; // Disregard

  /*          foreach (RoomInfo room in currentRoomList)
            {
                if (room.RemovedFromList)
                {
                    Debug.Log(room.Name + "(REMOVED)\n");
                }
                else
                {
                    Debug.Log(room.Name + "\n");
                }
            }
  */
            //DrawRoomList(currentRoomList); // Disregard
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

