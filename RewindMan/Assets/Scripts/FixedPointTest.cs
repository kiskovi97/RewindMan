using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FixedPointy;

public class FixedPointTest : MonoBehaviour
{

    public Text output;
    public Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        output.text = TestNumbers() + "\n" + TestNumbers2();
    }

    string TestNumbers()
    {
        string outText = "Cosinus\n";
        outText += Fix.FractionMask + " "+ Fix.FractionRange + " " + Fix.IntegerMask + " " + Fix.MaxInteger + "\n";
        outText += "cos(1): 0.5403023\n";
        outText += "cos(1): "+Mathf.Cos(1)+"\n";
        outText += "0.999847\n";
        outText += FixMath.Cos(Fix.One);
        outText += "\n";
        return outText;
    }
    string TestNumbers2()
    {
        string outText = "Multiple addings\n";
        Fix fixNumber= Fix.Zero;
        float floatNumber = 0f;
        Fix per = 10000;
        for (int i=1; i< 10000; i++)
        {
            Fix addedValue = 1;
            addedValue = addedValue / per;
            fixNumber += addedValue;
            floatNumber += 1 / 10000f;
        }
        outText += "1.068008\n";
        outText += fixNumber.ToString();
        outText += "\n";
        outText += "0.9999536\n";
        outText += floatNumber;
        outText += "\n";
        return outText;
    }

    private Fix time = Fix.Zero;
    private float timeFloat = 0f;

    private void FixedUpdate()
    {
        Fix per = 10000;
        Fix deltaTime = (int)(Time.fixedDeltaTime * 10000);
        if ((float) time < 10f)
        {
            timeFloat += Time.fixedDeltaTime;
            deltaTime = deltaTime / per;
            time += deltaTime;
            timeText.text = deltaTime + "\n" + Time.fixedDeltaTime + "\n" + FixConverter.ToFix(timeFloat).ToString() + " : 10.06004\n" + (float)time +" : 10.00842";
        }
    }
}
