using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;

    public Text winningText;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        winningText.gameObject.SetActive(false);
    }

    public float horizontalInput;
    public float jumpForce = 120.0f;
    private bool isGrounded = false;
    public float speed = 70.0f;
    public int health = 3;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Spikes") || collision.gameObject.CompareTag("Enemy"))
        {
            DecreasePlayerHealth();
        }
        else if (collision.gameObject.CompareTag("Respawn"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        GameObject spotlight = GameObject.Find("Spotlight");
        FocusSpotlight focusSpotlight = spotlight.GetComponent<FocusSpotlight>();
        focusSpotlight.spotlightSizeIdle -= 20f;
        focusSpotlight.spotlightSizeMoving -= 20f;
        focusSpotlight.UpdateSpotlightSize();
    }
}