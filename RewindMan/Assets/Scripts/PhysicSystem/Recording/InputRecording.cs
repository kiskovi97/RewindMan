using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

public class InputRecording
{
    List<InputRecord> recordings = new List<InputRecord>();

    public void Clear()
    {
        recordings.Clear();
    }

    public void AddState(InputRecord record)
    {
        InputRecord newState = new InputRecord(record.left, record.right, record.up, record.time);
        if (recordings.Count > 0)
        {
            InputRecord last = recordings[recordings.Count - 1];
            if ((last.left != record.left) || (last.right != record.right) || (last.up != record.up))
            {
                recordings.Add(newState);
                Debug.Log(record);
            }
        }
        else
        {
            Debug.Log(record);
            recordings.Add(newState);
        }
    }

    public InputRecord GetState(Fix time)
    {
        InputRecord output = new InputRecord();
        foreach (InputRecord rec in recordings)
        {
            if (rec.time > output.time && rec.time <= time)
            {
                output = rec;
            }
        }
        return output.Copy();
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
