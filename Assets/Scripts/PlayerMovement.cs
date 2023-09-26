using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private GameObject spotlight;

    public Text winningText;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spotlight = GameObject.Find("Spotlight");
        winningText.gameObject.SetActive(false);
    }

    public float horizontalInput;
    public float jumpForce = 120.0f;
    private bool canJump = false;
    private bool isMovementObstructedFromLeft = false;
    private bool isMovementObstructedFromRight = false;
    public float speed = 75.0f;
    public int health = 3;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if ((isMovementObstructedFromLeft && horizontalInput > 0f)
            || (isMovementObstructedFromRight && horizontalInput < 0f))
        {
            horizontalInput = 0f;
        }

        playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);

        if (canJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }

        if (collision.gameObject.CompareTag("Spikes") || collision.gameObject.CompareTag("Enemy"))
        {
            DecreasePlayerHealth();
            canJump = true;
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.CompareTag("Wall")
            && collision.GetContact(0).normal.y < 0.9f // collision is not from below
            && collision.GetContact(0).normal.y > -0.9f) // collision is not from above
        {
            if (collision.GetContact(0).normal.x > 0.9f)
            {
                isMovementObstructedFromRight = true;
            }
            else if (collision.GetContact(0).normal.x < -0.9f)
            {
                isMovementObstructedFromLeft = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            isMovementObstructedFromLeft = false;
            isMovementObstructedFromRight = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            speed = 0f;
            jumpForce = 0f;
            GameObject spotlight = GameObject.Find("Spotlight");
            FocusSpotlight focusSpotlight = spotlight.GetComponent<FocusSpotlight>();
            focusSpotlight.spotlightSizeIdle = 1000f;
            focusSpotlight.UpdateSpotlightSize();
            winningText.gameObject.SetActive(true);
        }
    }

    void DecreasePlayerHealth()
    {
        health -= 1;
        switch (health)
        {
            case 2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 0:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }

        FocusSpotlight focusSpotlight = spotlight.GetComponent<FocusSpotlight>();
        focusSpotlight.spotlightSizeMoving -= 50f;
        focusSpotlight.UpdateSpotlightSize();
    }
}