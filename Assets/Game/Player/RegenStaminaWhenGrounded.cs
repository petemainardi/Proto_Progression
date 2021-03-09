using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
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
[RequireComponent(typeof(StaminaTracker))]
public class RegenStaminaWhenGrounded : MonoBehaviour
{
	// Fields =====================================================================================
	private StaminaTracker Stamina;

	[SerializeField, Required] private ECM_Controller Mover;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Awake ()
	{
		this.Stamina = this.GetComponent<StaminaTracker>();
	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
		this.Stamina.AddRegenCondition(() => this.Mover.isGrounded);
	}
	// ----------------------------------------------------------------------------------
	// ============================================================================================
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================