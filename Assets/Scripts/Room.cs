using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

//public enum Position { N, S, W, E };
public class Room : MonoBehaviour
{
    public Transform N, S, W, E;//N=0, S=1, W=2, E=3
    public byte[] openings = new byte[4];//NSWE
    [SerializeField] private NavMeshSurface _surface;
    public Room[] Neighbour = new Room[4];
    public bool CanChangeDistance;
    public Vector3 Posizione { get; set; }

    private int dinstanceFromPlayer;
    public int DinstaceFromPlayer
    {
        get { return dinstanceFromPlayer; }
        set
        {
            if (CanChangeDistance)
            {
                dinstanceFromPlayer = value;
                CanChangeDistance = false;
            }
        }
    }

    public UnityAction AdaptiveRandom;
    public UnityAction ChangeDinstace;
    public UnityAction<int> aaa;

    private void GetWallOpen(Transform t, byte on)
    {
        if (on == 1) //1 == door
        {//t0: door t1: wall
            t.GetChild(1).gameObject.SetActive(false);
            t.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            t.GetChild(0).gameObject.SetActive(false);
            t.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void Init()
    {
        GetWallOpen(N, openings[0]);
        GetWallOpen(S, openings[1]);
        GetWallOpen(W, openings[2]);
        GetWallOpen(E, openings[3]);
        //_surface.BuildNavMesh();
    }

    public static int GetOpposite(int from)
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

    public int DoorCount()
    {
        int num = 0;
        for (int i = 0; i < openings.Length; i++)        
            if (openings[i] == 1)
                num++;
        
        return num;
    }

    public void GenerateWall(int from = -1)
    {
        int minimo = 2;
        if (from == -1)
            minimo = 3;
        do
        {
            for (int i = 0; i < 4; i++)
            {
                //N=0, S=1, W=2, E=3
                if (i != Room.GetOpposite(from))
                    openings[i] = (byte)(Random.Range(0, 10) < 6 ? 1 : 0);
            }
        } while (DoorCount() < minimo);
    }

    public void Delete()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Neighbour[i] != null)
                Neighbour[i].Neighbour[GetOpposite(i)] = null;

        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GameController"))
        {
            if (dinstanceFromPlayer != 0)
            {
                Debug.Log("Chiamato");
                ChangeDinstace();
                dinstanceFromPlayer = 0;
                CanChangeDistance = false;

                for (int i = 0; i < Neighbour.Length; i++)
                {
                    if (Neighbour[i] != null && Neighbour[i].CanChangeDistance) //se il vicino c'è ed è modificabile
                    {
                        Neighbour[i].DinstaceFromPlayer = 1; //cambio la distanza del mio vicino
                        for (int l = 0; l < Neighbour[i].Neighbour.Length; l++)
                        {
                            if (Neighbour[i].Neighbour[l] != null && Neighbour[i].Neighbour[l].CanChangeDistance)
                            { //se il vicino del mio vicino c'è ed è modificabile
                                Neighbour[i].Neighbour[l].DinstaceFromPlayer = 2; //cambio la distanza del vicino del mio vicino
                                for (int j = 0; j < Neighbour[i].Neighbour[l].Neighbour.Length; j++)
                                {
                                    if (Neighbour[i].Neighbour[l].Neighbour[j] != null && Neighbour[i].Neighbour[l].Neighbour[j].CanChangeDistance) //se il vicino del vicino del mio vicino c'è ed è modificabile
                                        Neighbour[i].Neighbour[l].Neighbour[j].DinstaceFromPlayer = 3; //cambio la distanza del vicino del vicino del mio vicino
                                }
                            }
                        }
                    }
                }
                //name = "Room " + dinstanceFromPlayer;
                AdaptiveRandom();
            }
        }
        
    }
}

