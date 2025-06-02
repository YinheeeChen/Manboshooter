using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPlayerStatDependency
{

    [Header(" Elements ")]
    [SerializeField] private MobileJoystick playerJoystick;
    private Rigidbody2D rig;

    [Header(" Settings ")]
    [SerializeField] private float baseMoveSpeed;
    private float speed;
 
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = Vector2.right;
    }

    private void FixedUpdate()
    {
        rig.velocity = playerJoystick.GetMoveVector() * speed * Time.deltaTime;
    }

    public void UpdateStats(PlayerStatManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatVlaue(Stat.MoveSpeed) / 100;
        speed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}
