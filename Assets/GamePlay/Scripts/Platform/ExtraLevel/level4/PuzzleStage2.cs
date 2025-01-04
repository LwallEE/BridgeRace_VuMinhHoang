using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleStage2 : MonoBehaviour
{
    [SerializeField] RotateBlock block1, block2, block3;

    public void OnClickButton1()
    {
        if (block1.IsRotating || block2.IsRotating) return;

        block1.OnRotate();
        block2.OnRotate(false);
    }
    public void OnClickButton2()
    {
        if (block1.IsRotating || block3.IsRotating) return;

        block1.OnRotate();
        block3.OnRotate();
    }
    public void OnClickButton3()
    {
        if (block3.IsRotating || block2.IsRotating) return;

        block2.OnRotate(false);
        block3.OnRotate();
    }
}
