using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using GestureRecognizer;
using UnityEngine.Rendering.PostProcessing;

public class WiiControlsDPad : MonoBehaviour
{
    [SerializeField] private float drawDelay = 0.4f;
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private float m_StepInterval;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
    public bool lockCursor = true;
    [SerializeField] private DrawDetector[] detectors;

    private Camera m_Camera;
    private bool m_Jump;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private AudioSource m_AudioSource;

    private bool m_cursorIsLocked = true;
    private bool spraying;
    private bool isLeft;
    private bool isRight;
    private bool isRotating = false;
    Quaternion targetRot = Quaternion.identity;
    private DrawDetector playerDrawDet;
    
    void Start()
    {
        detectors = FindObjectsOfType<DrawDetector>();
        playerDrawDet = GetComponentInChildren<DrawDetector>();

        DisableDrawing();

        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();

        spraying = false;
    }

    void Update()
    {
        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Cursor.visible = !Cursor.visible;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.C))
        {
            spraying = !spraying;

            if (spraying)
                StartCoroutine(EnableDrawing());
            else
                DisableDrawing();
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }


    private void FixedUpdate()
    {
        RotateCam();

        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                           m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        if (!isRotating && !spraying)
        {
            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;
        }


        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        
        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;

            if (m_Jump)
            {
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
        if(!isRotating && !spraying)
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);

        if (!spraying)
        {
            Time.timeScale = 1.0f;
            SetCursorLock(true);
            UpdateCursorLock();
        }
        else
        {
            Time.timeScale = 0.25f;
            SetCursorLock(false);
        }
    }


    IEnumerator EnableDrawing()
    {
        yield return new WaitForSeconds(drawDelay);

        print("Enabling All");

        foreach(DrawDetector d in detectors)
        {
            d.enabled = true;
        }
    }

    private void DisableDrawing()
    {
        StopCoroutine(EnableDrawing());

        print("Disabling All");

        playerDrawDet.ClearLines();

        foreach(DrawDetector d in detectors)
        {
            d.enabled = false;
        }
    }


    private void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            //we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }

    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }

    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                         Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

    private void GetInput(out float speed)
    {
        float vertical = 0.0f;
        float horizontal = 0.0f;

        // Read input, only if not rotating
        if (!isRotating)
        {
            vertical = CrossPlatformInputManager.GetAxis("Vertical");
            horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        }

        bool waswalking = m_IsWalking;

        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
    }
    
    private void RotateCam()
    {
        if (!spraying)
        {
            if (isLeft && !isRotating)
            {
                if (transform.rotation == targetRot)
                    targetRot = Quaternion.AngleAxis(90.0f, Vector3.down) * targetRot;
                isRotating = true;
            }

            if (isRight && !isRotating)
            {
                if (transform.rotation == targetRot)
                    targetRot = Quaternion.AngleAxis(90.0f, Vector3.up) * targetRot;
                isRotating = true;
            }
        }

        transform.localRotation = Quaternion.RotateTowards(transform.rotation, targetRot, m_RotationSpeed * Time.deltaTime);

        if(Quaternion.Dot(transform.rotation, targetRot) > 0.9999f)
        {
            isRotating = false;
        }

        UpdateCursorLock();
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /*Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);*/
    }

    public bool CheckIfSpraying()
    {
        return spraying;
    }
}
