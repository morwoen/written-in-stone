using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using CooldownManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Animator body;
    [SerializeField] private Transform passivesParent;
    [SerializeField] private InventorySO inventory;

    [SerializeField] private EffectImage effectImagePrefab;

    private bool isOpen = false;

    private void OnEnable() {
        body.gameObject.SetActive(false);

        InputManager.Instance.Player.OpenPauseMenu.performed += OnOpen;
        InputManager.Instance.Menu.ClosePauseMenu.performed += OnClose;
    }

    private void OnDisable() {
        InputManager.Instance.Player.OpenPauseMenu.performed -= OnOpen;
        InputManager.Instance.Menu.ClosePauseMenu.performed -= OnClose;
    }

    private void OnOpen(InputAction.CallbackContext obj) {
        body.gameObject.SetActive(true);
        InputManager.Instance.SwitchTo(InputManager.Instance.Menu);
        isOpen = true;
        Time.timeScale = 0;

        Cooldown.WaitUnscaled(1).OnComplete(() => {
            foreach (var passive in inventory.passive) {
                var el = Instantiate(effectImagePrefab, passivesParent);
                el.Apply(passive, true);
                el.SetTooltip(true, EffectImage.TooltipSide.BottomLeft);
            }
        });
    }

    private void OnClose(InputAction.CallbackContext obj) {
        Close();
    }

    public void Close() {
        InputManager.Instance.SwitchTo(InputManager.Instance.Player);
        isOpen = false;
        body.SetTrigger("Hide");
        passivesParent.DestroyChildren();
        Cooldown.WaitUnscaled(1).OnComplete(() => {
            body.gameObject.SetActive(false);
            Time.timeScale = 1;
        });
    }

    public void Quit() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
