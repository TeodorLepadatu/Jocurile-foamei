using UnityEngine;

public class CM2_ThrownEgg : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 targetPosition;
    private bool isThrown = false;

    public void Launch(Vector3 target)
    {
        gameObject.tag = "EggProjectile";
        targetPosition = target;
        isThrown = true;

        // Rotate toward direction
        Vector2 dir = (target - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        if (isThrown)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster") && gameObject.tag == "EggProjectile") // if the thrown object collides with a monster
        {
            var monster = other.GetComponent<CM2_MonsterAI>();

            if(monster != null) {
                other.GetComponent<CM2_MonsterAI>().TakeDamage();
            }

            Destroy(gameObject);
        }
    }
}
