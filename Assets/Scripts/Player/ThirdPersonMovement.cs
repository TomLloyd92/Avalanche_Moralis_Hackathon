using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThirdPersonMovement : NetworkBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    public Transform cam;
    CharacterController controller;
    public float speed = 6f;
    public float sprintSpeed = 12f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVel;
    public TMP_Text addressText;
    bool isAuthenticated = false;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        if (isLocalPlayer)
        {
            Debug.Log("CAMERA TIME");
            freeLookCamera = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
            freeLookCamera.LookAt = this.gameObject.transform;
            freeLookCamera.Follow = this.gameObject.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {

            Debug.Log("CAMERA TIME");
        if (isLocalPlayer)
        {
            //freeLookCamera = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
            //freeLookCamera.LookAt = this.gameObject.transform;
            //freeLookCamera.Follow = this.gameObject.transform;
        }

        updateText();

        if(Input.GetKey(KeyCode.Space))
        {
            jump();

        }

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
    private void jump()
    {

        anim.SetBool("Jump", true);
        Debug.Log("JUMPING");
    }

    private void updateText()
    {
        // Update character address if it has not been set
        if (!isAuthenticated && MoralisInterface.GetUser() != null)
        {
            string addr = MoralisInterface.GetUser().authData["moralisEth"]["id"].ToString();

            addressText.text = string.Format("{0}...{1}", addr.Substring(0, 6), addr.Substring(addr.Length - 3, 3));

            isAuthenticated = true;
        }
    }
}


