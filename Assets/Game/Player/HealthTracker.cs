using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Track collisions that affect a health value, and provide a period after such a collision (in
 *  which health was lost) during which further negative effects cannot be received.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Collider))]
public class HealthTracker : MonoBehaviour
{
    // Fields =====================================================================================
    public LayerMask DamagingLayers;
    public LayerMask HealingLayers;
    private LayerMask storageLayer = 0;

    public IntReactiveProperty Health = new IntReactiveProperty(5);
    public int MaxHealth { get; private set; }

    [SerializeField]
    private float invulnerabilityLength = 2;
    private float invulnerabilityTimer = 0;

    public BoolReactiveProperty IsInvulnerable = new BoolReactiveProperty();

    public bool DieAtZeroHealth = true; // TODO: Might need to replace with a DeathHandler for animations, etc.
    
    public Collider Collider { get; private set; }
	// ============================================================================================

	// Mono =======================================================================================
	void Awake ()
	{
        this.Collider = this.GetComponent<Collider>();

        this.storageLayer = this.HealingLayers;
        this.Health.Subscribe((int h) =>
            this.HealingLayers = h < this.MaxHealth
            ? this.storageLayer
            : (LayerMask) 0
            ).AddTo(this);

        this.MaxHealth = this.Health.Value;

        if (this.DieAtZeroHealth)
            this.Health
                .Where((int h) => h == 0)
                .Subscribe((int h) => GameObject.Destroy(this.gameObject))
                .AddTo(this);
    }
	// ----------------------------------------------------------------------------------
	void Update ()
	{
        this.invulnerabilityTimer -= Time.deltaTime;
        if (this.IsInvulnerable.Value && this.invulnerabilityTimer <= 0)
            this.IsInvulnerable.Value = false;
	}
    // ----------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.gameObject.name.Equals("Ground") || collision.collider.gameObject.name.Equals("Player")) return;
        this.CheckCollision(collision.gameObject);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if (hit.collider.gameObject.name.Equals("Ground") || hit.collider.gameObject.name.Equals("Player")) return;
        this.CheckCollision(hit.gameObject);
    }
    // ============================================================================================

    private void CheckCollision(GameObject other)
    {
        if ((this.DamagingLayers & (1 << other.layer)) != 0
            && !this.IsInvulnerable.Value)
        {
            this.Health.Value = Math.Max(this.Health.Value - 1, 0);
            this.invulnerabilityTimer = this.invulnerabilityLength;
            this.IsInvulnerable.Value = true;
        }
        else if ((this.HealingLayers.value & (1 << other.layer)) != 0)
        {
            this.Health.Value = Math.Min(this.Health.Value + 1, this.MaxHealth);
            GameObject.Destroy(other);
        }
    }

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
