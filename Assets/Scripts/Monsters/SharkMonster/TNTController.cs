using System.Collections;
using UnityEngine;

public class TNTController : MonoBehaviour
{
    public GameObject sharkMonster;
    public float explosionRadius = 5.0f; // Radius of the explosion
    public float explosionForce = 700f; // Force of the explosion
    public int damage = 30; // Damage dealt by the explosion
    public LayerMask damageLayer; // Layer of objects that can be damaged

    private Animator animator;
    
    //For Audio
    public AudioClip explodeSound;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object should trigger the explosion
        if (other.CompareTag("DemonShark"))
        {
            Explode();
        }
    }

    void Explode()
    {

        // Find all objects within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);
        
        SharkMonsterHealth monster = sharkMonster.GetComponent<SharkMonsterHealth>();
        SharkMonsterController monsterMovement = sharkMonster.GetComponent<SharkMonsterController>();
        if (monster != null)
        {
            monster.TakeDamage(damage);
            monsterMovement.Freeze();
        }

        foreach (Collider2D nearbyObject in colliders)
        {
            // Apply explosion force to nearby objects with a Rigidbody2D
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = nearbyObject.transform.position - transform.position;
                rb.AddForce(direction.normalized * explosionForce);
            }
        }

        // Destroy the TNT object after the explosion
        animator.SetTrigger("Explode");
        audioSource.PlayOneShot(explodeSound); // Play sound
        StartCoroutine(DestroyAfterAnimation());



    }
    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the length of the explosion animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // Optionally, add a slight delay to ensure the animation completes
        // yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
