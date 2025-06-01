using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chest : MonoBehaviour, ICollectable
{
    [Header("Actions")]
    public static Action onCollected;

    public void Collect(Player player)
    {
        onCollected?.Invoke();
        Destroy(gameObject);
    }

}
