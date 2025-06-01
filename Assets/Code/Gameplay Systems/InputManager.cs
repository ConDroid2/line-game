using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public BaseControls Controls { get; private set; }

    public enum Binding
    {
        Accept,
        Cancel,
        SlowMovement,
        Shoot,
        Grapple,
        Rotate,
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right
    }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Controls = new BaseControls();
        Controls.PlayerMap.Enable();

        Controls.PlayerMap.OpenMenu.performed += HandlePausePerformed;
        Controls.PlayerMap.OpenDevMenu.performed += HandleDevMenuPerformed;
        Controls.PlayerMap.OpenMap.performed += HandleMapPerformed;
        Controls.PauseMap.CloseMenu.performed += HandleCloseMenuPerformed;
        Controls.MapMap.CloseMap.performed += HandleMapClosed;

        Debug.Log(GetBinding(Binding.SlowMovement));
    }

    private void OnDisable()
    {
        Controls.PlayerMap.OpenMenu.performed -= HandlePausePerformed;
        Controls.PlayerMap.OpenDevMenu.performed -= HandleDevMenuPerformed;
        Controls.PlayerMap.OpenMap.performed -= HandleMapPerformed;
        Controls.PauseMap.CloseMenu.performed -= HandleCloseMenuPerformed;
        Controls.MapMap.CloseMap.performed -= HandleMapClosed;
    }

    public void HandlePausePerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null && SceneManager.GetActiveScene().name != "JPMenu_Main")
        {
            GameManager.Instance.HandlePause();
            Controls.PlayerMap.Disable();
            Controls.PauseMap.Enable();
        }
    }

    public void HandleDevMenuPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandleDevMenu();
        }
    }

    public void HandleMapPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandleMap();
            Controls.PlayerMap.Disable();
            Controls.MapMap.Enable();
        }
    }

    public void HandleCloseMenuPerformed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandlePause();
            Controls.PauseMap.Disable();
            Controls.PlayerMap.Enable();
        }
    }

    public void HandleCloseMenuFromButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HandlePause();
            Controls.PauseMap.Disable();
            Controls.PlayerMap.Enable();
        }
    }

    public void HandleMapClosed(InputAction.CallbackContext context)
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.HandleMap();
            Controls.MapMap.Disable();
            Controls.PlayerMap.Enable();
        }
    }

    public string GetBinding(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.SlowMovement:
                return Controls.PlayerMap.Sprint.bindings[1].ToDisplayString();
            case Binding.Shoot:
                return Controls.PlayerMap.FireProjectile.bindings[0].ToDisplayString();
            case Binding.Grapple:
                return Controls.PlayerMap.Grapple.bindings[0].ToDisplayString();
            case Binding.Rotate:
                return Controls.PlayerMap.Rotate.bindings[0].ToDisplayString();
            case Binding.Move_Up:
                return Controls.PlayerMap.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return Controls.PlayerMap.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return Controls.PlayerMap.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return Controls.PlayerMap.Move.bindings[4].ToDisplayString();
        }
    }

    public void RebindAction(Binding binding, System.Action onBindingComplete)
    {
        Controls.PlayerMap.Disable();

        InputAction action;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.SlowMovement:
                action = Controls.PlayerMap.Sprint;
                bindingIndex = 1;
                break;
            case Binding.Shoot:
                action = Controls.PlayerMap.FireProjectile;
                bindingIndex = 0;
                break;
            case Binding.Grapple:
                action = Controls.PlayerMap.Grapple;
                bindingIndex = 0;
                break;
            case Binding.Rotate:
                action = Controls.PlayerMap.Rotate;
                bindingIndex = 0;
                break;
        }

        action.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                Controls.PlayerMap.Enable();
                onBindingComplete();
            })
            .Start();
    }

}
