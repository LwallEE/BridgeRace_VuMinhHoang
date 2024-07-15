using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private BrickColor brickColor;
    public bool CanCollect(BrickColor pickerColor)
    {
        if (pickerColor == brickColor)
        {
            gameObject.SetActive(false);
            return true;
        }

        return false;
    }
}
