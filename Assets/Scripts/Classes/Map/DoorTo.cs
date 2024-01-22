using System.Collections.Generic;
using UnityEngine;

public class DoorTo : MonoBehaviour {

    public List<Door> doors = new List<Door>();
    public List<Vector2> doorPositions = new List<Vector2>();
    MapManager mapManager;

    void Start() {
        mapManager = FindObjectOfType<MapManager>();
    }
    void OnTriggerEnter2D(Collider2D col) {

        mapManager.LoadRoom(mapManager.roomLayout[closestDoor(col.gameObject.transform.position).goToIndex]);
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
        Debug.Log(minIndex);
        Debug.Log(doors[minIndex].doorPosition);
        Debug.Log(doors[minIndex].goToIndex);
        return doors[minIndex];
    } 
}
