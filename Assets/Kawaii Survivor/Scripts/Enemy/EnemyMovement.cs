using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Element")]
    private Player player;


    [Header("Settings")]
    [SerializeField] private float speed;


    public void StorePlayer(Player player){
        this.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) FollowPlayer();
    }


    private void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + speed * direction * Time.deltaTime;
        transform.position = targetPosition;
    }

}
