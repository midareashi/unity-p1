using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask playerMask;

    bool isJumping;
    float horizontalInput;
    Rigidbody rb;
    int superJumps;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        horizontalInput = Input.GetAxis("Horizontal");
    }

    // Once every physics update
    void FixedUpdate()
    {
        // Horizontal Input
        rb.velocity = new Vector3(horizontalInput, rb.velocity.y, 0);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (isJumping)
        {
            float jumpPower = 5f;
            if (superJumps > 0)
            {
                jumpPower *= 2;
                superJumps--;
            }
            rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            isJumping = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            superJumps++;
        }
    }
}
