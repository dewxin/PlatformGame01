using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BagContentScrollView : MonoBehaviour
{

    private BagPanel bagPanel;

    [SerializeField]
    private GameObject itemPrefab;


    private void Awake()
    {
        PlayerSingleton.Instance.Bag.OnBagContentChange += NeedRefresh;
        bagPanel = transform.GetComponentInParent<BagPanel>();
        bagPanel.OnVisibleTypeChange += OnVisibleTypeChange;

    }

    private void Start()
    {
        NeedRefresh();
    }


    private void Update()
    {
        AddItemToBag();
    }


    private void OnVisibleTypeChange(BagItemType type)
    {
        NeedRefresh();
    }

    private void NeedRefresh()
    {
        var childList = new System.Collections.Generic.List<GameObject>();
        foreach (Transform child in transform) childList.Add(child.gameObject);
        foreach (var child in childList)
        {
            child.SetActive(false);
            child.transform.SetParent(this.transform.parent);
            Destroy(child);
        }

        var visibleType = bagPanel.VisibleType;
        if(visibleType == BagItemType.Equip)
        {
            var list = PlayerSingleton.Instance.Bag.GetEquipList();
            foreach(var equip in list)
            {
                var buttonGameObj = Instantiate(itemPrefab);
                buttonGameObj.transform.SetParent(this.gameObject.transform,false);
                var bagItem = buttonGameObj.GetComponent<BagEquipButton>();
                bagItem.BagItem = equip;
                buttonGameObj.SetActive(true);

            }

        }

        //GetComponent<UIGrid>().Reposition();
    }



    private void AddItemToBag()
    {
        if(Input.GetKeyDown(KeyCode.F1))
            PlayerSingleton.Instance.Bag.AddEquip(Equip.Generate(1));
    }
}
