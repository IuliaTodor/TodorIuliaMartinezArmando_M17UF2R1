using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        DataManager.instance.LoadData();
        Debug.Log(CoinManager.instance.totalCoins);
    }

    public void StartGame()
    {
        //GameManager.instance.SceneChange("Example_Iulia1");
        GameManager.instance.SceneChange("MainScene");
        FindObjectOfType<AudioManager>().StopPlaying("RLMainMenu");
    }

    public void MainMenu()
    {
        GameManager.instance.SceneChange("MainMenu");
    }

    public void ExitGame()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Application.Quit();
        }
    }

}
