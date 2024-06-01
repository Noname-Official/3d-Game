using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private PlayerInputActions _actions;

    public static PlayerInputActions actions
    {
        get
        {
            return instance._actions;
        }
        set
        {
            instance._actions = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            throw new Exception("Input Manager instance already exists");
        }

        instance = this;
        _actions = new PlayerInputActions();
        _actions.Enable();
    }
}
