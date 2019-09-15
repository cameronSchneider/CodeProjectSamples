using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour
{
    public GameObject mTarget;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float mOffsetZ;

    private Vector3 mLastTargetPosition;
    private Vector3 mCurrentVelocity;
    private Vector3 mLookAheadPos;

        // Use this for initialization
        private void Start()
        {
            mTarget = GameObject.FindGameObjectWithTag("Player");
            mLastTargetPosition = mTarget.transform.position;
            mOffsetZ = (transform.position - mTarget.transform.position).z;
            transform.parent = null;
        }


        // FixedUpdate used to smooth the physics following
        private void FixedUpdate()
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (mTarget.transform.position - mLastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                mLookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                mLookAheadPos = Vector3.MoveTowards(mLookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = mTarget.transform.position + mLookAheadPos + Vector3.forward*mOffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref mCurrentVelocity, damping);

            transform.position = newPos;

            mLastTargetPosition = mTarget.transform.position;
        }
    }
