using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCurrentKeyCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = (int)KeyCode.Mouse0; i<= (int)KeyCode.Mouse6;++i )
        {
            var key =(KeyCode)i;
            if(Input.GetKeyDown(key))
                Debug.Log(key);
        }

    }

    private void OnGUI()
    {
    }
}
