using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Titlemaneger : MonoBehaviour
{
    [SerializeReference]
    private string stagename;                                                   //シーン名

    private InputAction pressAnyKeyAction =
            new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    private void OnEnable() => pressAnyKeyAction.Enable();
    private void OnDisable() => pressAnyKeyAction.Disable();

    void Update()
    {
        if (pressAnyKeyAction.triggered)
        {
            Debug.Log("key=" + pressAnyKeyAction);
            Time.timeScale = 1f;
            SceneManager.LoadScene(stagename);
        }
    }
}