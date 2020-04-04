using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    Animator m_Animator;

    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // FixedUpdate in time with physics. 
    // Instead of being called before every rendered frame, 
    // FixedUpdate is called before the physics system solves any collisions 
    // and other interactions that have happened.
    // By default it is called exactly 50 times every second.
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);

        //The movement vector is made up of two numbers that can have a maximum value of 1.If they both have a value of 1, 
        //the length of the vector(known as its magnitude) will be greater than 1.
        // This is the relation between sides of a triangle described by Pythagoras’ theorem.

        //This means that your character will move faster diagonally than it will along a single axis.  
        // In order to make sure this doesn’t happen, you need to ensure the movement vector always 
        // has the same magnitude.You can do this by normalizing it.  
        // Normalizing a vector means keeping the vector’s direction the same, but changing its magnitude to 1.
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
    }

    // How root motion is applied from the Animator.

    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }


}
