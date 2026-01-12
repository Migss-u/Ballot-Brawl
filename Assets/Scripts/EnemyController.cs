using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;           
    public Animator animator;     
    public float chaseRange = 5.0f; 

    private GameObject player;    
    private bool isChasing = false; 
    private bool hasCollided = false; 

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

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (hasCollided)
            return;

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Check if the player is within chase range
        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
            animator.SetFloat("Speed", 0);
        }

        // Only chase the player if within range
        if (isChasing)
        {
            // Calculate the direction toward the player
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Determine the closest matching direction index
            currentDirectionIndex = GetClosestDirectionIndex(direction);

            // Move the AI object toward the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            // Update animator parameters based on the direction
            animator.SetFloat("Horizontal", directions[currentDirectionIndex].x);
            animator.SetFloat("Vertical", directions[currentDirectionIndex].y);
            animator.SetFloat("Speed", direction.magnitude > 0 ? 1 : 0);
        }
    }

    private int GetClosestDirectionIndex(Vector2 direction)
    {
        int closestIndex = 0;
        float closestDot = -1f; // Start with the smallest possible dot product

        for (int i = 0; i < directions.Length; i++)
        {
            float dotProduct = Vector2.Dot(direction, directions[i].normalized);
            if (dotProduct > closestDot)
            {
                closestDot = dotProduct;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true; 
            isChasing = false;  

            animator.SetBool("IsExploding", true);
            audioManager.PlaySFX(audioManager.explosion);

            Destroy(gameObject, 0.5f);
        }
    }
}