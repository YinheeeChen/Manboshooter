using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    private bool collected;
    
    private void OnEnable()
    {
        collected = false;
    }

    public void Collect(Player player)
    {
        if (collected)
            return;

        collected = true;

        StartCoroutine(MoveTowardsPlayer(player));
    }

    IEnumerator MoveTowardsPlayer(Player player)
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            Vector2 targetPosition = player.GetCenter();

            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Collected();
    }

    protected abstract void Collected();
   
}
