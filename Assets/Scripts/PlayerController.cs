using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Button controlButton;
    public VoteManager VoteManager;
    public Health playerHealth;
    public Text BuffIndicator;
    public Text deathMessage;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private Vector2[] directions = {
        Vector2.up,
        new Vector2(1, 1),
        Vector2.right,
        new Vector2(1, -1),
        Vector2.down,
        new Vector2(-1, -1),
        Vector2.left,
        new Vector2(-1, 1)
    };

    private int currentDirectionIndex = 4;
    private bool isMoving = false;
    private bool isKnockedBack = false;
    private bool isDead = false;

    private void Start()
    {
        controlButton.onClick.AddListener(OnButtonTap);

        deathMessage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isKnockedBack) return;

        if (isMoving)
        {
            rb.MovePosition(rb.position + directions[currentDirectionIndex].normalized * moveSpeed * Time.deltaTime);
        }

        animator.SetFloat("Horizontal", directions[currentDirectionIndex].x);
        animator.SetFloat("Vertical", directions[currentDirectionIndex].y);
        animator.SetFloat("Speed", isMoving ? 1 : 0);
    }

    public void OnButtonTap()
    {
        if (isKnockedBack) return;
        currentDirectionIndex = (currentDirectionIndex + 1) % directions.Length;
    }

    public void OnButtonHold()
    {
        if (isKnockedBack) return;
        isMoving = true;
    }

    public void OnButtonRelease()
    {
        isMoving = false;
    }

    private IEnumerator currentSpeedEffect;
    private bool isFrozen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Vote"))
        {
            Destroy(other.gameObject);
            int randomValue = Random.Range(5, 12);
            VoteManager.playerVoteCount += randomValue;
            audioManager.PlaySFX(audioManager.votepickup);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            playerHealth.TakeDamage(1);
            animator.SetBool("isHurt", true);

            if (playerHealth.currentHealth <= 0)
            {
                StartCoroutine(HandleGameOver());
                return;
            }

            Vector2 knockbackDirection = (rb.position - (Vector2)other.transform.position).normalized;

            StartCoroutine(ApplyKnockback(knockbackDirection));

            StartCoroutine(ResetHurtAnimation());
        }
        else if (other.gameObject.CompareTag("Buff"))
        {
            int randomEffect = Random.Range(0, 5);

            switch (randomEffect)
            {
                case 0:
                    playerHealth.AddHealth(1);
                    BuffIndicator.text = "Vital Vote (adds health)";
                    audioManager.PlaySFX(audioManager.vitalvote);
                    break;

                case 1:
                    playerHealth.TakeDamage(1);
                    animator.SetBool("isHurt", true);
                    BuffIndicator.text = "Reputation Dip (reduces health)";

                    if (playerHealth.currentHealth <= 0)
                    {
                        StartCoroutine(HandleGameOverFromDebuff());
                        Destroy(other.gameObject);
                        return;
                    }

                    StartCoroutine(ResetHurtAnimation());
                    audioManager.PlaySFX(audioManager.reputationdip);
                    break;

                case 2:
                    int randomSpeedChange = Random.Range(0, 2) == 0 ? -3 : 3;
                    ApplySpeedEffect(randomSpeedChange, 10f);

                    if (randomSpeedChange == 3)
                    {
                        BuffIndicator.text = "Campaign Surge (speeds up)";
                        audioManager.PlaySFX(audioManager.campaignsurge);
                    }
                    else if (randomSpeedChange == -3)
                    {
                        BuffIndicator.text = "Slow Roll (slows down)";
                        audioManager.PlaySFX(audioManager.slowroll);
                    }
                    break;

                case 3:
                    int randomValue = Random.Range(10, 21);
                    VoteManager.playerVoteCount += randomValue;
                    BuffIndicator.text = $"Voting Boost +{randomValue} Votes";
                    audioManager.PlaySFX(audioManager.votingboost);
                    break;

                case 4:
                    ApplyFreezeEffect(2f);
                    BuffIndicator.text = "Frozen Fury (3s freeze)";
                    audioManager.PlaySFX(audioManager.frozenfury);
                    break;
            }

            Destroy(other.gameObject);
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction)
    {
        float knockbackForce = 3f;
        float knockbackDuration = 0.5f;

        isKnockedBack = true;
        rb.velocity = direction * knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    private void ApplySpeedEffect(int speedChange, float duration)
    {
        if (currentSpeedEffect != null)
        {
            StopCoroutine(currentSpeedEffect);
        }

        currentSpeedEffect = SpeedEffectCoroutine(speedChange, duration);
        StartCoroutine(currentSpeedEffect);
    }

    private IEnumerator SpeedEffectCoroutine(int speedChange, float duration)
    {
        float originalSpeed = 5f;

        this.moveSpeed = originalSpeed + speedChange;

        yield return new WaitForSeconds(duration);

        this.moveSpeed = originalSpeed;
        currentSpeedEffect = null;
    }

    private void ApplyFreezeEffect(float duration)
    {
        if (isFrozen) return;

        StartCoroutine(FreezeEffectCoroutine(duration));
    }

    private IEnumerator FreezeEffectCoroutine(float duration)
    {
        isFrozen = true;
        isMoving = false;
        isKnockedBack = true;

        animator.SetFloat("Speed", 0);

        yield return new WaitForSeconds(duration);

        isKnockedBack = false;
        isFrozen = false;
    }

    private IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isHurt", false);
    }

    private IEnumerator HandleGameOver()
    {
        isDead = true;

        yield return new WaitForSeconds(.5f);

        Time.timeScale = 0f;

        // Allow the animator to use unscaled time so the death animation can play
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        animator.SetBool("isDead", true);

        // Monitor animation progress and freeze before it resets
        AnimatorStateInfo stateInfo;
        do
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Check if animation is near the end (e.g., 90% complete)
            if (stateInfo.IsName("Death Front") && stateInfo.normalizedTime >= 0.9f)
            {
                animator.speed = 0; 
                break;             
            }

            yield return null; 
        }
        while (true);

        yield return new WaitForSecondsRealtime(1f);

        deathMessage.gameObject.SetActive(true);
        deathMessage.text = "You Died!";

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        SceneManager.LoadScene("Death Scene");
    }

    private IEnumerator HandleGameOverFromDebuff()
    {
        isDead = true;

        yield return new WaitForSeconds(.3f);

        Time.timeScale = 0f;

        // Allow the animator to use unscaled time so the death animation can play
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        animator.SetBool("isDead", true);

        // Monitor animation progress and freeze before it resets
        AnimatorStateInfo stateInfo;
        do
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Check if animation is near the end (e.g., 90% complete)
            if (stateInfo.IsName("Death Front") && stateInfo.normalizedTime >= 0.9f)
            {
                animator.speed = 0;
                break;
            }

            yield return null;
        }
        while (true);

        yield return new WaitForSecondsRealtime(1f);

        deathMessage.gameObject.SetActive(true);
        deathMessage.text = "You Died!";

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        SceneManager.LoadScene("Death Scene");
    }
}
