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
    [SerializeField] private Button _slowMovementKeyButton;
    [SerializeField] private Button _shootKeyButton;
    [SerializeField] private Button _grappleKeyButton;
    [SerializeField] private Button _rotateKeyButton;

    private void Start()
    {
        UpdateUIText();
    }

    private void UpdateUIText()
    {
        _slowMovementKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.SlowMovement);
        _shootKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.Shoot);
        _grappleKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.Grapple);
        _rotateKeyText.text = InputManager.Instance.GetBinding(InputManager.Binding.Rotate);

        _slowMovementKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.SlowMovement); });
        _shootKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Shoot); });
        _grappleKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Grapple); });
        _rotateKeyButton.onClick.AddListener(() => { HandleRebinding(InputManager.Binding.Rotate); });
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
