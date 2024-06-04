using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndSimButton : MonoBehaviour
{

    private void Start()
    {
    }
    public void LoadScene()
    {
        Destroy(FindObjectOfType<SpeciesManager>().gameObject);
        SceneManager.LoadScene(2);

    }
}
