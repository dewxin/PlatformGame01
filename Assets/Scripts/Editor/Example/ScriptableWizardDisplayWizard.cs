// Simple Wizard that clones an object.

using UnityEngine;
using UnityEditor;
using System.Collections;

public class ScriptableWizardDisplayWizard : ScriptableWizard
{
    public GameObject objectToCopy = null;
    public int numberOfCopies = 2;

    [MenuItem("Tools/Example/Show DisplayWizard usage")]
    static void CreateWindow()
    {
        // Creates the wizard for display
        ScriptableWizard.DisplayWizard("Copy an object.",
            typeof(ScriptableWizardDisplayWizard),
            "Copy!");
    }

    void OnWizardUpdate()
    {
        helpString = "Clones an object a number of times";
        if (!objectToCopy)
        {
            errorString = "Please assign an object";
            isValid = false;
        }
        else
        {
            errorString = "";
            isValid = true;
        }
    }

    void OnWizardCreate()
    {
        for (int i = 0; i < numberOfCopies; i++)
            Instantiate(objectToCopy, Vector3.zero, Quaternion.identity);
    }
}