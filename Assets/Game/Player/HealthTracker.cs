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
    public bool DieAtZeroHealth = true; // TODO: Might need to replace with a DeathHandler for animations, etc.

    [SerializeField]
    private float InvulnerabilityLength = 2;
    private float invulnerabilityTimer = 0;
    public BoolReactiveProperty IsInvulnerable = new BoolReactiveProperty();
    
    public MeshRenderer Mesh;
    [SerializeField] private float MinOpacity = 0.35f;
    [SerializeField] private float CycleOffset = 8f;
    private float blinkTimer = 0;

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

        this.IsInvulnerable.Subscribe((bool b) =>
        {
            if (b)
                this.blinkTimer = 0;
            else
                this.ResetOpacity();

        }).AddTo(this);
    }
	// ----------------------------------------------------------------------------------
	void Update ()
	{
        this.blinkTimer += Time.deltaTime;
        this.invulnerabilityTimer -= Time.deltaTime;
        if (this.IsInvulnerable.Value)
        {
            if (this.Mesh != null)
                this.BlinkOpacity();
            this.IsInvulnerable.Value = this.invulnerabilityTimer > 0;
        }
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
            this.invulnerabilityTimer = this.InvulnerabilityLength;
            this.IsInvulnerable.Value = true;
            //this.blinkTimer = 0;
        }
        else if ((this.HealingLayers.value & (1 << other.layer)) != 0)
        {
            this.Health.Value = Math.Min(this.Health.Value + 1, this.MaxHealth);
            GameObject.Destroy(other);
        }
    }

    private void BlinkOpacity()
    {
        Color c = this.Mesh.material.color;
        c.a = Math.Max(
            this.MinOpacity,
            0.5f + 0.5f * Mathf.Cos(2 * (float)Math.Pow(this.blinkTimer + this.CycleOffset, 2))
            );
        this.Mesh.material.color = c;
    }
    private void ResetOpacity()
    {
        Color c = this.Mesh.material.color;
        c.a = 1;
        this.Mesh.material.color = c;
    }

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
