using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelectionIndicator : MonoBehaviour
{
    public GameObject IndicatorPrefab;

            //切地图会导致indicator被Destroy
    private GameObject indicator;

    void Awake()
    {
        //怪物挂掉的时候，会同时Destroy这个选择指示器，不太好。
        MapManager.Instance.BeforeLoadScene += SetIndicatorBack;
        indicator = Instantiate(IndicatorPrefab);
        indicator.SetActive(false);
    }


    public void OnEnable()
    {
        MouseManager.OnClickSceneUnitMaybeNull += OnClickSceneUnitOwner;
    }
    public void OnDisable()
    {
        MouseManager.OnClickSceneUnitMaybeNull -= OnClickSceneUnitOwner;
    }

    private void SetIndicatorBack()
    {
        indicator.transform.SetParent(transform);
        indicator.SetActive(false);
    }


    public void OnClickSceneUnitOwner(IHasSceneUnitInfo hasSceneUnitInfo)
    {
        if(hasSceneUnitInfo == null)
        {
            indicator.SetActive(false);
            indicator.transform.SetParent(this.transform, false);
            return;
        }

        var viewRoot = hasSceneUnitInfo.GetViewRoot();
        indicator.transform.SetParent(viewRoot.transform, false);
        indicator.transform.localPosition = Vector3.zero;
        indicator.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
