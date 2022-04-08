using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

namespace PhotonTutorial.Menus
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] 
        //this will store the textbox for the players name 
        TMP_InputField nameInputField = null;
        [SerializeField]
        //this will store the continue button for the players name input menu
        Button continueButton = null;

        //key for the name for the saving system
        private const string PlayerPrefsNameKey = "PlayerName";

        //on start call the function that is going to setup the input field
        private void Start() => SetUpInputField();
        


        private void SetUpInputField()
        {
            //if we havent saved a player name yet return, since we dont have the name
            if(!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

            //sinc ewe do have the name key saved on p+layer prefs, we get that name
            string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

            //write on the input field the stored player name
            nameInputField.text = defaultName;

            //call the function set Player name and send the stored player name
            SetPlayerName(defaultName);
        }

        //this will be called every time the name texbox changes, and will send that value to the name string
        public void SetPlayerName(string name)
        {
            //make the button interactable of the name of the player is not null
            continueButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SavePlayerName()
        {
            //get the player name from the name text box
            string playerName = nameInputField.text;

            //tell the photon network, wich name this client is
            PhotonNetwork.NickName = playerName;

            //store the player name on the player prefs so that the next time we load the game we remember the name
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
        }
    }
}
