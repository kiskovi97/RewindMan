using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FixedPointy;

namespace FixPhysics
{
    [System.Serializable]
    public class TimeRecording
    {
        private Stack<Fix> timeRecord = new Stack<Fix>();
        public RecordUIList recordUIList;

        public void Push(Fix time)
        {
            timeRecord.Push(time);
            recordUIList.SetByList(timeRecord);
        }

        public Fix GetByTime(Fix time)
        {
            Fix fromTime = 0;
            if (timeRecord.Count > 0)
            {
                fromTime = timeRecord.Pop();
                while (fromTime >= time)
                {
                    if (timeRecord.Count == 0) break;
                    fromTime = timeRecord.Pop();
                }
                if (fromTime < time)
                {
                    timeRecord.Push(fromTime);
                }
            }
            recordUIList.SetByList(timeRecord);
            return fromTime;
        }
    }
}
