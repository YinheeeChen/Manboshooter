using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gifplayer : MonoBehaviour
{
    public List<Sprite> gifFrames; // 存储 GIF 的帧
    public float frameRate = 0.01f; // 每帧的时间间隔
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (gifFrames == null || gifFrames.Count == 0)
        {
            Debug.LogError("请在 Inspector 中为 gifFrames 添加帧图像！");
        }
    }

    void Update()
    {
        if (gifFrames == null || gifFrames.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % gifFrames.Count; // 循环播放
            spriteRenderer.sprite = gifFrames[currentFrame];
        }
    }
}