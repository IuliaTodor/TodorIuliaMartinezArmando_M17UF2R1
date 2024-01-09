using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] public bool debug = true;
    private Inputs input;
    Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    [SerializeField] BoundsInt bounds;
    // IMPORTANTE: CAMBIAR EL FILENAME Y FILEPATH PARA QUE CONCUERDE CON LA CARPETA MAPS
    private string filePath = Application.dataPath + "\\Scripts\\Classes\\Map\\Maps\\";
    [SerializeField] string filename = "example.json";
    void Start()
    {
        MapUtils.DebugMap();
        MapUtils.getRoomPool(filePath);
        input = new Inputs();
        initTilemaps();
        if (debug) 
        {
            Debug.Log("huh");
            input.Enable();
            input.RoomManager.Load.performed += LoadRoom;
            input.RoomManager.Save.performed += SaveRoom;
        } else {
            initFloor();
        }
    }
    public void LoadRoom(InputAction.CallbackContext value) 
    {
        Debug.Log("AYUDA");
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
                    map.SetTile(tile.position, tile.tile);
                }
            }
        }

        Debug.Log(filename + " loaded.");
    }

    public void LoadRoom(string nextRoom){
        // Yeah hice copiar y pegar idk
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(filePath + nextRoom + ".json");

        foreach(var mapData in data) {
            if(!tilemaps.ContainsKey(mapData.key))
            {
                Debug.Log("Lol, lmao. " + mapData.key + " te lo has inventado.");
            }
            var map = tilemaps[mapData.key];
            map.ClearAllTiles(); 
            if(mapData.tiles != null && mapData.tiles.Count > 0) {
                foreach(TileInfo tile in mapData.tiles){
                    map.SetTile(tile.position, tile.tile);
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
                        TileInfo tileInfo = new TileInfo(tile, pos);
                        mapData.tiles.Add(tileInfo);
                    }
                }
            }
            data.Add(mapData);
        }
        // Usando FileHandler, guardamos la información de nuestro tilemapdata en un json
        FileHandler.SaveToJSON<TilemapData>(data, filePath + filename);
        Debug.Log("Map saved");
    }

    void initTilemaps()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach(var map in maps) { tilemaps.Add(map.name, map); }
    }

    void initFloor()
    {
        Room[,] floorLayout = new Room[5,5];
        
        MapUtils.GetRoomSlots(filename);
        MapUtils.DebugMap();
    }

}
[Serializable]
public class TilemapData {
    public string key;
    public List<TileInfo> tiles = new List<TileInfo>();
}

[Serializable]
public class TileInfo {
    public TileBase tile;
    public Vector3Int position;
    public string tilename;

    public TileInfo(TileBase tile, Vector3Int pos) {
        
        this.tile = tile;
        this.position = pos;
        
    }
}

[Serializable]
public class Room {
    public string roomName;
    public int[,] slots = new int[2,2]; 

    public Room(string name, int[,] slots) {
        this.roomName = name;
        this.slots = slots;
    }
}