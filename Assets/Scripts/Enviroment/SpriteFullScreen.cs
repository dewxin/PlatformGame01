using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFullScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Scale();

    }

    // Update is called once per frame
    void Update()
    {
        //Scale();
    }

    private void Scale()
    {
        var verticalHalfSize = Camera.main.orthographicSize;
        var horizontalHalfSize = verticalHalfSize / Screen.height * Screen.width;

        var sprite = GetComponent<SpriteRenderer>().sprite;
        var spriteVerticalHalfSize = sprite.bounds.size.y / 2;
        var yScale = verticalHalfSize/spriteVerticalHalfSize;
        var spriteHorizontalHalfSize = sprite.bounds.size.x / 2;
        var xScale = horizontalHalfSize / spriteHorizontalHalfSize;
        this.transform.localScale = new Vector3(xScale* transform.localScale.x, yScale * transform.localScale.y,1);

    }


}
