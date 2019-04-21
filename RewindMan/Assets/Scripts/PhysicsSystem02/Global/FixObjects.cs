using UnityEngine;
using System.Collections.Generic;
using FixedPointy;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

namespace FixPhysics
{
    public class FixObjects : MonoBehaviour
    {
        public bool DebugText = false;
        public TextMeshProUGUI debugText;
        private FixCollider[] colliders;
        private FixPlayer fixPlayer;
        private FixAI[] fixAIs;
        private MovingObject[] objects;
        private IRecordedObject[] recordedObjects;
        private CollidableObject[] collidables;

        void TextUpdate()
        {
            if (debugText != null)
            {
                int cache = CacheSize();
                int recordNumber = fixPlayer.getRecordNumber();
                int objectCount = recordedObjects.Length;
                debugText.text = SceneManager.GetActiveScene().name + "\n" +
                "Recorded Objects: " + objectCount + "\n" +
                "Colliders " + colliders.Length + "\n" +
                "PreRecords / object: " + recordNumber + "\n" +
                "Cache records / object: " + cache + "\n" +
                "Cache + Pre Records / object: " + (recordNumber + cache) + "\n" +
                "PreRecords: " + (objectCount * recordNumber) + "\n" +
                "Cache records: " + (objectCount * cache) + "\n" +
                "Cache + Pre Records: " + (objectCount * (recordNumber + cache)) + "\n";
            }
        }

        // Use this for initialization
        void Start()
        {
            objects = FindObjectsOfType<MovingObject>();
            collidables = FindObjectsOfType<CollidableObject>();
            colliders = FindObjectsOfType<FixCollider>();
            fixPlayer = FindObjectOfType<FixPlayer>();
            fixAIs = FindObjectsOfType<FixAI>();
            var ss = FindObjectsOfType<MonoBehaviour>().OfType<IRecordedObject>();
            List<IRecordedObject> list = new List<IRecordedObject>();
            foreach (IRecordedObject s in ss)
            {
                list.Add(s);
            }
            recordedObjects = list.ToArray();
        }

        private void Update()
        {
            TextUpdate();
        }

        public bool CahceIsEmpty()
        {
            int min = -1;
            for (int i = 0; i < recordedObjects.Length; i++)
            {
                int actual = recordedObjects[i].CacheSize();
                if ((actual >= 0) && (actual < min || min < 0)) min = actual;
            }
            return min == 0;
        }

        private int CacheSize()
        {
            if (recordedObjects == null) return 0;
            int min = -1;
            for (int i = 0; i < recordedObjects.Length; i++)
            {
                int actual = recordedObjects[i].CacheSize();
                if ((actual >= 0) && (actual < min || min < 0)) min = actual;
            }
            return min;
        }

        public void SetFromCache()
        {
            for (int i = 0; i < recordedObjects.Length; i++)
            {
                recordedObjects[i].SetFromCache();
            }
        }

        public void CacheClear()
        {
            for (int i = 0; i < recordedObjects.Length; i++)
            {
                recordedObjects[i].CacheClear();
            }
        }

        public void Step(InputState state)
        {
            fixPlayer.KeyCheck(state);
            for (int i = 0; i < fixAIs.Length; i++)
            {
                fixAIs[i].DoThinking();
            }
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].Move();
            }
                AsyncCollisionDetection();
        }

        void SyncCollisionDetection()
        {
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

        void AsyncCollisionDetection()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < collidables.Length; i++)
            {
                CollidableObject collidable = collidables[i];
                Task task = Task.Factory.StartNew(() => CollisionDetection(collidable));
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }

        void CollisionDetection(CollidableObject collidable)
        {
            List<Collision> collisions = new List<Collision>();
            for (int j = colliders.Length - 1; j >= 0; j--)
            {
                Collision collision = collidable.GetCollision(colliders[j]);
                if (collision != null)
                    collisions.Add(collision);
            }
            collidable.Collide(collisions.ToArray());
        }

        public void Record()
        {
            for (int i = 0; i < recordedObjects.Length; i++)
            {
                recordedObjects[i].Record();
            }
        }

        public void RecordToCache(Fix time)
        {
            for (int i = 0; i < recordedObjects.Length; i++)
            {
                recordedObjects[i].RecordToCache(time);
            }
        }

        public void SetState(Fix time)
        {
            for (int i = 0; i < recordedObjects.Length; i++)
            {
                recordedObjects[i].SetLast(time);
            }
        }
    }
}
