using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask playerMask;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI massText;

    bool isJumping;
    bool isOnBreakableFloor = false;
    float horizontalInput;
    Rigidbody rb;
    int coins = 0;
    int mass = 1;


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
        if (Input.GetKeyDown("e"))
        {
            mass = 2;
        }
        if (Input.GetKeyDown("q"))
        {
            mass = 1;
        }
        horizontalInput = Input.GetAxis("Horizontal") * 1.9f;
    }

    // Once every physics update
    void FixedUpdate()
    {
        if (isOnBreakableFloor && mass == 2)
        {

        }

        // Horizontal Input
        rb.velocity = new Vector3(horizontalInput, rb.velocity.y, 0);

        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (isJumping)
        {
            rb.AddForce(Vector3.up * 7, ForceMode.VelocityChange);
            isJumping = false;
        }
        // Update Coin Counter
        coinText.text = "Coins: " + coins.ToString();
        massText.text = "Mass: " + mass.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            coins++;
            coinText.text = "Coins: " + coins.ToString();
        }

        if (other.gameObject.layer == 8)
        {
            isOnBreakableFloor = true;
            if (mass == 2)
            {
                Destroy(other.transform.parent.gameObject);
            }
        }
    }
}
