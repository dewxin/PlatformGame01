using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach2Camera : MonoBehaviour
{
    // Start is called before the first frame update

    public bool RemoveSameName = true;
    public Vector3 Offset = Vector3.zero;
    void Start()
    {
        RemoveSame();
        transform.SetParent(Camera.main.transform);
        transform.localPosition = Offset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RemoveSame()
    {
        if (!RemoveSameName)
            return;
        var attachList = Camera.main.GetComponentsInChildren<Attach2Camera>();

        foreach(var previous in attachList)
        {
            if(previous.gameObject.name == this.name)
                Destroy(previous.gameObject);
        }


    }
}
