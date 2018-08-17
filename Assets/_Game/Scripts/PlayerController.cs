using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float m_Speed = 2.2f;
    private Vector2 m_Movement;
    protected Rigidbody2D m_Rb;
    protected Animator m_Anim;


    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();
    }
	private void Start ()
    {
		
	}
	
	private void FixedUpdate ()
    {
        m_Movement.x = Input.GetAxisRaw("Horizontal");
        m_Movement.y = Input.GetAxisRaw("Vertical");
        Move(m_Movement);
	}

    public void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            m_Anim.SetBool("walking", true);
            m_Anim.SetFloat("input_x", direction.x);
            m_Anim.SetFloat("input_y", direction.y);
        }
        else
        {
            m_Anim.SetBool("walking", false);
        }

        transform.position += (Vector3)direction * Time.deltaTime * m_Speed;
    }
}
