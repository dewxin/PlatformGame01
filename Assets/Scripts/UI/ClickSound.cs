using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WildBoar.GUIModule;

public class ClickSound : MonoBehaviour,IPointerClickHandler
{
    public enum SoundType
    {
        Effect,
        Music,
    }

    public SoundType Type;

    //ϣ����������������ѡ����
    public string SoundName;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (Type == SoundType.Effect)
            SoundManager.Instance.PlayEffect(SoundName);
        else if (Type == SoundType.Music)
            SoundManager.Instance.PlayMusic(SoundName);
    }
}
