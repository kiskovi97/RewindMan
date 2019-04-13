using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

namespace FixPhysics
{
    [RequireComponent(typeof(FixObjects))]
    public class FixWorld : MonoBehaviour
    {
        // PhysicalObjects And Or Forces need it
        public static FixVec3 gravity = FixVec3.Zero;
        public static Fix time = Fix.Zero;
        public static Fix deltaTime;
        public string timeOut = "";
        public GameObject musicObject;
        public TimeRecording timeRecording;
        private FixObjects fixObjects;
        private InputRecording stateRecordings = new InputRecording();
        private InputState state = new InputState();
        private MusicClass music;

        public static bool GameOver
        {
            get; private set;
        }

        public static bool Forward
        {
            get; private set;
        }

        public static bool Backward
        {
            get; private set;
        }

        public static void GameOverSet()
        {
            GameOver = true;
        }

        public static FixVec3 GravitySizeVector(FixVec3 vector)
        {
            return vector.Normalize() * gravity.GetMagnitude();
        }

        public void Awake()
        {
            gravity = FixConverter.ToFixVec3(Physics.gravity);
            deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
            time = Fix.Zero;
            GameObject obj  = GameObject.FindGameObjectWithTag("Music");
            if (obj == null)
            {
                obj = Instantiate(musicObject);
            }
            music = obj.GetComponent<MusicClass>();
        }

        public void Start()
        {
            GameOver = false;
            Forward = true;
            Backward = false;
            fixObjects = GetComponent<FixObjects>();
        }

        private bool firstTime = true;

        private volatile bool simulate = false;

        private void FixedUpdate()
        {
            timeOut = time + "";
            if (simulate) return;
            simulate = true;

            if (firstTime)
            {
                fixObjects.Record();
                timeRecording.Push(time);
                firstTime = false;
            }

            ReverseCheck();
            if (Forward && !GameOver)
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
                time += deltaTime;
                SimulateForward();
            }
            else if (Backward)
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayReverse();
                GameOver = false;
                SimulateBackward();
                time -= deltaTime;
            } else
            {
                GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
            }

            simulate = false;
        }

        private void ReverseCheck()
        {
            if (0 != Input.GetAxisRaw("Reverse"))
            {
                Forward = false;
                if (time > Fix.Zero) Backward = true;
                else Backward = false;
            }
            else
            {
                if (Backward)
                {
                    thisFrame = perFrame - 1;
                }
                Forward = true;
                Backward = false;
            }
        }

        private volatile int thisFrame = 0;
        private volatile int perFrame = 60;

        private void SimulateForward()
        {
            fixObjects.CacheClear();
            InputToState();
            fixObjects.Step(state);
            thisFrame++;
            if (perFrame <= thisFrame)
            {
                fixObjects.Record();
                timeRecording.Push(time);
                thisFrame = 0;
            }
        }

        private void SimulateBackward()
        {
            if (fixObjects.CahceIsEmpty())
            {
                ReSimulateFromPoint();
            }
            InputToState(time);
            fixObjects.SetFromCache();
            stateRecordings.ClearFrom(time);
            thisFrame--;
            if (thisFrame < 0)
            {
                thisFrame = perFrame - 1;
            }
        }

        private void ReSimulateFromPoint()
        {
            Fix fromTime = timeRecording.GetByTime(time);
            fixObjects.SetState(fromTime);
            Fix to = time;
            for (time = fromTime; time < to; time += deltaTime)
            {
                fixObjects.RecordToCache(to - time);
                InputToState(time + deltaTime);
                fixObjects.Step(state);
            }
        }

        private void InputToState()
        {
            
            if (0 < Input.GetAxis("Horizontal")) state.right = true;
            else state.right = false;

            if (0 > Input.GetAxis("Horizontal")) state.left = true;
            else state.left = false;

            if (0 != Input.GetAxisRaw("Jump")) state.up = true;
            else state.up = false;

            stateRecordings.AddState(state);
        }

        private void InputToState(Fix fromTime)
        {
            state = stateRecordings.GetState(fromTime);
        }
    }
}
