using UnityEngine;

public class FocusSpotlight : MonoBehaviour
{
    public GameObject player;
    public float spotlightSizeMoving = 100f;
    public float spotlightSizeIdle = 700f;
    public bool slowShrink = false;

    void LateUpdate()
    {
        // Follow x and y position of player, maintain z position
        transform.position = new Vector3(player.transform.position.x, 25f + player.transform.position.y,
            transform.position.z);
        // If player has any velocity
        if (player.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        {
            // Animate the spotlight to shrink down to 100x100
            transform.localScale = Vector3.Lerp(transform.localScale,
                new Vector3(spotlightSizeMoving, spotlightSizeMoving, 1f), slowShrink ? 0.001f : 0.1f);
        }
        else
        {
            // Animate the spotlight to grow to 700x700
            transform.localScale = Vector3.Lerp(transform.localScale,
                new Vector3(spotlightSizeIdle, spotlightSizeIdle, 1f), 0.1f);
        }
    }
}