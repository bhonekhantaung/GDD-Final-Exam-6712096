using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;

    [Header("Movement")]
    [SerializeField] private float speed = 500f;

    [Header("Powerup")]
    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    [SerializeField] private float normalStrength = 10f;
    [SerializeField] private float powerupStrength = 25f;

    [Header("Turbo Boost")]
    [SerializeField] private float turboStrength = 20f;
    public ParticleSystem turboParticle;

    private Coroutine powerupCoroutine;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        // Optional fallback so the starter Smoke_Particle can be found by name.
        if (turboParticle == null)
        {
            GameObject smokeObject = GameObject.Find("Smoke_Particle");
            if (smokeObject != null)
            {
                turboParticle = smokeObject.GetComponent<ParticleSystem>();
            }
        }
    }

    void Update()
    {
        float verticalInput = 0f;

        // Unity 6 project is using the new Input System.
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                verticalInput += 1f;
            }

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                verticalInput -= 1f;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TurboBoost();
            }
        }

        // Move in the direction the focal point/camera is facing.
        if (playerRb != null && focalPoint != null)
        {
            playerRb.AddForce(
                focalPoint.transform.forward * verticalInput * speed * Time.deltaTime
            );
        }

        // Keep the powerup indicator underneath the player.
        if (powerupIndicator != null)
        {
            powerupIndicator.transform.position =
                transform.position + new Vector3(0f, -0.6f, 0f);
        }
    }

    private void TurboBoost()
    {
        if (playerRb == null || focalPoint == null)
        {
            return;
        }

        playerRb.AddForce(
            focalPoint.transform.forward * turboStrength,
            ForceMode.Impulse
        );

        if (turboParticle != null)
        {
            turboParticle.Play();
        }
    }

    // If Player collides with powerup, activate powerup.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;

            if (powerupIndicator != null)
            {
                powerupIndicator.SetActive(true);
            }

            // Restart the timer if another powerup is collected before the old one expires.
            if (powerupCoroutine != null)
            {
                StopCoroutine(powerupCoroutine);
            }

            powerupCoroutine = StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration.
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);

        hasPowerup = false;

        if (powerupIndicator != null)
        {
            powerupIndicator.SetActive(false);
        }

        powerupCoroutine = null;
    }

    // If Player collides with enemy, knock the enemy AWAY from the player.
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();

            if (enemyRigidbody == null)
            {
                return;
            }

            // Enemy position minus player position points away from the player.
            Vector3 awayFromPlayer =
                (other.gameObject.transform.position - transform.position).normalized;

            float hitStrength = hasPowerup ? powerupStrength : normalStrength;

            enemyRigidbody.AddForce(
                awayFromPlayer * hitStrength,
                ForceMode.Impulse
            );
        }
    }
}