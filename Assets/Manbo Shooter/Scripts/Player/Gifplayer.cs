using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gifplayer : MonoBehaviour
{
    public List<Sprite> gifFrames; // 存储 GIF 的帧
    public float frameRate = 0.04f; // 每帧的时间间隔
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

    public void SetFrames(List<Sprite> frames)
    {
        gifFrames = frames;
        currentFrame = 0;
        timer = 0f;
    }

    public void SetFrames(Sprite[] frames)
    {
        gifFrames = new List<Sprite>(frames);
        currentFrame = 0;
        timer = 0f;
    }
}