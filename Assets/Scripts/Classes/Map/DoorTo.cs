using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorTo : MonoBehaviour {

    public List<Door> doors = new List<Door>();
    public List<Vector2> doorPositions = new List<Vector2>();
    MapManager mapManager;

    void Start() {
        mapManager = FindObjectOfType<MapManager>();
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {

            Door entryDoor = closestDoor(col.gameObject.transform.position);
            Room nextRoom = mapManager.roomLayout[entryDoor.goToIndex];
            Door exitDoor = entryDoor;



            Debug.Log("Go to Room: " + nextRoom.roomName + 
                    "\nwith index: " + entryDoor.goToIndex +
                    "\ngoing to: " + entryDoor.doorPosition);
            foreach (Door d in nextRoom.doors) {
                if ((Vector2)d.doorPosition == (Vector2)entryDoor.doorPosition * -1) { // Aquí busca la puerta opuesta, debería comprobar que goToIndex de la otra puerta es la sala actual pero son la 1AM
                    exitDoor = d;
                }
            }
            
            int size = 0;
            foreach(int n in nextRoom.slots) if (n == 1) size++;
            
            if (nextRoom.slots[0] == 0 && size == 1) {
                for (int i = 1; i < nextRoom.slots.Length; i++) if (nextRoom.slots[i] == 1) exitDoor.doorPosition.z -= i;
            }

            if (nextRoom.slots[0] == 0 && nextRoom.slots[2] == 0 && size == 2) exitDoor.doorPosition.z -= 1;
            else if (nextRoom.slots[0] == 0 && nextRoom.slots[1] == 0 && size == 2) exitDoor.doorPosition.z -= 2;

            Vector2 newPos = mapManager.possibleDoorPositions[(int)exitDoor.doorPosition.z, (Vector2)exitDoor.doorPosition == Vector2.right ? 0 :
                                                                                            (Vector2)exitDoor.doorPosition == Vector2.down ? 1 :
                                                                                            (Vector2)exitDoor.doorPosition == Vector2.up ? 2 : 3];
            newPos.x += entryDoor.doorPosition.x * 2;
            newPos.y -= entryDoor.doorPosition.y * 2;

            col.gameObject.transform.position = new Vector3(newPos.x, newPos.y, 0);

            mapManager.LoadRoom(nextRoom);
        }
    }
    
    private Door closestDoor(Vector2 playerPosition) {
        float minDistance = Vector2.Distance(playerPosition, doorPositions[0]);
        int minIndex = 0;
        for (int i = 1; i < doors.Count; i++) {
                if (Vector2.Distance(playerPosition, doorPositions[i]) < minDistance) {
                    minIndex = i;
                    minDistance = Vector2.Distance(playerPosition, doorPositions[i]);
                }
            
        }
        return doors[minIndex];
    } 
}
