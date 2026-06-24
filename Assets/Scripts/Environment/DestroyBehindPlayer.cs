using UnityEngine;

public class DestroyBehindPlayer : MonoBehaviour
{
    [SerializeField] private float destroyDistance = 20f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null)
            return;

        if (transform.position.z <
            player.position.z - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}