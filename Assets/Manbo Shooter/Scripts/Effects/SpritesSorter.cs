using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesSorter : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private SpriteRenderer spritesRenderers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spritesRenderers.sortingOrder = -(int)(transform.position.y * 5);
    }
}
