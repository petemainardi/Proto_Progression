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
 *  Responsible for determining by how much to move the assocatiated CharacterController,
 *  accounting for:
 *      - Player input
 *      - Bounce triggers
 *      - Groundedness state
 *      
 *  TEMP: Sprint capability is patched in here for demo purposes.
 *  TODO: Extra abilities should be abstracted to separate components.
 *  TODO: Could probably leverage AddForce to abstract bouncing out to a separate component.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    // Fields =====================================================================================
    [SerializeField]
    private float moveSpeed = 6.8f;
    [SerializeField, Sirenix.OdinInspector.ReadOnly]
    private Vector3 moveDir = Vector3.zero;
    public Vector3 Movement => this.moveDir;

    [SerializeField]
    private float jumpSpeed = 0.5f;
    [SerializeField]
    private float gravity = 2f;
    [SerializeField]
    private float maxCombinedJumpSpeed = 0.7f; // Limit bonus height from jump + bounce

    [SerializeField]
    private BounceDetector bouncer;
    private BoolReactiveProperty CanBounce = new BoolReactiveProperty();
    private Vector3 bounceForce;

    private CharacterController charControl;

    // TEMP
    [Sirenix.OdinInspector.Required]
    public PlayerData PlayerData;
    public float sprintMultiplier = 1.5f;
	// ============================================================================================

	// Mono =======================================================================================
	void Awake ()
	{
        this.charControl = this.GetComponent<CharacterController>();
        this.charControl
            .ObserveEveryValueChanged(c => c.velocity)
            .Subscribe((Vector3 v) => this.CanBounce.Value = v.y < 0)
            .AddTo(this);

        if (this.bouncer == null)
            Debug.LogError($"{this.NameAndID()}'s CharacterMover is missing a reference to a BounceDetector.");

        if (this.PlayerData.HasDoubleJump)
            this.gameObject.AddComponent<DoubleJump>();
	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
        this.bouncer.SetBounceCondition(this.CanBounce);
        this.bouncer.BounceInfo
            .Where((BounceInfo b) => b.BouncedOn != null)
            .Subscribe((BounceInfo b) => this.Bounce(b.Direction))
            .AddTo(this);
	}
    // ----------------------------------------------------------------------------------
    void FixedUpdate ()
	{
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;
        Vector3 transformDir = inputDir;// this.transform.TransformDirection(inputDir);
        Vector3 flatMovement = transformDir * this.moveSpeed * Time.deltaTime;

        if (Input.GetButton("Fire1") && PlayerData.HasSprint && this.charControl.isGrounded)
            flatMovement *= sprintMultiplier;

        // TODO: If in air, dampen flat movement according to momentum

        this.moveDir = new Vector3(flatMovement.x, this.moveDir.y, flatMovement.z);
        this.moveDir.y = this.PlayerJumped
            ? this.jumpSpeed
            : this.charControl.isGrounded || this.bounceForce.y > 0
            ? 0f
            : this.moveDir.y - this.gravity * Time.deltaTime;

        this.moveDir += this.bounceForce;
        this.moveDir.y = Math.Min(this.moveDir.y, this.maxCombinedJumpSpeed);
        this.bounceForce = Vector3.zero;

        this.charControl.Move(this.moveDir);
	}
    // ============================================================================================

    // Helpers ====================================================================================
    public bool IsGrounded => this.charControl.isGrounded;

    private bool PlayerJumped => Input.GetButton("Jump") && this.charControl.isGrounded;

    private void Bounce(Vector3 direction)
    {
        //Debug.Log($"Bouncing with force {direction}");
        //if (!this.bounceForce.Equals(Vector3.zero))
        //    Debug.Log($".. but already bouncing with force {this.bounceForce}!");

        this.bounceForce = direction;
    }

    public void AddForce(Vector3 force)
    {
        this.moveDir += force;
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
