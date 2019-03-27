using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FixedPointy;

public class RecordUI : MonoBehaviour
{

    public Fix time;
    public Text text;
    public RectTransform rect;

    // Update is called once per frame
    public void Update()
    {
        rect.localPosition = new Vector3( (float)time * 40.0f, 0, 0);
        text.text = time.ToString();
    }
}
