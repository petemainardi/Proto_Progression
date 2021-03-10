using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
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
public class TimerConditionCollision : MonoBehaviour
{
	// Fields =====================================================================================
	[SerializeField, Sirenix.OdinInspector.Required]
	private DifficultyTimer Timer;

	[SerializeField] private LayerMask TriggeringLayers;
	private bool IsColliding;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Start ()
	{
		this.Timer.AddTimerCondition(() => this.IsColliding);
	}
    // ----------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
	{
		if ((this.TriggeringLayers & (1 << collision.gameObject.layer)) != 0)
		{
			ECM_Controller mover = collision.collider.GetComponent<ECM_Controller>();
			if (mover != null)
				this.IsColliding = true;
		}
    }
    private void OnCollisionExit(Collision collision)
	{
		if ((this.TriggeringLayers & (1 << collision.gameObject.layer)) != 0)
		{
			ECM_Controller mover = collision.collider.GetComponent<ECM_Controller>();
			if (mover != null)
				this.IsColliding = false;
		}
	}
    // ============================================================================================

    // ==================================================================================
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
