using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject button;

    private void Start() {
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void Play() {
        SceneManager.LoadScene(1);
    }
}
