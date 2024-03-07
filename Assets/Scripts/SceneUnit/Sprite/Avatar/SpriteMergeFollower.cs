using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMergeFollower : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer spriteRender;
    void Start()
    {

        spriteRender= GetComponent<SpriteRenderer>();

        var merge = PlayerSingleton.Instance.PlayerMain.gameObject.GetComponentInChildren<SpriteMerge>();
        if(merge != null )
        {

            merge.OnMainCharacterSpriteUpdate += OnSpriteUpdate;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpriteUpdate(Sprite sprite)
    {
        spriteRender.sprite = sprite;

    }
}
