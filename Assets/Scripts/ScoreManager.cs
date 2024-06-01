using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    private int _score;

    [SerializeField] private TMPro.TMP_Text text;

    public static int score
    {
        get => instance._score;
        set
        {
            instance._score = value;
            Globals.score = value;
            instance.text.text = "Score: " + instance._score;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            throw new Exception("Score Manager instance already exists");
        }

        instance = this;
        score = Globals.score;
    }

    private void Start()
    {
        // FIXME (Timon): Unsubscrive on scene deload
        InputManager.actions.Player.Jump.performed += Increment;
    }

    private void Increment(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            score += 1;
        }
    }

    private void OnDisable()
    {
        InputManager.actions.Player.Jump.performed -= Increment;
    }

    public static void AddScore(int amount)
    {
        score += amount;
    }
}
