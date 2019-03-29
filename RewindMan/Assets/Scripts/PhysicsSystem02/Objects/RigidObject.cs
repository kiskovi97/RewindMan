using UnityEngine;
using UnityEditor;
using FixedPointy;
using System.Collections.Generic;

namespace FixPhysics
{
    [RequireComponent(typeof(CollidableObject))]
    public class RigidObject : RecordedObject
    {
        // Inspector Initial values
        public float frictionCoefficientFloat = 0.98f;
        public float impulseLoseCoefficentFloat = 0.5f;

        private static readonly int collideOverlap = 1;
        private static Fix minCollide = new Fix(4);
        private Fix frictionCoefficient;
        private Fix impulseLoseCoefficent;
        private FixVec3 savedVelocity = FixVec3.Zero;
        private Forces forces = new Forces();
        private CollidableObject collidable;

        protected override void Start()
        {
            base.Start();
            ResetRecording();
            state = RigidRecord.RecordFromBase(state, 0, FixVec3.Zero);
            frictionCoefficient = FixConverter.ToFix(frictionCoefficientFloat);
            impulseLoseCoefficent = FixConverter.ToFix(impulseLoseCoefficentFloat);
            forces = new Forces();
            forces.Clear();
            forces.AddForce(FixWorld.gravity);
            minCollide = FixMath.Abs(FixWorld.gravity.Y) * FixWorld.deltaTime * FixWorld.deltaTime;
            collidable = GetComponent<CollidableObject>();
            collidable.SetPositionAndVelocity(state.position, state.velocity);
            collidable.ReactToCollide += ReactToCollide;
        }

        // ---------------- FixObject Implementations -----------------

        public override void Move()
        {
            Accelerate(forces.GetSumForces());
            Step();
            Step(((RigidRecord)state).prevVelocity);
            ((RigidRecord)state).prevVelocity = FixVec3.Zero;
            collidable.SetPositionAndVelocity(state.position, state.velocity);
            forces.Clear();
            SetOnTheFloor(false);
        }

        public bool OnTheFloor()
        {
            return ((RigidRecord)state).onTheFloor > 0;
        }

        // --------------- Inner Help Functions  -------------

        protected void SetOnTheFloor(bool collided)
        {
            if (collided)
            {
                ((RigidRecord)state).onTheFloor = collideOverlap;
            }
            else
            {
                RigidRecord fullState = ((RigidRecord)state);
                fullState.onTheFloor--;
                if (fullState.onTheFloor < 0) fullState.onTheFloor = 0;
                state = fullState;
            }
        }

        void ReactToCollide(Collision[] collisions)
        {
            for (int i = 0; i < collisions.Length; i++)
            {
                if (!collisions[i].isStatic)
                {
                    ReactDynamicCollide(collisions[i]);
                }
                OverlapCorrection(collisions[i]);
                if (collisions[i].Normal.Y > 0)
                {
                    SetOnTheFloor(true);
                }
            }
            for (int i = 0; i < collisions.Length; i++)
            {
                if (collisions[i].isStatic)
                {
                    ReactStaticCollide(collisions[i]);
                }
            }
            startVelocity = FixConverter.ToFixVec3(state.velocity);
        }

        private void ReactStaticCollide(Collision collision)
        {
            FixVec3 N = collision.Normal;
            FixVec3 paralellVector = new FixVec3(-N.Y, N.X, N.Z);
            FixVec3 projectedForce = HelpFixMath.Project(state.velocity, paralellVector);
            VelocityCorrection(projectedForce * frictionCoefficient);

            ((RigidRecord)state).prevVelocity += collision.savedVelocity;
        }

        private void ReactDynamicCollide(Collision collision)
        {
            FixVec3 N = collision.Normal;
            // Direction
            FixVec3 velocity = state.velocity + FixMath.Abs(2 * state.velocity.Dot(N)) * N;
            // Impulse
            velocity = ((velocity + collision.savedVelocity) / 2) * impulseLoseCoefficent;

            VelocityCorrection(velocity);
        }

        private void OverlapCorrection(Collision collision)
        {
            FixVec3 Something = collision.Overlap;
            Fix length = Something.GetMagnitude();

            //if (length == 0) collision.Overlap = collision.Normal;
            if (length > minCollide)
            {
                length -= minCollide;
            }
            else
            {
                length = 0;
            }
            FixVec3 Correction = Something.Normalize() * length;
            if (collision.isStatic)
            {
                if (state.position.Y >= collision.position.Y)
                {
                    PositionCorrection(state.position + Correction);
                }
                else
                {
                    PositionCorrection(state.position + collision.Overlap);
                }
            }
            else
            {
                if (state.position.Y >= collision.position.Y)
                {
                    PositionCorrection(state.position + Something - Something.Normalize() * minCollide);
                }
            }
        }
    }
}
