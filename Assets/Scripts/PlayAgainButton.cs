using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayAgainButton : MonoBehaviour
{
    private void Start()
    {
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
