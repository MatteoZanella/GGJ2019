using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static WorldGenerator instance;
    public int genDistance = 2;
    public GameObject RoomPrefab;
    public Vector2 gridSize;
    public Transform player;
    public List<Room> rooms;
    private int numero = 0;

    void Awake()
    {
        instance = this;
        rooms = new List<Room>();
        Generate();
    }

    void Generate(Vector3 pos = new Vector3(), int from = -1, Room origin = null)
    {
        Debug.Log("genero " +numero);
        GameObject roomObj = Instantiate(RoomPrefab, pos, Quaternion.identity);
        roomObj.name = "Room " + numero;
        numero++;
        Room room = roomObj.GetComponent<Room>();
        rooms.Add(room);//aggiungo la room alla lista di tutte le room
        room.Posizione = pos; //salvo la posizione della room dentro alla stessa
        if (from != -1) //se la room è stata generata da un'altra; quindi a rigor di logica non è la prima
        {
            room.openings[Room.GetOpposite(from)] = (byte) 1; //la porta comunicante dell'origine deve essere accoppiata con una porta nella stanza adiacente
            origin.Neighbour[from] = room; //l'orgine mi ha come vicino nella sua porta comunicante con me
            room.Neighbour[Room.GetOpposite(from)] = origin; // io ho come vicino l'origine nella porta accoppiata
            room.CanChangeDistance = true; //cambio la distanza
            room.DinstaceFromPlayer = origin.DinstaceFromPlayer + 1; //la distanza dal player è data dalla distanza che ha l'origine + 1
            if (room.DinstaceFromPlayer == 2) //se la  la distanza dal player è arrivata a 2 vuol dire che non ne devo generare altre stanze
            {
                room.Init();//termina generando la struttura della stanza
                return;
            }
            room.GenerateWall(from);
        }
        else
        { // questa stanza è la stanza radice, la prima generata
            room.CanChangeDistance = true;
            room.DinstaceFromPlayer = 0;
            room.GenerateWall();
        }
        for (int i = 0; i < 4; i++)//N=0, S=1, W=2, E=3
        { 
            if (Room.GetOpposite(from) != i && room.Neighbour[i] == null && room.openings[i] == 1)
                Generate(pos + new Vector3((i < 2 ? 0 : gridSize.x * (i == 2 ? -1 : 1)), 0,
                        (i < 2 ? gridSize.y * (i == 0 ? 1 : -1) : 0)), i, room);
        }
        room.Init();

        room.AdaptiveRandom = delegate
        {
            List<Room> iterator = new List<Room>();
            iterator.AddRange(rooms);
            foreach (Room item in iterator)
            {
                if (item.DinstaceFromPlayer > 2)
                {
                    rooms.Remove(item);
                    item.Delete();
                }
                else if (item.DinstaceFromPlayer == 2)
                {
                    int nearest = 0;
                    Room r = new Room();
                    r.CanChangeDistance = true;
                    r.DinstaceFromPlayer = 5;
                    for (int i = 1; i < 4; i++)
                    {                        
                        if (item.Neighbour[i] != null && item.Neighbour[i].DinstaceFromPlayer < ((item.Neighbour[nearest] == null) ? r.DinstaceFromPlayer: item.Neighbour[nearest].DinstaceFromPlayer))
                            nearest = i;
                    }
                    item.GenerateWall(nearest);
                    item.Init();
                }
                else if (item.DinstaceFromPlayer == 1)
                {
                    for (int i = 0; i < item.Neighbour.Length; i++)
                    {
                        if (item.openings[i] == 1 && item.Neighbour[i] == null)
                            Generate(item.Posizione, i, item);
                    }
                }
            }
        };
        room.ChangeDinstace = delegate
        {
            foreach (Room item in rooms)
            {
                if (item == null)
                {

                }
                item.CanChangeDistance = true;
            }
        };

        room.aaa = delegate (int a)
        {

        };
    }
}