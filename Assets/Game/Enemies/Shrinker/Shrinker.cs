using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Enemy type that shrinks in size a certain number of times before perishing.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class Shrinker : MonoBehaviour
{
    // Fields =====================================================================================
    [SerializeField, Sirenix.OdinInspector.Required]
    private BounceRegistrar BounceRegistrar;
    
    [SerializeField, Sirenix.OdinInspector.Required]
    public Transform TransformToShrink;
    public readonly float PercentToShrinkBy = 75;
    public readonly float NumShrinks = 3;
    private int numShrunk;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Start ()
	{
        this.BounceRegistrar.SubscribeOnBounce(_ => this.Shrink());
	}
	// ----------------------------------------------------------------------------------
	// ============================================================================================

	// Events =====================================================================================
    public void Shrink()
    {
        if (this.numShrunk++ < this.NumShrinks)
        {
            this.TransformToShrink.localScale = this.TransformToShrink.localScale * this.PercentToShrinkBy / 100;
        }
        else
        {
            GameObject.Destroy(this.TransformToShrink.gameObject);
        }
    }
	// ============================================================================================
	
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
