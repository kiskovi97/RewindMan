using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

namespace FixPhysics
{
    public class FixObjects : MonoBehaviour
    {
        private FixCollider[] colliders;
        private FixPlayer fixPlayer;
        private RigidObject[] objects;
        private CollidableObject[] collidables;
        private MovingPlatform[] movingObjects;

        // Use this for initialization
        void Start()
        {
            objects = FindObjectsOfType<RigidObject>();
            collidables = FindObjectsOfType<CollidableObject>();
            movingObjects = FindObjectsOfType<MovingPlatform>();
            colliders = FindObjectsOfType<FixCollider>();
            fixPlayer = FindObjectOfType<FixPlayer>();
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
            foreach (MovingPlatform moving in movingObjects)
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
            foreach (MovingPlatform moving in movingObjects)
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
            foreach (MovingPlatform moving in movingObjects)
            {
                moving.Move();
            }
            for (int i = 0; i < collidables.Length; i++)
            {
                List<Collision> collisions = new List<Collision>();
                for (int j = colliders.Length - 1; j >= 0; j--)
                {
                    Collision collision = collidables[i].GetCollision(colliders[j]);
                    if (collision != null)
                        collisions.Add(collision);
                }
                collidables[i].Collide(collisions.ToArray());
            }
        }

        public void Record()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].Record();
            }
            foreach (MovingPlatform moving in movingObjects)
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
            foreach (MovingPlatform moving in movingObjects)
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
            foreach (MovingPlatform moving in movingObjects)
            {
                moving.SetLast();
            }
        }
    }
}
