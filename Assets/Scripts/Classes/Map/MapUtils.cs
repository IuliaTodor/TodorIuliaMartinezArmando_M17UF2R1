using System;
using System.Collections.Generic;
using System.IO;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public static class MapUtils
{
    public static List<string> GetRoomNamesWithBit(Vector2 direction) {
        List<string> rooms = new List<string>(); 
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "\\Scripts\\Classes\\Map\\Maps\\");
        foreach (var file in dir.GetFiles("*.json")) {
            // Debug.Log(file.Name);
            if (file.Name != "example.json") {
                if (direction == Vector2.right && GetRoomSlots(file.Name)[ 0 ] == 1) rooms.Add(file.Name);
                else if (direction == Vector2.up && GetRoomSlots(file.Name)[ 1 ] == 1) rooms.Add(file.Name);
                else if (direction == Vector2.down && GetRoomSlots(file.Name)[ 2 ] == 1) rooms.Add(file.Name);
                else if (direction == Vector2.left && GetRoomSlots(file.Name)[ 3 ] == 1) rooms.Add(file.Name);
            }
        }
        return rooms;
    }

    
    public static void DebugMap(int[,] bitmap)
    {
        string mappedLayout = " 0 0 1 2 3 4 5 6 7 8 9";

        for (int x = 0; x < bitmap.GetLength(0); x++) {
            mappedLayout += $"\n {x} ";
            for (int y = 0; y < bitmap.GetLength(1); y++) {
                mappedLayout += $"{bitmap[x, y]} ";
            }
        }
        Debug.Log(mappedLayout);
    }

    // Los nombres contendrán la información de cuantos slots ocupa la sala
    public static int[] GetRoomSlots(string floorName) {
        int[] slots = new int[4];
        slots[0] = int.Parse($"{floorName[floorName.Length - 9]}");
        slots[1] = int.Parse($"{floorName[floorName.Length - 8]}");
        slots[2] = int.Parse($"{floorName[floorName.Length - 7]}");
        slots[3] = int.Parse($"{floorName[floorName.Length - 6]}");
        return slots;
    }

    // Añadir una habitación random nueva al layout
    public static void AddRandomRooms(ref int[,] bitmap, ref Vector2 currentPosition, ref List<Room> rooms, int nRooms) {
        // Posibilidad random de bifurcar el camino
        if (UnityEngine.Random.Range(0, 8) == 4 && nRooms > 2) 
        {
            AddRandomRooms(ref bitmap, ref currentPosition, ref rooms, nRooms / 2);
            nRooms -= nRooms / 2;
        }
        // Genera una nueva posición en la que colocar una habitación
        Vector2 newPosition = currentPosition;
        Room newRoom; //algunos valores por defecto idk
    
        while (newPosition == currentPosition) {
            // Se intenta generar una sala, en una dirección y se comrpueba que haya espacios
            Vector2 newDirection = RandomDirection();
            newRoom = RandomRoom(newDirection);
            Vector2 possiblePosition = currentPosition + newDirection;
            
            if (possiblePosition.x >= 1 && possiblePosition.y >= 1 &&
            possiblePosition.x <= bitmap.GetLength(0) - 2 && possiblePosition.y <= bitmap.GetLength(1) - 2){
                if (HasAvailableSpace(bitmap, possiblePosition, newRoom.slots, newDirection)) {
                    Debug.Log("Tried in: " + possiblePosition.x + ", " +  possiblePosition.y + " with " + newRoom.roomName + "\n direction:" + newDirection);
                    newPosition = possiblePosition;
                    PaintRoom(ref bitmap, newPosition, ref newRoom, newDirection);
                    newRoom.doors = GetAdjacentRoomDoors(bitmap, newPosition, newRoom, newDirection);
                    rooms.Add(newRoom);
                } else if (bitmap[(int)possiblePosition.y, (int)possiblePosition.x] > 0)
                { 
                    currentPosition = possiblePosition;
                    newPosition = currentPosition;
                }
            }
        }
        currentPosition = newPosition;
        
        // Añadir habitaciones hasta que se acabe nRooms
        if (nRooms > 0) AddRandomRooms(ref bitmap, ref currentPosition, ref rooms, nRooms - 1);
    }

    private static List<Door> GetAdjacentRoomDoors(int[,] bitmap, Vector2 position, Room room, Vector2 direction) {

        List<Door> doors = new List<Door>();
        return doors;
    }
    // Devuelve una sala aleatoria de la lista
    private static Room RandomRoom(Vector2 direction) {
        /*
        La selección de salas dependerá de la dirección de la que venga:   v
            De izquierda a derecha: 1000 y demás con bit 1 positivo    > 0 0
            De arriba a abajo: 0100 y demás con bit 2 positivo           0 0 <
            De abajo a arriba: 0010 y demás con bit 3 positivo           ^
            De derecha a izquierda: 0001 y demás con bit 4 positivo
        */
        List<string> possibleRooms = GetRoomNamesWithBit(direction);
        return new Room(possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Count)]);
    }

    // Pinta la sala especificada en el lugar especificado
    private static void PaintRoom(ref int[,] bitmap, Vector2 position, ref Room room, Vector2 direction) {
        Debug.Log("I LOVE PAINT: " + position.x + ", " + position.y);
        bitmap[(int)position.y, (int)position.x] += 1;
        // VOY A MATARME ENCIMA DE ESTAS 4 LINEAS DE CÓDIGO
        // if (direction == Vector2.right) { position.x -= 1; } // no tocar
        if (direction == Vector2.down) { position.y -= 1; } // 
        else if (direction == Vector2.up) { position.x -= 1; } // !!!!
        else if (direction == Vector2.left) { position.y -= 1; position.x -= 1; } //!!!!!!

        for(int i = 0; i < 4; i++ ) {
                if (room.slots[i] == 1) {
                    bitmap[(int)position.y + i / 2, (int)position.x + i % 2] += 1;
            }
        }
    }

    private static bool HasAvailableSpace(int[,] bitmap, Vector2 roomPosition, int[] slots, Vector2 direction){
        int size = 0;
        foreach(int n in slots) if (n == 1) size++;
        switch (size) {
            case 1:
                // Checkea 1000, 0100, 0010, 0001
                return bitmap[(int)roomPosition.y, (int)roomPosition.x] == 0;
            case 2:
                // Checkea 1010, 0101
                if (direction.x == 0) {
                    return bitmap[(int)roomPosition.y, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y, (int)roomPosition.x  + (int)direction.x] == 0 &&
                            bitmap[(int)roomPosition.y + 1, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y - 1, (int)roomPosition.x] == 0;
                }
                // Checkea 1100 y 0011
                else {
                    return bitmap[(int)roomPosition.y, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y + (int)direction.y, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y, (int)roomPosition.x + 1] == 0 &&
                            bitmap[(int)roomPosition.y, (int)roomPosition.x - 1] == 0;
                }
            default:
                // Checkea entrada en bit 2 o 3 (0100, 0010)
                if (direction.x == 0) {

                    return bitmap[(int)roomPosition.y, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y, (int)roomPosition.x + 1] == 0 &&
                            bitmap[(int)roomPosition.y, (int)roomPosition.x - 1] == 0 &&
                            bitmap[(int)roomPosition.y + (int)direction.y, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y + (int)direction.y, (int)roomPosition.x + 1] == 0 &&
                            bitmap[(int)roomPosition.y + (int)direction.y, (int)roomPosition.x - 1] == 0;
                } else { // Checkea entrada en bit 1 o 4 (1000, 0001)
                    return bitmap[(int)roomPosition.y, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y + 1, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y - 1, (int)roomPosition.x] == 0 &&
                            bitmap[(int)roomPosition.y, (int)roomPosition.x + (int)direction.x] == 0 &&
                            bitmap[(int)roomPosition.y + 1, (int)roomPosition.x + (int)direction.x] == 0 &&
                            bitmap[(int)roomPosition.y - 1, (int)roomPosition.x + (int)direction.x] == 0;
                }
        }
    }
    private static Vector2 RandomDirection() {
        switch(UnityEngine.Random.Range(0, 4)) {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.right;
            case 2:
                return Vector2.down;
            default:
                return Vector2.left;
        }
    }
}