using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField]
    private Vector3 MoveVec;

    private HorizontalBoundry boundry;

    void Start()
    {
        boundry = Boundry.FindBoundryInAncestorChild(transform) as HorizontalBoundry;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(MoveVec*Time.deltaTime);

        var boundryPair = this.boundry.GetBoundry();

        if(transform.position.x < boundryPair.Item1.x)
        {
            transform.position = new Vector3(boundryPair.Item2.x, transform.position.y, transform.position.z);
        }

        if(transform.position.x > boundryPair.Item2.x)
        {
            transform.position = new Vector3(boundryPair.Item1.x, transform.position.y, transform.position.z);
        }

        
    }
}
