using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildBoar.GUIModule;

[RequireComponent(typeof(UISprite))]
public class ShortCutSlotCoolDown : MonoBehaviour
{
    private float cooldown;
    private float maxCoolDown;

    private UISprite sprite;
    void Start()
    {
        sprite = GetComponent<UISprite>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        sprite.SetFillAmount(cooldown / maxCoolDown);

        if (cooldown<=0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetCoolDown(float cooldown)
    {
        this.cooldown = cooldown;
        this.maxCoolDown= cooldown;
        this.gameObject.SetActive(true);
    }
}
