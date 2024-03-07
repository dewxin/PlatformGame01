using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Nameplate : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro nameText;
    // Start is called before the first frame update

    [SerializeField] 
    private SpriteRenderer nameplateRenderer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeName(string name)
    {
        nameText.text = name;

        float fontSize = nameText.fontSize;

        nameplateRenderer.size = new Vector2(0.2f+fontSize*name.Length,nameplateRenderer.size.y);


    }
}
