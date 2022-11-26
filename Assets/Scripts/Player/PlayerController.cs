using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.InputSystem;
using CooldownManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour, ICharacterController
{
    public enum CharacterState
    {
        Grounded,
        Dashing,
        Dead,
    }

    public enum DashDirection
    {
        LookDirection,
        MovementDirection,
    }

    private KinematicCharacterMotor Motor;
    private PlayerInputController input;

    [Header("Stable Movement")]
    [SerializeField] private float maxStableMoveSpeed = 10f;
    [SerializeField] private float stableMovementSharpness = 15f;
    [SerializeField] private float rotationSharpness = 20f;

    [Header("Air Movement")]
    [SerializeField] private float maxAirMoveSpeed = 15f;
    [SerializeField] private float airAccelerationSpeed = 15f;
    [SerializeField] private float drag = 0.1f;

    //[Header("Jumping")]
    //[SerializeField] private bool AllowJumpingWhenSliding = false;
    //[SerializeField] private float JumpUpSpeed = 10f;
    //[SerializeField] private float JumpScalableForwardSpeed = 10f;
    //[SerializeField] private float JumpPreGroundingGraceTime = 0f;
    //[SerializeField] private float JumpPostGroundingGraceTime = 0f;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashDuration = 0.05f;
    [SerializeField] private DashDirection dashDirection;
    [SerializeField] private ParticleSystem dashEffect = null;
    [SerializeField] private DashSO dash;

    [Header("Health")]
    [SerializeField] public HealthSO health;
    //[SerializeField] private GameObject healEffect;
    //[SerializeField] private GameObject deathEffect;

    [Header("Fighting")]
    [SerializeField] private Transform castingPoint;
    [SerializeField] private Transform handPoint;
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float invulnerabilityTime = 1;
    [SerializeField] private float respawnTime = 5;
    [SerializeField] private Color hitColor;
    private bool invulnerable = false;
    private AudioSource source;

    [Header("Misc")]
    [SerializeField] private List<Collider> ignoredColliders = new List<Collider>();
    [SerializeField] private Animator animator = null;
    [SerializeField] private LayerMask enemyLayerMask;

    public CharacterState CurrentCharacterState { get; private set; }

    private LayerMask raycastMask;

    private void OnEnable() {
        raycastMask = ~LayerMask.GetMask("EnemyIgnoreRaycast", "Ignore Raycast");
        Motor = GetComponent<KinematicCharacterMotor>();
        input = GetComponent<PlayerInputController>();
        source = GetComponent<AudioSource>();
        TransitionToState(CharacterState.Grounded);
        Motor.CharacterController = this;
        health.Respawn();

        Application.targetFrameRate = 60;
    }

    private void OnDisable() {
    }

    /// <summary>
    /// Handles movement state transitions and enter/exit callbacks
    /// </summary>
    public void TransitionToState(CharacterState newState) {
        CharacterState tmpInitialState = CurrentCharacterState;
        OnStateExit(tmpInitialState, newState);
        CurrentCharacterState = newState;
        OnStateEnter(newState, tmpInitialState);
    }

    /// <summary>
    /// Event when entering a state
    /// </summary>
    public void OnStateEnter(CharacterState state, CharacterState fromState) {

    }

    /// <summary>
    /// Event when exiting a state
    /// </summary>
    public void OnStateExit(CharacterState state, CharacterState toState) {

    }

    public void BeforeCharacterUpdate(float deltaTime) {

    }

    public void AfterCharacterUpdate(float deltaTime) {

    }

    public bool IsColliderValidForCollisions(Collider coll) {
        if (CurrentCharacterState == CharacterState.Dashing && enemyLayerMask == (enemyLayerMask | (1 << coll.gameObject.layer))) {
            return false;
        }

        if (ignoredColliders.Count == 0) {
            return true;
        }

        if (ignoredColliders.Contains(coll)) {
            return false;
        }

        return true;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider) {

    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {

    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {

    }

    public void PostGroundingUpdate(float deltaTime) {
        // Handle landing and leaving ground
        if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround) {
            OnLanded();
        } else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround) {
            OnLeaveStableGround();
        }
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) {

    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
        switch (CurrentCharacterState) {
            case CharacterState.Grounded: {
                if (input.LookDirection.sqrMagnitude > 0f && rotationSharpness > 0f) {
                    Vector3 direction = new Vector3(input.LookDirection.x, 0, input.LookDirection.y);
                    Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, direction, 1 - Mathf.Exp(-rotationSharpness * deltaTime)).normalized;

                    // Set the current rotation (which will be used by the KinematicCharacterMotor)
                    Quaternion LookAtRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);

                    currentRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }
                break;
            }
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
        switch (CurrentCharacterState) {
            case CharacterState.Grounded: {
                    // Ground movement
                    if (Motor.GroundingStatus.IsStableOnGround) {
                        float currentVelocityMagnitude = currentVelocity.magnitude;

                        Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                        // Reorient velocity on slope
                        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                        // Calculate target velocity
                        Vector3 inputRight = Vector3.Cross(input.Movement, Motor.CharacterUp);
                        Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * input.Movement.magnitude;
                        var maxVelocity = maxStableMoveSpeed;
                        Vector3 targetMovementVelocity = reorientedInput * maxVelocity;

                        // Smooth movement Velocity
                        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-stableMovementSharpness * deltaTime));

                        //var normalisedVelocity = currentVelocity;
                        //var forwardVector = Vector3.Project(normalisedVelocity, transform.forward);
                        //var rightVector = Vector3.Project(normalisedVelocity, transform.right);
                        //var forwardVelocity = forwardVector.magnitude * Vector3.Dot(forwardVector, transform.forward) / maxStableMoveSpeed / maxVelocity;
                        //var rightVelocity = rightVector.magnitude * Vector3.Dot(rightVector, transform.right) / maxStableMoveSpeed / maxVelocity;

                        //animator.SetFloat("Locomotion", forwardVelocity);
                        //animator.SetFloat("LocomotionSide", rightVelocity);
                        animator.SetFloat("Speed", Mathf.Abs(currentVelocity.magnitude));
                    }
                    // Air movement
                    else {
                        // Add move input
                        if (input.Movement.sqrMagnitude > 0f) {
                            Vector3 addedVelocity = input.Movement * airAccelerationSpeed * deltaTime;

                            Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                            // Limit air velocity from inputs
                            if (currentVelocityOnInputsPlane.magnitude < maxAirMoveSpeed) {
                                // clamp addedVel to make total vel not exceed max vel on inputs plane
                                Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, maxAirMoveSpeed);
                                addedVelocity = newTotal - currentVelocityOnInputsPlane;
                            } else {
                                // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f) {
                                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                }
                            }

                            // Prevent air-climbing sloped walls
                            if (Motor.GroundingStatus.FoundAnyGround) {
                                if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f) {
                                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                }
                            }

                            // Apply added velocity
                            currentVelocity += addedVelocity;
                        }

                        var horVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
                        if (horVelocity.magnitude > maxAirMoveSpeed) {
                            horVelocity = Vector3.ClampMagnitude(horVelocity, maxAirMoveSpeed);
                            currentVelocity = new Vector3(horVelocity.x, currentVelocity.y, horVelocity.z);
                        }

                        // Gravity
                        currentVelocity += Physics.gravity * deltaTime;

                        // Drag
                        currentVelocity *= (1f / (1f + (drag * deltaTime)));
                    }

                    // Handle jumping
                    //if (input.JumpPressed) {
                    //  // See if we actually are allowed to jump
                    //  if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) {
                    //    // Calculate jump direction before ungrounding
                    //    Vector3 jumpDirection = Motor.CharacterUp;
                    //    if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround) {
                    //      jumpDirection = Motor.GroundingStatus.GroundNormal;
                    //    }

                    //    // Makes the character skip ground probing/snapping on its next update. 
                    //    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                    //    Motor.ForceUnground();

                    //    // Add to the return velocity and reset jump state
                    //    currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                    //    currentVelocity += (input.Movement * JumpScalableForwardSpeed);
                    //    input.JumpPerformed();
                    //  }
                    //}

                    if (input.Dash && dash.Charges > 0) {
                        TransitionToState(CharacterState.Dashing);
                        input.DashPerformed();
                        dash.Dash();
                        //dashEffect?.Play();
                        //dashEffect?.GetComponent<PolygonSoundSpawn>().Execute();
                        //animator?.SetTrigger("Dash");
                        Cooldown.Wait(dashDuration)
                            .OnComplete(() => {
                                //dashEffect?.Stop();
                                TransitionToState(CharacterState.Grounded);
                            });
                    }
                    break;
                }
            case CharacterState.Dashing: {
                    if (dashDirection == DashDirection.LookDirection) {
                        currentVelocity = Motor.CharacterForward * dashSpeed;
                    } else if (dashDirection == DashDirection.MovementDirection) {
                        currentVelocity = currentVelocity.WithY(0).normalized * dashSpeed;
                    }
                    break;
                }
        }
    }

    protected void OnLanded() {
    }

    protected void OnLeaveStableGround() {
    }

    public void Damage(int damage) {
        if (CurrentCharacterState == CharacterState.Dashing || invulnerable || health.CurrentHealth <= 0) return;
        var screenShakeMultiplier = 1 - PlayerPrefs.GetInt("NoScreenShake", 0);
        //GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulseAt(transform.position, impulse * screenShakeMultiplier);
        print("damaged!!!==================");
        var dead = health.Damage(damage);
        if (dead) {
            animator.SetTrigger("Die");
            input.enabled = false;
            //Cooldowns.Wait(respawnTime)
            //  .OnComplete(() => {
            //  });
        } else {
            //Instantiate(hitEffectPrefab, transform.position, hitEffectPrefab.transform.rotation, transform);

            //invulnerable = true;
            meshRenderer.material.SetColor("_BaseColor", hitColor);
            //Cooldowns.Wait(invulnerabilityTime)
            //  .OnComplete(() => {
            //      invulnerable = false;
            //      meshRenderer.material.SetColor("_BaseColor", Color.white);
            //  });
        }
    }

    //public void Heal(int damage) {
    //    health.Heal(damage);
    //    Instantiate(healEffect, transform.position - Vector3.up, healEffect.transform.rotation, transform);
    //}

    public Vector3 PredictMovement(float time) {
        return transform.position + Motor.Velocity * time;
    }

    public void IgnoreCollider(Collider col, bool ignore = true) {
        if (ignore) {
            ignoredColliders.Add(col);
        } else {
            ignoredColliders.Remove(col);
        }
    }
}
