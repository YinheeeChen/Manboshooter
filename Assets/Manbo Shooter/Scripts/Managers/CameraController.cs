using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private Vector2 minmaxXY;
    
    // Start is called before the first frame update
    private void LateUpdate() {
        if (target == null){
            Debug.LogError("no target!");
            return;
        }

        Vector3 targetPos = target.position;
        targetPos.z = -10;

        if (!GameManager.instance.UseInfiniteMap)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, -minmaxXY.x, minmaxXY.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -minmaxXY.y, minmaxXY.y);
        }

        transform.position = targetPos;
    }
}
