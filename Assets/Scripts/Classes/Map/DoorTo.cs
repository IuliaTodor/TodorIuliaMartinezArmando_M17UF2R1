using UnityEngine;

public class DoorTo : MonoBehaviour {

    Door[] doors;
    string[] doorNames = new string[] {"1", "2", "3", "4"}; // solo pa debugar
    Vector2[] doorPositions;
    Room currentRoom;

    MapManager mapManager;

    void Start() {
        mapManager = FindObjectOfType<MapManager>();
        currentRoom = new Room("base1110.json");

        doorPositions = new Vector2[] {new Vector2(-7, 2), new Vector2(0, 3), new Vector2(7, 2), new Vector2(0, -7)};
    }
    void OnTriggerEnter2D(Collider2D col) {

        Debug.Log("ME HA TOCADO EL GORDO" + col.gameObject.transform.position);
        Debug.Log(closestDoor(col.gameObject.transform.position));
        // mapManager.LoadRoom(currentRoom);
    }
    
    private string closestDoor(Vector2 playerPosition) {
        float minDistance = Vector2.Distance(playerPosition, doorPositions[0]);
        int minIndex = 0;
        for (int i = 1; i < doorNames.Length; i++) {
            if (Vector2.Distance(playerPosition, doorPositions[i]) < minDistance) {
                minIndex = i;
                minDistance = Vector2.Distance(playerPosition, doorPositions[i]);
            }
        }

        return doorNames[minIndex];
    } 
}
