using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RateAnimator : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update

    float totalRate = -0.01f;
    string suffix = "_rate";

    Dictionary<string, float> stateName2RateDict = new Dictionary<string, float>();
    Dictionary<string, float> paramName2RateDict = new Dictionary<string, float>(); 

    void Start()
    {
        animator= GetComponent<Animator>();
        CollectRateInfo();
    }


    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            var name = GetRandomAnimationName();
            animator.Play(name,0,0);
        }
    }



    private string GetRandomAnimationName()
    {
        var ran = Random.Range(0, totalRate);

        float accumulated = 0;
        foreach (var paramNameRate in paramName2RateDict)
        {
            accumulated += paramNameRate.Value;
            if (accumulated >= ran)
                return paramNameRate.Key;
        }

        throw new System.Exception("cannot reach here");
    }


    private void CollectRateInfo()
    {
        var clipList = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clipList)
        {
            stateName2RateDict.Add(clip.name, 0);
        }


        foreach(var para in animator.parameters)
        {
            if (!para.name.EndsWith(suffix))
                return;

            var paraNameWithoutSuffix= para.name.Split('_')[0];
            paramName2RateDict.Add(paraNameWithoutSuffix, para.defaultFloat);
            totalRate+= para.defaultFloat;
        }
    }
}
