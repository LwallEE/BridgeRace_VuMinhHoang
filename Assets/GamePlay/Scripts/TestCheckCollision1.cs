using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example usage
public class TestCheckCollision1 : MonoBehaviour
{
    void Start()
    {
        Box box1 = new Box(new Vector3(-2.07f, 0.00f, 1.05f), new Vector3(1.00f, 1.00f, 2.30f), new Vector3(0.00f, 1.38f, 0.00f));
        Box box2 = new Box(new Vector3(0.18f, 0.00f, 0.00f), new Vector3(3.36f, 1.00f, 1.00f), new Vector3(0.00f, 5.59f, 0.00f));

        bool collision = CollisionDetection.BoxesCollide(box1, box2);
        Debug.Log("Collision: " + collision); // Expected output: true
    }
}



