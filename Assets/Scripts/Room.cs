using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    public Transform N, S, W, E;
    public byte[] openings = new byte[4];//NSWE
    [SerializeField] private NavMeshSurface _surface;

    private void GetWallOpen(Transform t, byte on)
    {
        if (on == 1) t.GetChild(1).gameObject.SetActive(false); 
        else t.GetChild(0).gameObject.SetActive(false); 
    }

    public void Init()
    {
        GetWallOpen(N, openings[0]);
        GetWallOpen(S, openings[1]);
        GetWallOpen(W, openings[2]);
        GetWallOpen(E, openings[3]);
        _surface.BuildNavMesh();
    }
}
