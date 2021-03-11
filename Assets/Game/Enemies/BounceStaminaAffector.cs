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
 *  Restore or expend the specified stamina when this gameobject is bounced upon.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(BounceRegistrar))]
public class BounceStaminaAffector : MonoBehaviour
{
	// Fields =====================================================================================
	public BounceRegistrar Registrar { get; private set; }

	[SerializeField, Range(-1, 1)]
	private float staminaOnBounce;
	public float StaminaOnBounce => this.staminaOnBounce;

    [SerializeField]
    private bool OnlyRestoreOnNewTarget = true;
    // ============================================================================================

    // Mono =======================================================================================
    private void Awake() => this.Registrar = this.GetComponent<BounceRegistrar>();

    private void Start()
    {
        this.Registrar.SubscribeOnBounce((BounceDetector bd) =>
        {
            StaminaTracker staminaTracker = bd.GetComponentInParent<StaminaTracker>();
            if (staminaTracker != null && (bd.LastBouncedOn != this.Registrar.Collider || !this.OnlyRestoreOnNewTarget))
            {
                if (this.staminaOnBounce < 0)
                    staminaTracker.UseStamina(this.staminaOnBounce);
                else
                    staminaTracker.RegainStamina(this.staminaOnBounce);
            }
        });
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
