using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    [SerializeReference]
    private string stagename;                                                   //シーン名

    public void Stagemove()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(stagename);
    }
}