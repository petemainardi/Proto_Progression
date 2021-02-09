using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Track what BounceDetector objects have interacted with the attached Collider and manage the
 *  lifetime of event streams related to those interactions.
 *  
 *  The purpose of this class is to serve as a central point of management for event streams that
 *  want to listen for when a BounceDetector interacts with the attached Collider. The reason it is
 *  useful to centralize this is that it allows us to avoid rewriting the same code for determining
 *  a bounce has occurred and for tracking the IDisposable lifetime cleanup of every listening 
 *  event individually. This way, all another component needs to do to create an event in response
 *  to a bounce is register the event with this class.
 *  
 *  TODO: One side effect of how this is being handled is that, in forfeiting the responsibility of
 *  managing their streams' lifetimes, memory leaks could be created if those components are
 *  destroyed while this one persits. I need to handle that here somewhere instead of assuming that
 *  all such components will be attached to the same gameobject as this.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Collider))]
public class BounceRegistrar : MonoBehaviour
{
    // Fields =====================================================================================
    public Collider Collider { get; private set; }

    [SerializeField, ReadOnly]
    private readonly ReactiveDictionary<BounceDetector, CompositeDisposable> registeredBouncers
        = new ReactiveDictionary<BounceDetector, CompositeDisposable>();

    [SerializeField, ReadOnly]
    private List<string> allowableTags = new List<string>();

    // ============================================================================================

    // Mono =======================================================================================
    // Lifetime Management --------------------------------------------------------------
    private void Awake()
    {
        this.Collider = this.GetComponent<Collider>();

        if (this.gameObject.layer != LayerMask.NameToLayer("Bouncing"))
            Debug.LogWarning($"{this.NameAndID()} has a BounceRewarder, it should be on the Bouncing layer.");
    }

    private void OnDestroy()
    {
        foreach (IDisposable val in this.registeredBouncers.Values)
            val.Dispose();
    }
    // ----------------------------------------------------------------------------------
    // Collision ------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        BounceDetector bouncer = collision.collider.GetComponent<BounceDetector>();
        if (bouncer == null)
            return;

        if (!this.registeredBouncers.ContainsKey(bouncer))
        {
            this.RegisterBouncer(bouncer);
        }
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================

    // Registration ===============================================================================

    public bool IsRegistered(BounceDetector bouncer) => this.registeredBouncers.ContainsKey(bouncer);

    // Track the bouncer's lifetime -----------------------------------------------------
    public void RegisterBouncer(BounceDetector bouncer)
    {
        if (this.IsRegistered(bouncer))
            return;

        IDisposable disposable = bouncer
            .OnDestroyAsObservable()
            .Subscribe(_ => this.UnRegisterBouncer(bouncer));

        this.registeredBouncers.Add(bouncer, new CompositeDisposable(disposable));
    }
    // ----------------------------------------------------------------------------------
    // Dispose of all event listeners before untracking ---------------------------------
    public void UnRegisterBouncer(BounceDetector bouncer)
    {
        if (!this.IsRegistered(bouncer))
            return;

        this.registeredBouncers[bouncer].Dispose();
        this.registeredBouncers.Remove(bouncer);
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================

    // Subscription ===============================================================================
    // Tie a stream to the registered-lifetime of the BounceDetector ------------------------------
    public bool AddToBouncerRegistration (BounceDetector bouncer, IDisposable disposable)
    {
        if (this.IsRegistered(bouncer))
        {
            this.registeredBouncers[bouncer].Add(disposable);
            return true;
        }
        return false;
    }
    // ----------------------------------------------------------------------------------
    // Add an event listener for when a new BounceDetector is registered ----------------
    // and tie the lifetime of the listener stream to the lifetime of the registration
    public void SubscribeToRegistration(Func<BounceDetector, IDisposable> observer)
    {
        this.registeredBouncers
            .ObserveAdd()
            .Subscribe((DictionaryAddEvent<BounceDetector, CompositeDisposable> e) =>
            {
                IDisposable disposable = observer(e.Key);
                if (disposable != null)
                    e.Value.Add(disposable);
            })
            .AddTo(this);
    }
    // ----------------------------------------------------------------------------------
    // Add an event listener for when any registered BounceDetector triggers a bounce ---
    // TODO: Not sure if the listener should take in the whole BounceDetector or just the BounceInfo???
    public void SubscribeOnBounce(Action<BounceDetector> observer)
    {
        this.SubscribeToRegistration((BounceDetector bouncer) =>
            bouncer.BounceInfo
                .Where((BounceInfo info) => info.BouncedOn == this.Collider)
                .Subscribe((BounceInfo info) => observer(bouncer))
            );

        // Not sure how performant this is... depends on how UniRx works.
        // While we are only subscribed to bounces on our collider, do we still process the event
        // for every bounce from this BounceDetector? That would be a lot of wasted cycles for one
        // of these alone, multiplied by however many exist...
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
