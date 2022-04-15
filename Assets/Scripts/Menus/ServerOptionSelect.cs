using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ServerOptionSelect : MonoBehaviour
{
    public void LoadCasualMatchMenuScene()
    {
        SceneManager.LoadScene("Scene_MainMenu");
    }

    public void LoadPrivateMatchMenuScene()
    {
        SceneManager.LoadScene("Scene_MainMenu_PrivateMatch");
    }
}
