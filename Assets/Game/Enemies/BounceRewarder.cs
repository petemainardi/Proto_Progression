using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Awards an integer-amount of points on a subscribed bounce event by emitting the points as a
 *  change in the corresponding ReactiveProperty.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(BounceRegistrar))]
public class BounceRewarder : MonoBehaviour, IPointsRewarder
{
    // Fields =====================================================================================
    public BounceRegistrar Registrar { get; private set; }

    [SerializeField]
    private readonly int rewardPoints = 5;
    public int RewardPoints => this.rewardPoints;

    [ReadOnly]
    public readonly IntReactiveProperty Reward = new IntReactiveProperty();
    IntReactiveProperty IPointsRewarder.Reward => this.Reward;
    // ============================================================================================

    // Mono =======================================================================================
    private void Awake() => this.Registrar = this.GetComponent<BounceRegistrar>();
    private void Start()
    {
        this.Registrar.SubscribeOnBounce(_ => this.AwardPoints());
        PointsAccumulator.RegisterWithAllAccumulators(this);
    }
    // ============================================================================================

    // Events =====================================================================================
    private void AwardPoints()
    {
        //Debug.Log($"Awarding {this.rewardPoints} points!");
        this.Reward.Value = this.RewardPoints;  // OnNext doesn't fire unless the value actually changes,
        this.Reward.Value = 0;                  // so we have to clear it. Not ideal, but is simplest way...
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
