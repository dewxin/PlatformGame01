using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHighlightWhenHover : MonoBehaviour
{
    // Start is called before the first frame update
    public Material HighlightMaterial;

    private SpriteRenderer dirtySpriteRender;
    private Material dirtyRenderOriginMaterial;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HoverSceneObj();
    }

    private void HoverSceneObj()
    {
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        var mask = LayerMask.GetMask("Enemy", "Player");
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(myRay.origin.x, myRay.origin.y), Vector2.down,0.05f, mask);
        if (hit.collider != null)
        {
            var sceneUnitInfo = hit.collider.gameObject.GetComponentInChildren<IHasSceneUnitInfo>();
            var spriteRender = sceneUnitInfo.GetSpriteRenderer();
                
            if(spriteRender != dirtySpriteRender)
                HighlightSceneObj(spriteRender);
        }
        else
        {
            UnHighlightPrevObj();
        }
    }

    private void HighlightSceneObj(SpriteRenderer spriteRender)
    {
        UnHighlightPrevObj();
        if (spriteRender == null)
            return;

        dirtySpriteRender = spriteRender;
        dirtyRenderOriginMaterial = dirtySpriteRender.material;

        spriteRender.material = HighlightMaterial;
    }

    private void UnHighlightPrevObj()
    {
        if(dirtySpriteRender != null)
        {
            dirtySpriteRender.material = dirtyRenderOriginMaterial;
            dirtySpriteRender = null;
        }
    }
}
