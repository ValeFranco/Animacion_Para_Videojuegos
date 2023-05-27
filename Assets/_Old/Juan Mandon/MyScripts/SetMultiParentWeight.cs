using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class SetMultiParentWeight : MonoBehaviour
{
    void Start()
    {
        // Get all GameObjects in the scene
        GameObject[] objects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in objects)
        {
            // Check if object has Multi-Parent constraint
            MultiParentConstraint constraint = obj.GetComponent<MultiParentConstraint>();
            if (constraint != null)
            {
                // Set weight to 1
                constraint.weight = 1;
            }
        }
    }
}
