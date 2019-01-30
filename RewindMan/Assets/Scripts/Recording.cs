using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recording
{

    private Stack<Record> records = new Stack<Record>();

    public void Add(Record record)
    {
        records.Push(record);
    }

    public Record Get(float time)
    {
        Record last = records.Peek();
        while (last.time > time)
        {
            records.Pop();
            last = records.Peek();
        }
        return last;
    } 

}
