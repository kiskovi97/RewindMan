using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

namespace FixPhysics
{
    public class InputRecording
    {
        List<InputRecord> recordings = new List<InputRecord>();

        public void Clear()
        {
            recordings.Clear();
        }

        public void AddState(InputRecord record)
        {
            InputRecord newState = record.Copy();
            if (recordings.Count > 0)
            {
                InputRecord last = recordings[recordings.Count - 1];
                if ((last.left != record.left) || (last.right != record.right) || (last.up != record.up))
                {
                    recordings.Add(newState);
                }
            }
            else
            {
                recordings.Add(newState);
            }
        }

        public InputRecord GetState(Fix time)
        {
            InputRecord max = new InputRecord();
            foreach (InputRecord current in recordings)
            {
                if (current.time > max.time && current.time <= time)
                {
                    max = current;
                }
            }
            return max.Copy();
        }

        public void ClearFrom(Fix time)
        {
            List<InputRecord> okayRecordings = new List<InputRecord>();
            foreach (InputRecord rec in recordings)
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
