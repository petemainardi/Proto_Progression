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
 *  Remain in place until a collision with the specified LayerMask occurs, triggering, after the
 *  specified fall time, the effect of gravity and physics on this object. After the specified
 *  respawn time, the object resets to its original position.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class FallAndRespawn : MonoBehaviour, Disappearable
{
	// Fields =====================================================================================
	public Vector3 StartPos { get; private set; }
	public Vector3 StartRot { get; private set; }

	public MeshRenderer MeshRenderer;
	public LayerMask Mask;
	private Rigidbody rb;
	private Collider Collider;

	public float FallTime;
	private float fallTimer;
	private bool shouldFall;

	public float Shakiness;
	private IEnumerator shaking;

	public float RespawnTime = 5f;
	private float respawnTimer;

	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Awake ()
	{
		this.rb = this.GetComponent<Rigidbody>();
		this.Constrain(true);
		this.Collider = this.GetComponent<Collider>();

		this.shaking = this.ShakeRoutine();
	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
		this.StartPos = this.transform.position;
		this.StartRot = this.transform.rotation.eulerAngles;		
	}
	// ----------------------------------------------------------------------------------
	void Update ()
	{
		this.fallTimer += Time.deltaTime;
		if (this.shouldFall && this.fallTimer > this.FallTime)
        {
			this.shouldFall = false;
			this.Constrain(false);
			this.respawnTimer = this.RespawnTime;
			StopCoroutine(this.shaking);
		}

		this.respawnTimer -= Time.deltaTime;
		if (!this.transform.position.Equals(this.StartPos) && this.respawnTimer < 0)
		{
			this.Constrain(true);
			this.transform.position = this.StartPos;
			this.transform.rotation = Quaternion.Euler(this.StartRot);
			this.Reappear();
        }
	}
	// ----------------------------------------------------------------------------------
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.shouldFall && (this.Mask & (1 << collision.gameObject.layer)) != 0)
        {
			this.shouldFall = true;
			this.fallTimer = 0;
			StartCoroutine(this.shaking);
        }
	}
	// ----------------------------------------------------------------------------------
	// ============================================================================================

	// Helpers ====================================================================================
	private void Constrain(bool freeze)
	{
		this.rb.useGravity = !freeze;
		this.rb.constraints = freeze
			? RigidbodyConstraints.FreezeAll
			: RigidbodyConstraints.None;
	}

	private IEnumerator ShakeRoutine()
    {
		while (true)
        {
			this.transform.localRotation = this.NoiseRotation();
			yield return new WaitForEndOfFrame();
        }
	}

	private float Noise(float seed) => (Mathf.PerlinNoise(seed, this.fallTimer * this.Shakiness) - 0.5f) * 2;
	private Quaternion NoiseRotation() =>
		Quaternion.Euler(
			this.Noise(1) * this.Shakiness,
			this.Noise(10) * this.Shakiness,
			this.Noise(1) * this.Shakiness
			);
	// ============================================================================================

	// Disappearable ==============================================================================
	public Transform Transform => this.transform;
	public void Disappear()
    {
		this.Collider.enabled = false;
		this.rb.isKinematic = true;
		if (this.MeshRenderer != null)
			this.MeshRenderer.enabled = false;
    }

	public void Reappear()
    {
		this.Collider.enabled = true;
		this.rb.isKinematic = false;
		if (this.MeshRenderer != null)
			this.MeshRenderer.enabled = true;
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
