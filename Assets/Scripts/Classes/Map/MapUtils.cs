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
        int[,] generalLayout = new int[roomLayout.GetLength(0) * 4, roomLayout.GetLength(1) * 4];

        Room currentRoom = new Room("", new int[0, 0]);
        for (int i = 0; i < roomLayout.GetLength(0); i++) {
            for (int j = 0; j < roomLayout.GetLength(1); j++) {

                if (roomLayout[i, j] != null) {
                    currentRoom = roomLayout[i, j];
                    generalLayout[i * 4, j * 4] += 1;
                }
            }
        }

        for (int x = 0; x < generalLayout.GetLength(0); x++) {
            for (int y = 0; y < generalLayout.GetLength(1); y++) {
                mappedLayout += $"{generalLayout[x, y]} ";
            }
            mappedLayout += "\n";
        }
        Debug.Log(mappedLayout);
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
        Debug.Log($"{slots[0, 0]}, {slots[0, 1]}\n {slots[1, 0]}, {slots[1, 1]}");
        return slots;
    }

    // Añadir una habitación random nueva al layout
    public static void AddRoom(ref Room[,] layout, int[,] roomPosition) {

    }
}