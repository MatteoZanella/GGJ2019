using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator instance;
    public int genDistance = 2;
    public GameObject RoomPrefab;
    public Vector2 gridSize;
    public Transform player;

    void Awake()
    {
        instance = this;
        Generate(deep:genDistance);
    }

    int GetOpposite(int from)
    {
        switch (from)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 2;
            default: return -1;
        }
    }

    private int maxCalls = 50;
    private int calls = 0;
    void Generate(Vector3 pos = new Vector3(), int deep = 0, int from = -1, Room origin = null)
    {
        if (calls > maxCalls)
            return;
        calls++;
//        Debug.Break();
        if (deep <= 0) return;
        GameObject roomObj = Instantiate(RoomPrefab, pos, Quaternion.identity);
        roomObj.name = "Room" + deep;
        Room room = roomObj.GetComponent<Room>();
        for (int i = 0; i < 4; i++)
        {
            room.openings[i] = (byte) (Random.Range(0, 10) < 6 ? 1 : 0);
            if (GetOpposite(from) != i && room.openings[i] == 1 && deep > 1)
                Generate( //N=0, S=1, W=2, E=3
                    pos + new Vector3((i < 2 ? 0 : gridSize.x * (i == 2 ? -1 : 1)), 0,
                        (i < 2 ? gridSize.y * (i == 0 ? 1 : -1) : 0)), deep - 1, i, room);
        }

        if (from != -1)
        {
            room.openings[GetOpposite(from)] = 1;
            origin.neighboors.Add(room);
        }

        room.Enter = delegate
        {
            Debug.Log("Entered in room " + room.name);
            if (deep == 1)
            {
                deep++;
                calls = 0;
                Debug.Log("Should generate");
                for (int i = 0; i < 4; i++)
                {
                    if (GetOpposite(from) != i && room.openings[i] == 1)
                        Generate( //N=0, S=1, W=2, E=3
                            pos + new Vector3((i < 2 ? 0 : gridSize.x * (i == 2 ? -1 : 1)), 0,
                                (i < 2 ? gridSize.y * (i == 0 ? 1 : -1) : 0)), 1, i, room);
                }
            }
        };
        room.Init();
    }
}