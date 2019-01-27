using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    public Transform N, S, W, E;
    public byte[] openings = new byte[4];//NSWE
    [SerializeField] private NavMeshSurface _surface;
    public List<Room> neighboors = new List<Room>();

    public Action Enter;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GameController"))
            Enter();
    }
}
