using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private AssetBundle dungeonBundle;
    public Vector2Int[,] possibleDoorPositions = new Vector2Int[,] {
                            { new Vector2Int(8, -2), new Vector2Int(0, 4), new Vector2Int(0, -9), new Vector2Int(-9, -2)}, // cool
                            { new Vector2Int(26, -2), new Vector2Int(18, 4), new Vector2Int(18, -9), new Vector2Int(9, -2)},
                            { new Vector2Int(8, -12), new Vector2Int(0, -9), new Vector2Int(0, -21), new Vector2Int(-9, -12)},
                            { new Vector2Int(26, -12), new Vector2Int(18, -9), new Vector2Int(18, -21), new Vector2Int(9, -12)}
                            };

    private Inputs input;
    public int[,] bitmap;
    private string filePath = Application.dataPath + "\\Scripts\\Classes\\Map\\Maps\\";
    public List<Room> roomLayout = new List<Room>();
    Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    [SerializeField] public bool debug;
    [SerializeField] public bool canSave;
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
        DoorTo doorTo = GameObject.FindObjectOfType<DoorTo>();
        // Yeah hice copiar y pegar idk
        List<TilemapData> data = FileHandler.ReadListFromJSON<TilemapData>(filePath + nextRoom.roomName);
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

            if (mapData.key == "Door") {
                doorTo.doors = new List<Door>();
                doorTo.doorPositions = new List<Vector2>();
                foreach (Door d in nextRoom.doors) {

                    int size = 0;
                    foreach(int n in nextRoom.slots) if (n == 1) size++;
                    
                    if (nextRoom.slots[0] == 0 && size == 1) {
                        for (int i = 1; i < nextRoom.slots.Length; i++) if (nextRoom.slots[i] == 1) d.doorPosition.z -= i;
                    }

                    if (nextRoom.slots[0] == 0 && nextRoom.slots[2] == 0 && size == 2) d.doorPosition.z -= 1;
                    else if (nextRoom.slots[0] == 0 && nextRoom.slots[1] == 0 && size == 2) d.doorPosition.z -= 2;


                    Vector2 doorPos = new Vector3(0, 0, 0);
                    switch((Vector2)d.doorPosition) {
                        case Vector2 v when v.Equals(Vector2.right):
                            doorPos = possibleDoorPositions[(int)d.doorPosition.z, 0];
                            break;
                        case Vector2 v when v.Equals(Vector2.down):
                            doorPos = possibleDoorPositions[(int)d.doorPosition.z, 1];
                            break;
                        case Vector2 v when v.Equals(Vector2.up):
                            doorPos = possibleDoorPositions[(int)d.doorPosition.z, 2];
                            break;
                        case Vector2 v when v.Equals(Vector2.left):
                            doorPos = possibleDoorPositions[(int)d.doorPosition.z, 3];
                            break;
                    }
                    map.SetTile( new Vector3Int((int)doorPos.x, (int)doorPos.y, 1), dungeonBundle.LoadAsset<TileBase>("assets/tiles/dungeon_tileset_66.asset"));
                    doorTo.doors.Add(d);
                    doorTo.doorPositions.Add(new Vector2(doorPos.x, doorPos.y));
                }
            }
        } 
        Debug.Log(nextRoom.roomName + " loaded.");
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
        foreach(Room r in roomLayout) {
            r.doors = MapUtils.GetAdjacentRoomDoors(bitmap, r);
        }

        LoadRoom(roomLayout[0]);

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
    public int goToIndex;
    // La posición (dirección) de la puerta + el bit en el que está
    public Vector3 doorPosition;

    public Door(int index, Vector3 pos) {
        this.goToIndex = index;
        this.doorPosition = pos;
    }
}