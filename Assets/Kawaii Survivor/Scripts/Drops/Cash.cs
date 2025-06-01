using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cash : DroppableCurrency
{
    [Header("Actions")]
    public static Action<Cash> onCollected;

    protected override void Collected()
    {
        onCollected?.Invoke(this);

    }
}
