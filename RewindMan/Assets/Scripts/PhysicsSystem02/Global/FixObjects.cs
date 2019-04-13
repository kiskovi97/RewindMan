using UnityEngine;
using System.Collections.Generic;
using FixedPointy;
using System.Linq;

namespace FixPhysics
{
    public class FixObjects : MonoBehaviour
    {
        private FixCollider[] colliders;
        private FixPlayer fixPlayer;
        private MovingObject[] objects;
        private IRecordedObject[] recordedObjects;
        private CollidableObject[] collidables;

        // Use this for initialization
        void Start()
        {
            objects = FindObjectsOfType<MovingObject>();
            collidables = FindObjectsOfType<CollidableObject>();
            colliders = FindObjectsOfType<FixCollider>();
            fixPlayer = FindObjectOfType<FixPlayer>();
            var ss = FindObjectsOfType<MonoBehaviour>().OfType<IRecordedObject>();
            List<IRecordedObject> list = new List<IRecordedObject>();
            foreach (IRecordedObject s in ss)
            {
                list.Add(s);
            }
            recordedObjects = list.ToArray();
        }

        public bool CahceIsEmpty()
        {
            int min = -1;
            for (int i = 0; i < objects.Length; i++)
            {
                int actual = objects[i].CacheSize();
                if ((actual >= 0) && (actual < min || min < 0)) min = actual;
            }
            return min == 0;
        }

        public void SetFromCache()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetFromCache();
            }
        }

        public void CacheClear()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].CacheClear();
            }
        }

        public void Step(InputState state)
        {
            fixPlayer.KeyCheck(state);
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].Move();
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
        }

        public void RecordToCache(Fix time)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].RecordToCache(time);
            }
        }

        public void SetState(Fix time)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetLast(time);
            }
        }
    }
}
