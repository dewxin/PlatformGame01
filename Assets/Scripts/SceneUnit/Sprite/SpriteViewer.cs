using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteViewer:MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();

    public int currentFrameId;

    private float currentFrameTime = 0;
    public float fps = 4;
    public float second1Frame => 1f / fps;

    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        UpdateFrameId();

        render.sprite = sprites[currentFrameId];
    }

    private void UpdateFrameId()
    {
        currentFrameTime += Time.deltaTime;
        if (currentFrameTime > second1Frame)
        {
            currentFrameTime -= second1Frame;
            currentFrameId++;
            if (currentFrameId >= sprites.Count)
                currentFrameId = 0;
        }
    }
}
