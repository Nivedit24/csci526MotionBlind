using UnityEngine;

public class FocusSpotlight : MonoBehaviour
{
    public GameObject player;
    public float spotlightOffsetX = 0f;
    public float spotlightOffsetY = 25f;
    public float spotlightSizeMoving = 150f;
    public float spotlightSizeIdle = 700f;
    public float shrinkSpeed = 0.1f;
    public float expandSpeed = 0.2f;
    public bool slowShrink = false;
    public float slowShrinkSpeed = 0.001f;
    public bool slowExpand = false;
    public float slowExpandSpeed = 0.001f;
    public float minVelocityForShrink = 0.1f;

    public void UpdateSpotlightSize()
    {
        // If player has any velocity or is moving, shrink the spotlight
        if (player.GetComponent<Rigidbody2D>().velocity.magnitude > minVelocityForShrink)
        {
            // Animate the spotlight to shrink down to 100x100
            transform.localScale = Vector3.Lerp(transform.localScale,
                new Vector3(spotlightSizeMoving, spotlightSizeMoving, 1f), slowShrink ? slowShrinkSpeed : shrinkSpeed);
        }
        else
        {
            // Animate the spotlight to grow to 700x700
            transform.localScale = Vector3.Lerp(transform.localScale,
                new Vector3(spotlightSizeIdle, spotlightSizeIdle, 1f), slowExpand ? slowExpandSpeed : expandSpeed);
        }
    }

    void LateUpdate()
    {
        // Follow x and y position of player, maintain z position
        transform.position = new Vector3(player.transform.position.x + spotlightOffsetX,
            player.transform.position.y + spotlightOffsetY,
            transform.position.z);
        UpdateSpotlightSize();
    }
}