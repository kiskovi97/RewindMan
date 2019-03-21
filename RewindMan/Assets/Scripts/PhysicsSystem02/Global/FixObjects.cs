using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

public class FixObjects : MonoBehaviour
{
    private FixCollider[] colliders;
    private FixPlayerOther fixPlayer;
    private RigidObjectOther[] objects;
    private MovingPlatformOther[] movingObjects;

    // Use this for initialization
    void Start()
    {
        objects = FindObjectsOfType<RigidObjectOther>();
        movingObjects = FindObjectsOfType<MovingPlatformOther>();
        colliders = FindObjectsOfType<FixCollider>();
        fixPlayer = FindObjectOfType<FixPlayerOther>();
    }

    public bool CahceIsEmpty()
    {
        return objects[0].CacheSize() == 0;
    }

    public void SetFromCache()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetFromCache();
        }
        foreach (MovingPlatformOther moving in movingObjects)
        {
            moving.SetFromCache();
        }
    }

    public void CacheClear()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].CacheClear();
        }
        foreach (MovingPlatformOther moving in movingObjects)
        {
            moving.CacheClear();
        }
    }

    public void Step(InputRecord state)
    {
        fixPlayer.KeyCheck(state);
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Move();
        }
        foreach (MovingPlatformOther moving in movingObjects)
        {
            moving.Move();
        }
        for (int i = 0; i < objects.Length; i++)
        {
            List<Collision> collisions = new List<Collision>();
            for (int j = colliders.Length - 1; j >= 0; j--)
            {
                Collision collision = objects[i].GetCollision(colliders[j]);
                if (collision != null)
                    collisions.Add(collision);
            }
            objects[i].Collide(collisions.ToArray());
        }
    }

    public void Record()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Record();
        }
        foreach (MovingPlatformOther moving in movingObjects)
        {
            moving.Record();
        }
    }

    public void RecordToCache(Fix time)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].RecordToCache(time);
        }
        foreach (MovingPlatformOther moving in movingObjects)
        {
            moving.RecordToCache(time);
        }
    }

    public void SetState()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetLast();
        }
        foreach (MovingPlatformOther moving in movingObjects)
        {
            moving.SetLast();
        }
    }
}
