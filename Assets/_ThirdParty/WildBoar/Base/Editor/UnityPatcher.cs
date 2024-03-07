using HarmonyLib;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class UnityPatcher
{

    static UnityPatcher()
    {
        try
        {
            DoPatch();
        }
        catch(Exception e)
        {
            Debug.LogError(e.ToString());  
        }
    }


    public static void DoPatch()
    {

        PatchImplicitConversion();
    }

    private static bool MethodPatched(string name)
    {
        var originalMethods = Harmony.GetAllPatchedMethods();
        foreach (var method in originalMethods) 
        {
            if (method.Name == name)
                return true;
        }
        return false;   
    }

    #region Implicit Conversion

    private static void PatchImplicitConversion()
    {
        var method = GetImplicitMethod(typeof(UnityEngine.Object), typeof(bool));


        var patched = typeof(UnityPatcher).GetMethod(nameof(PatchBoolConversion));


        var harmony = new Harmony("com.company.project.product");
        var assembly = typeof(UnityEngine.Object).Assembly;
        harmony.Patch(method, new HarmonyMethod(patched));

        Debug.Log($"op_Implicit is pateched: {MethodPatched("op_Implicit")}");
    }

    private static MethodInfo GetImplicitMethod(Type type, Type returnType)
    {
        var methodList = type.GetMethods();
        foreach (var method in methodList)
        {
            if (method.Name == "op_Implicit" && method.ReturnType == returnType)
            {
                return method;
            }
        }
        return null;
    }

    private static int i = 0;
    public static bool PatchBoolConversion(UnityEngine.Object exists)
    {
        i++;
        if(i % 3 == 0)
        {
            System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(2);

            var invokingAssembly = stackFrame.GetMethod().DeclaringType.Assembly.FullName;
            bool isUnity = invokingAssembly.Contains("Unity");

            if (!isUnity)
                Debug.LogWarning($"WARNING: invoking implicit conversion to bool! {invokingAssembly}");
        }
        return exists != null;
    }

    #endregion

    #region Log
    private static void PatchTemp()
    {

    }


    public static void RowGotDoubleClicked(int index)
    {
        Debug.Log($"{index} got double clicked");
    }


    #endregion
}



