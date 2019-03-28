using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixedPointy;

public class RecordUIList : MonoBehaviour
{
    List<GameObject> records = new List<GameObject>();
    public GameObject template;

    private void Start()
    {
        IEnumerable<Fix> list = new List<Fix>();
        SetByList(list);
    }

    public void SetByList(IEnumerable<Fix> records)
    {
        foreach (GameObject record in this.records)
        {
            Destroy(record);
        }
        this.records.Clear();
        foreach (Fix record in records)
        {
            GameObject recordOj = Instantiate(template);
            RecordUI ui = recordOj.GetComponent<RecordUI>();
            recordOj.transform.SetParent(transform);
            ui.time = record;
            ui.Update();
            this.records.Add(recordOj);
        }
    }
}
