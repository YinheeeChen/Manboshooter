using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private bool collected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect(Player playerTransform)
    {
        if (collected)
            return;

        collected = true;

        StartCoroutine(MoveTowardsPlayer(playerTransform));
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

    private void Collected()
    { 
        gameObject.SetActive(false);
        
    }
}
