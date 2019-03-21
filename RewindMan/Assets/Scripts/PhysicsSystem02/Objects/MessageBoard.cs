using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace FixPhysics
{
    [RequireComponent(typeof(CollidableObject))]
    public class MessageBoard : MonoBehaviour
    {
        private CollidableObject collidable;
        public Text text;

        // Use this for initialization
        void Start()
        {
            collidable = GetComponent<CollidableObject>();
            collidable.ReactToCollide += Logger;
            collidable.ReactNotToCollide += LoggerNothing;
        }

        void Logger(Collision[] collisions)
        {
            bool player = false;
            foreach(Collision collision in collisions)
            if (collision.tag == "Player")
            {
                    player = true;
            }
            if (player)
                text.text = "You Were Here";
            else
                LoggerNothing();
        }

        void LoggerNothing()
        {
            text.text = "";
        }
    }
}
