using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string SaveFiles;
    public GameData gameData = new GameData();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SaveFiles = Application.persistentDataPath + "/GameData.json"; //La localización de la carpeta donde están las SaveFiles
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadData();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveData();
        }

    }
    [RuntimeInitializeOnLoadMethod]
    public void LoadData()
    {
        if (File.Exists(SaveFiles))
        {
            string content = File.ReadAllText(SaveFiles);
            Debug.Log(content);
            GameData loadedData = JsonUtility.FromJson<GameData>(content);

            // Copy values from loadedData to the existing gameData
            gameData.coins = loadedData.coins;
            CoinManager.instance.totalCoins = gameData.coins;

            Debug.Log("Game data coins: " + gameData.coins);
        }

        else
        {
            Debug.Log("El archivo de guardado no existe");
        }
    }

    private void SaveData()
    {
        GameData newData = new GameData();
        {
            newData.coins = CoinManager.instance.totalCoins;
        };

        string JsonString = JsonUtility.ToJson(newData);

        File.WriteAllText(SaveFiles, JsonString);

        Debug.Log("Saved File");
    }


}
