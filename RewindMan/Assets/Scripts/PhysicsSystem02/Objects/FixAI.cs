using UnityEngine;
using UnityEngine.UI;
using FixedPointy;
using FixPhysics;
using System.Collections.Generic;

namespace FixPhysics
{
    [RequireComponent(typeof(ControllableRigidObject))]
    [RequireComponent(typeof(CollidableObject))]
    public class FixAI : RecordedObject<AIState>
    {
        private ControllableRigidObject fixObject;
        private CollidableObject collidable;
        public Animator animator;
        private GameOver gameOver;
        public int initSpeed;
        public Material enemy;
        public Material cube;
        public MeshRenderer meshRenderer;

        private void Start()
        {
            fixObject = GetComponent<ControllableRigidObject>();
            collidable = GetComponent<CollidableObject>();
            gameOver = GetComponent<GameOver>();
            collidable.ReactToCollide += Collided;
            state = new AIState(initSpeed, false);
        }

        public void DoThinking()
        {
            if (!state.dead)
                fixObject.MovePosition(new FixVec3(state.speed, 0, 0));
            if (animator != null)
            {
                animator.SetBool("Dead", state.dead);
            }
            if (state.dead)
            {
                gameOver.IsOn = false;
                if (meshRenderer != null)
                {
                    meshRenderer.material = cube;
                }
            } else
            {
                gameOver.IsOn = true;
                if (meshRenderer != null)
                {
                    meshRenderer.material = enemy;
                }
            }
        }

        private void Collided(Collision[] collisions)
        {
            foreach(Collision collision in collisions)
            {
                if (collision.tag == "Player")
                {
                    if (collision.Normal.Y < 0)
                    {
                        state.dead = true;
                    }
                    else continue;
                }
                if (FixMath.Abs(collision.Normal.X) > 0)
                    state.speed *= -1;
            }
        }
    }
}