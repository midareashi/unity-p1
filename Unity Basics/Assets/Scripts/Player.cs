using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask playerMask;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI massText;
    [SerializeField] TextMeshProUGUI massStoredText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI speedStoredText;

    bool isJumping;
    float horizontalInput;
    Rigidbody rb;
    int coins = 0;
    float mass = 1f;
    float massStored = 10f;
    float speed = 1.9f;
    GameObject BreakableFloor;
    Vector3 playerStart;
    int interval = 1;
    float nextTime = 0;
    bool nextInterval = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStart = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime)
        {
            nextInterval = true;
            nextTime += interval;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        // Decrease Mass
        if (Input.GetKeyDown("q"))
        {
            if (mass != 0.25f)
            {
                mass += -0.25f;
                rb.mass = mass;
                massText.text = "Mass: " + mass.ToString();
            }
        }

        // Increase Mass
        if (Input.GetKeyDown("e"))
        {
            if ((mass - 1) < massStored)
            {
                mass += 0.25f;
                rb.mass = mass;
                massText.text = "Mass: " + mass.ToString();
            }
        }

        horizontalInput = Input.GetAxis("Horizontal") * speed;

        if (BreakableFloor != null && mass >= 2)
        {
            Destroy(BreakableFloor.transform.parent.gameObject);
            BreakableFloor = null;
        }

        if (nextInterval)
        {
            ModifyMass();
        }
        nextInterval = false;
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
            BreakableFloor = other.gameObject;
        }
        if (other.gameObject.layer == 9)
        {
            //Game Over
            rb.position = playerStart;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        BreakableFloor = null;
    }

    private void ModifyMass()
    {
        float massDiff = (mass - 1);

        // Update Mass Storage
        massStored += -massDiff;

        // Check if we have enough mass stored to continue spending
        if (massDiff > 0 && massDiff > massStored)
        {
            mass = 1 + massStored;
            rb.mass = mass;
        }

        massText.text = "Mass: " + mass.ToString();
        massStoredText.text = "Stored Mass: " + massStored.ToString();
    }
}