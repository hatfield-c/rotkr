using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class InputRunner
{
    public static InputRunner Instance { get; private set; }
    public InputRunner()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }
        controls = new InputMaster();
        controls.Enable();
    }
    ~InputRunner()
    {
        controls.Disable();
    }

    #region references
    public InputMaster controls;
    #endregion
}
