using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UnityEngine;

public partial class MouseManager : MonoBehaviour
{
    public Sprite defaultCursorSprite;
    private Texture2D defaultCursor2D;
    public Sprite clickCursorSprite;
    private Texture2D clickCursor2D;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public static Action<IHasSceneUnitInfo> OnClickSceneUnitMaybeNull = delegate { };

    public static SceneUnitInfo FocusedSceneUnit { get; private set; }

    void Start()
    {
        defaultCursor2D = CopyCurosrTexture(defaultCursorSprite);
        clickCursor2D = CopyCurosrTexture(clickCursorSprite);

        Cursor.SetCursor(defaultCursor2D, hotSpot, cursorMode);
    }

    private Texture2D CopyCurosrTexture(Sprite sprite)
    {
        var texture = CopyTexture(sprite);

        return texture;
    }

    private Texture2D CopyTexture(Sprite sprite)
    {
        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeMouseSprite();
        SelectSceneObj();
    }

    private void ChangeMouseSprite()
    {
        //ЪѓБъзѓМќ
        if(Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(clickCursor2D, hotSpot, cursorMode);

        }

        if(Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(defaultCursor2D, hotSpot, cursorMode);
        }
    }


    private void SelectSceneObj()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var mask = LayerMask.GetMask("Enemy", "Player");
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(myRay.origin.x, myRay.origin.y), Vector2.down, 0.05f, mask);

            if (hit.collider != null)
            {
                var battleInfoHolder = hit.collider.GetComponent<IHasSceneUnitInfo>();
                FocusedSceneUnit = battleInfoHolder.GetSceneUnit();
                OnClickSceneUnitMaybeNull(battleInfoHolder);
            }
            else
            {
                OnClickSceneUnitMaybeNull(null);
            }

        }

    }

}
