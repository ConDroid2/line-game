using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlBindingsController : MonoBehaviour
{
    [SerializeField] private GameObject _rebindingDisplay;

    [Header("KeyBoardBindings")]
    [SerializeField] private TextMeshProUGUI _slowMovementKeyText;
    [SerializeField] private TextMeshProUGUI _shootKeyText;
    [SerializeField] private TextMeshProUGUI _grappleKeyText;
    [SerializeField] private TextMeshProUGUI _rotateKeyText;
    [SerializeField] private TextMeshProUGUI _selfDestructKeyText;
    [SerializeField] private Button _slowMovementKeyButton;
    [SerializeField] private Button _shootKeyButton;
    [SerializeField] private Button _grappleKeyButton;
    [SerializeField] private Button _rotateKeyButton;
    [SerializeField] private Button _selfDestrcutKeyButton;

    [Header("ControllerBindings")]
    [SerializeField] private TextMeshProUGUI _slowMovementButtonText;
    [SerializeField] private TextMeshProUGUI _shootButtonText;
    [SerializeField] private TextMeshProUGUI _grappleButtonText;
    [SerializeField] private TextMeshProUGUI _rotateButtonText;
    [SerializeField] private TextMeshProUGUI _selfDestructButtonText;
    [SerializeField] private Button _slowMovementButtonButton;
    [SerializeField] private Button _shootButtonButton;
    [SerializeField] private Button _grappleButtonButton;
    [SerializeField] private Button _rotateButtonButton;
    [SerializeField] private Button _selfDestructButtonButton;

    private void Start()
    {
        _slowMovementKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.SlowMovement); });
        _shootKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Shoot); });
        _grappleKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Grapple); });
        _rotateKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Rotate); });
        _selfDestrcutKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.SelfDestruct); });

        _slowMovementButtonButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.SlowMovement_Gamepad); });
        _shootButtonButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Shoot_Gamepad); });
        _grappleButtonButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Grapple_Gamepad); });
        _rotateButtonButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Rotate_Gamepad); });
        _selfDestructButtonButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.SlowMovement_Gamepad); });

        UpdateUIText();
    }

    

    private void UpdateUIText()
    {
        Debug.Log("Updating UI Text");
        _slowMovementKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.SlowMovement);
        _shootKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.Shoot);
        _grappleKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.Grapple);
        _rotateKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.Rotate);
        _selfDestructKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.SelfDestruct);

        _slowMovementButtonText.text = $"<sprite name=\"{InputManager.Instance.GetBinding(InputManager.Binding.SlowMovement_Gamepad)}\">";
        _shootButtonText.text = $"<sprite name=\"{InputManager.Instance.GetBinding(InputManager.Binding.Shoot_Gamepad)}\">";
        _grappleButtonText.text = $"<sprite name=\"{InputManager.Instance.GetBinding(InputManager.Binding.Grapple_Gamepad)}\">";
        _rotateButtonText.text = $"<sprite name=\"{InputManager.Instance.GetBinding(InputManager.Binding.Rotate_Gamepad)}\">";
        _selfDestructButtonText.text = $"<sprite name=\"{InputManager.Instance.GetBinding(InputManager.Binding.SelfDestruct_Gamepad)}\">";
    }

    private void HandleRebinding(InputManager.Binding binding)
    {
        ShowRebindingDisplay();
        InputManager.Instance.RebindAction(binding, () => 
            {
                HideRebindingDisplay();
                UpdateUIText();
            });
    }

    private void ShowRebindingDisplay()
    {
        _rebindingDisplay.SetActive(true);
    }

    private void HideRebindingDisplay()
    {
        _rebindingDisplay.SetActive(false);
    }
}
