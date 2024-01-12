using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public static class MapUtils
{
    public static void DebugMap(Room[,] roomLayout)
    {
        string mappedLayout = "";
        int[,] generalLayout = new int[roomLayout.GetLength(0) * 2 , roomLayout.GetLength(1) * 2];

        Room currentRoom = new Room("", new int[0, 0]);
        Vector2 roomPosition = new Vector2(0, 0);
        foreach (Room r in roomLayout){
            Debug.Log("AAAA" + roomPosition.x + "," + roomPosition.y);
            roomPosition.x += 2;
            if (roomPosition.x > generalLayout.GetLength(0)) {
                roomPosition.y += 2;
                roomPosition.x = 0;
            }
            if (r != null) {
                currentRoom = r;
                break;
            }
        }

        paintRoom(ref generalLayout, roomPosition, currentRoom.slots);


        for (int x = 0; x < generalLayout.GetLength(0); x++) {
            for (int y = 0; y < generalLayout.GetLength(1); y++) {
                mappedLayout += $"{generalLayout[x, y]} ";
            }
            mappedLayout += "\n";
        }
        Debug.Log(mappedLayout);
    }

    private static void paintRoom(ref int[,] generalLayout, Vector2 roomPosition, int[,] slots) {
        Debug.Log("Mahoraga ayudame: " + roomPosition.x + "," +  roomPosition.y);
        generalLayout[(int)roomPosition.x, (int)roomPosition.y] += 2;
        if (slots[0, 1] == 1) generalLayout[(int)roomPosition.x, (int)roomPosition.y + 1] += 1;
        if (slots[1, 0] == 1) generalLayout[(int)roomPosition.x + 1, (int)roomPosition.y] += 1;
        if (slots[1, 1] == 1) generalLayout[(int)roomPosition.x + 1, (int)roomPosition.y + 1] += 1;
    }

    public static List<string> getRoomPool(string mapPath)
    {
        List<string> rooms = new List<string>(); 
        DirectoryInfo dir = new DirectoryInfo(mapPath);
        foreach (var file in dir.GetFiles("*.json")) {
            // Debug.Log(file.Name);
            rooms.Add(file.Name.Substring(0, file.Name.Length - 5));
        }
        return rooms;
    }

    // Los nombres contendrán la información de cuantos slots ocupa la sala
    public static int[,] GetRoomSlots(string floorName) {
        int[,] slots = new int[2,2];
        slots[0, 0] = int.Parse($"{floorName[floorName.Length - 9]}");
        slots[0, 1] = int.Parse($"{floorName[floorName.Length - 8]}");
        slots[1, 0] = int.Parse($"{floorName[floorName.Length - 7]}");
        slots[1, 1] = int.Parse($"{floorName[floorName.Length - 6]}");
        return slots;
    }

    // Añadir una habitación random nueva al layout
    public static void AddRoom(ref Room[,] layout, Vector2 roomPosition) {

    }
}