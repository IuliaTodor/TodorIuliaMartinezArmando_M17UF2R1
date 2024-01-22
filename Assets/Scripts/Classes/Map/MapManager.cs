using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private AssetBundle dungeonBundle;
    private Inputs input;
    private int[,] bitmap;
    private string filePath = Application.dataPath + "\\Scripts\\Classes\\Map\\Maps\\";
    private List<Room> roomLayout;
    Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    [SerializeField] public bool debug = true;
    [SerializeField] public bool canSave = false;
    [SerializeField] BoundsInt bounds;
    // IMPORTANTE: CAMBIAR EL FILENAME Y FILEPATH PARA QUE CONCUERDE CON LA CARPETA MAPS
    [SerializeField] string filename = "example.json";
    void Start()
    {
        dungeonBundle = AssetBundle.LoadFromFile(Application.dataPath + "\\AssetBundles\\dungeon_tiles");
        initTilemaps();

        input = new Inputs();
        if (debug) 
        {
            input.Enable();
            input.RoomManager.Load.performed += LoadRoom;
            if (canSave) input.RoomManager.Save.performed += SaveRoom;
        } else {
            initFloor();
        }
    }
    public void LoadRoom(InputAction.CallbackContext value) 
    {
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(filePath + filename);

        foreach(var mapData in data) {
            if(!tilemaps.ContainsKey(mapData.key))
            {
                // si por algún casual ese mapData.key no existe
                Debug.Log("Lol, lmao. " + mapData.key + " te lo has inventado.");
            }

            var map = tilemaps[mapData.key];
            map.ClearAllTiles(); // Que método tan conveniente, gracias Unity

            // Cada tilemap en el JSON, se coloca correctamente. Espero.
            if(mapData.tiles != null && mapData.tiles.Count > 0) {
                foreach(TileInfo tile in mapData.tiles){
                    map.SetTile(tile.position, dungeonBundle.LoadAsset<TileBase>("assets/tiles/" + tile.tilename + ".asset"));
                }
            }

        }

        Debug.Log(filename + " loaded.");
    }

    public void LoadRoom(Room nextRoom){
        // Yeah hice copiar y pegar idk
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(filePath + nextRoom.roomName);
        Debug.Log(filePath);
        foreach(var mapData in data) {
            if(!tilemaps.ContainsKey(mapData.key))
            {
                Debug.Log("Lol, lmao. " + mapData.key + " te lo has inventado.");
            }
            var map = tilemaps[mapData.key];
            map.ClearAllTiles(); 
            if(mapData.tiles != null && mapData.tiles.Count > 0) {
                foreach(TileInfo tile in mapData.tiles){
                    map.SetTile(tile.position, dungeonBundle.LoadAsset<TileBase>("assets/tiles/" + tile.tilename + ".asset"));
                }
            }
        }
    }

    void SaveRoom(InputAction.CallbackContext value) 
    {
        List<TilemapData> data = new List<TilemapData>();
        foreach(var mapObj in tilemaps) {
            TilemapData mapData = new TilemapData();
            mapData.key = mapObj.Key;

            // Sabes como me hice estas cicatrices?
            for(int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for(int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    TileBase tile = mapObj.Value.GetTile(pos);

                    if (tile != null)
                    {
                        TileInfo tileInfo = new TileInfo(pos, tile.name);
                        mapData.tiles.Add(tileInfo);
                    }
                }
            }
            data.Add(mapData);
        }
        // Usando FileHandler, guardamos la información de nuestro tilemapdata en un json
        FileHandler.SaveToJSON<TilemapData>(data, filePath + filename);
        Debug.Log(filename + " Map saved");
    }

    void initTilemaps()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach(var map in maps) { tilemaps.Add(map.name, map); }
    }

    void initFloor()
    {
        
        bitmap = new int[10, 10];
        Vector2 nextPosition = new Vector2 (UnityEngine.Random.Range(1, bitmap.GetLength(0) - 1), UnityEngine.Random.Range(1, bitmap.GetLength(1) - 1));

        MapUtils.AddRandomRooms(ref bitmap, ref nextPosition, ref roomLayout, 10);

        MapUtils.DebugMap(bitmap);
    }

}
[Serializable]
public class TilemapData {
    public string key;
    public List<TileInfo> tiles = new List<TileInfo>();
}

[Serializable]
public class TileInfo {
    public Vector3Int position;
    public string tilename;

    public TileInfo(Vector3Int pos, string name) {
        
        this.position = pos;
        this.tilename = name.ToLower();
        
    }
}

[Serializable]
public class Room {
    public string roomName;
    public int[] slots; 
    public List<Door> doors;
    public Vector2[] positionsInMap = new Vector2[4];

    public Room(string name) {
        this.roomName = name;
        this.slots = MapUtils.GetRoomSlots(name);
    }
}

[Serializable]
public class Door {
    public Room goTo;
    // La posición (dirección) de la puerta + el bit en el que está
    public Vector3 doorPosition;
    
}