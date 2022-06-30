using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask playerMask;
    [SerializeField] TextMeshProUGUI coinText;

    bool isJumping;
    float horizontalInput;
    Rigidbody rb;
    int iCoins = 0;


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
            rb.AddForce(Vector3.up * 7, ForceMode.VelocityChange);
            isJumping = false;
        }
        // Update Coin Counter
        coinText.text = "Coins: " + iCoins.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            iCoins++;
            coinText.text = "Coins: " + iCoins.ToString();
        }
    }
}
