using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  This class does things...
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================

[RequireComponent(typeof(Collider))]
public class DisappearOnCollide : SerializedMonoBehaviour
{
    [SerializeField, Required]
    private Disappearable ThingToDisappear;

    public LayerMask Mask;

    private void OnCollisionEnter(Collision collision)
    {
        if ((this.Mask & (1 << collision.gameObject.layer)) != 0)
            this.ThingToDisappear.Disappear();
    }
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================

public interface Disappearable
{
    void Disappear();
    Transform Transform { get; }
}