using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Transform cam;
    CharacterController controller;
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVel;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f && Input.GetKey(KeyCode.LeftShift))
        {
            run();
        }
        else if (direction.magnitude >= 0.1f && !Input.GetKey(KeyCode.LeftShift))
        {
            walk();
        }
        else
        {
            idle();
        }
    }

    private void walk()
    {
        anim.SetFloat("Speed", 0.5f);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);
    }

    private void run()
    {
        anim.SetFloat("Speed", 1f);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDirection.normalized * sprintSpeed * Time.deltaTime);
    }

    private void idle()
    {
        anim.SetFloat("Speed", 0.0f);
    }
}


