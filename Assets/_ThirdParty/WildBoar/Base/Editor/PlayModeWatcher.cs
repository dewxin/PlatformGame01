using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayModeWatcher
{
    public enum EditorMode
    {
        None,
        EditMode,
        PlayMode,
    }

    public static EditorMode CurrentMode = EditorMode.None;

    static PlayModeWatcher()
    {
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
    }

    private static void EditorApplication_playModeStateChanged(PlayModeStateChange modeState)
    {
        if(modeState == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Exit Edit mode");
            CurrentMode = EditorMode.None;

            Singleton.Reset();
        }

        if(modeState == PlayModeStateChange.EnteredEditMode)
        {
            UnityPatcher.DoPatch();
            Debug.Log("enter edit mode");
            CurrentMode = EditorMode.EditMode;

        }

        //ExitingPlayMode
        if (modeState == PlayModeStateChange.ExitingPlayMode)
        {
            Debug.Log("Exit play mode");
            CurrentMode = EditorMode.None;
            Singleton.Reset();
        }
        //EnteredPlayMode
        if (modeState == PlayModeStateChange.EnteredPlayMode)
        {
            CurrentMode = EditorMode.PlayMode;
            Debug.Log("Enter play mode");
            Debug.Log(SceneManager.GetActiveScene().name);

        }

    }





}



