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
public class FallAndRespawn : MonoBehaviour
{
	// Fields =====================================================================================
	public Vector3 StartPos { get; private set; }
	public Vector3 StartRot { get; private set; }

	public LayerMask Mask;
	private Rigidbody rb;

	public float FallTime;
	private float fallTimer;
	private bool shouldFall;

	public float RespawnTime = 5f;
	private float respawnTimer;

	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Awake ()
	{
		this.rb = this.GetComponent<Rigidbody>();
		this.Constrain(true);
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
		this.fallTimer -= Time.deltaTime;
		if (this.shouldFall && this.fallTimer < 0)
        {
			this.shouldFall = false;
			this.Constrain(false);
			this.respawnTimer = this.RespawnTime;
		}

		this.respawnTimer -= Time.deltaTime;
		if (!this.transform.position.Equals(this.StartPos) && this.respawnTimer < 0)
		{
			this.Constrain(true);
			this.transform.position = this.StartPos;
			this.transform.rotation = Quaternion.Euler(this.StartRot);
        }
	}
	// ----------------------------------------------------------------------------------
	private void OnCollisionEnter(Collision collision)
	{
		if ((this.Mask & (1 << collision.gameObject.layer)) != 0)
        {
			this.shouldFall = true;
			this.fallTimer = this.FallTime;
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
	// ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
