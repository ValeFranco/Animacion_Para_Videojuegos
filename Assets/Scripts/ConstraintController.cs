using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ConstraintController : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float proceduralInfluence;
    [SerializeField] private MultiParentConstraint[] animatedBones;
    [SerializeField] private MultiParentConstraint[] proceduralBones;

    private void UpdateInfluence(float weight)
    {
        if (animatedBones == null) return;
        foreach (MultiParentConstraint multiParentConstraint in animatedBones)
        {
            if (multiParentConstraint == null) continue;
            multiParentConstraint.weight = weight;
        }

        if (proceduralBones == null) return;
        foreach (MultiParentConstraint proceduralConstraint in proceduralBones)
        {
            if (proceduralBones == null) continue;
            proceduralConstraint.weight = 1 - weight;
        }
    }
    private void OnValidate()
    {
        UpdateInfluence(proceduralInfluence);
    }
}
