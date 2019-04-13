using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

namespace FixPhysics
{
    public class InputRecording
    {
        List<Record<InputState>> recordings = new List<Record<InputState>>();

        public void Clear()
        {
            recordings.Clear();
        }

        public void AddState(InputState state)
        {
            if (recordings.Count > 0)
            {
                Record<InputState> last = recordings[recordings.Count - 1];
                if ((last.state.left != state.left) || (last.state.right != state.right) || (last.state.up != state.up))
                {
                    Record<InputState> newState = new Record<InputState>(state, FixWorld.time);
                    recordings.Add(newState);
                }
            }
            else
            {
                Record<InputState> newState = new Record<InputState>(state, FixWorld.time);
                recordings.Add(newState);
            }
        }

        public InputState GetState(Fix time)
        {
            Record<InputState> max = new Record<InputState>(new InputState(), 0);
            foreach (Record<InputState> current in recordings)
            {
                if (current.time > max.time && current.time <= time)
                {
                    max = current;
                }
            }
            return (InputState)max.state.Clone();
        }

        public void ClearFrom(Fix time)
        {
            List<Record<InputState>> okayRecordings = new List<Record<InputState>>();
            foreach (Record<InputState> rec in recordings)
            {
                if (rec.time <= time)
                {
                    okayRecordings.Add(rec);
                }
            }
            recordings = okayRecordings;
        }
    }
}
