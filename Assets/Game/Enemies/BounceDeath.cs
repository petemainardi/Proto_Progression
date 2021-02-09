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
 *  Limit the number of times this may be bounced upon before perishing.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(BounceRegistrar))]
public class BounceDeath : MonoBehaviour
{
    // Fields =====================================================================================
    public BounceRegistrar Registrar { get; private set; }

    [SerializeField]
    private readonly int bouncesToKill = 1;
    public int NumBouncesToKill => this.bouncesToKill;

    private int numBounces;
    public int NumBouncesUntilDeath => this.bouncesToKill - this.numBounces;

    [SerializeField, Sirenix.OdinInspector.Required]
    private GameObject ThingToKill;
    // ============================================================================================

    // Mono =======================================================================================
    private void Awake() => this.Registrar = this.GetComponent<BounceRegistrar>();
    private void Start()
    {
        this.Registrar.SubscribeOnBounce(_ => this.CountBounce());
    }
    // ============================================================================================

    // Events =====================================================================================
    private void CountBounce()
    {
        if (++this.numBounces >= this.bouncesToKill)
            GameObject.Destroy(this.ThingToKill);
        //Debug.Log($"{this.NameAndID()}: Bounced {this.numBounces}");
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
