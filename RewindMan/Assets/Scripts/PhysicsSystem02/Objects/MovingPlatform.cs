using UnityEngine;
using System.Collections;
using FixedPointy;
namespace FixPhysics
{
    [RequireComponent(typeof(FixCollider))]
    public class MovingPlatform : MovingObject
    {
        public Vector3 startDistance = new Vector3(0, 1, 0);
        public float startSpeed = 2.0f;
        private Fix speed;
        private FixVec3 start;
        private FixVec3 distance;
        private FixCollider fixCollider;

        protected override void Start()
        {
            base.Start();
            speed = FixConverter.ToFix(startSpeed);
            start = state.position;
            distance = FixConverter.ToFixVec3(startDistance);
            state = MovingAngleState.RecordFromBase(state, 0);
            fixCollider = GetComponent<FixCollider>();
            fixCollider.isStatic = true;
        }

        public override void Move()
        {
            FixVec3 position = state.position;
            Increment(FixWorld.deltaTime * speed);
            FixVec3 position2 = GetPosition();
            state.velocity = (position2 - position) * (1 / FixWorld.deltaTime);
            state.position = position2;
            fixCollider.SetPositionAndVelocity(state.position, state.velocity);
        }

        private FixVec3 GetPosition()
        {
            return start + distance * (FixMath.Cos(GetAngle() * 100) * -1 + 1);
        }
    

        private void Increment(Fix inc)
        {
            ((MovingAngleState)state).angle += inc;
        }

        private Fix GetAngle()
        {
            return ((MovingAngleState)state).angle;
        }
    }
}
