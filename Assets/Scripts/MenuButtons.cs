using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    private void Start()
    {
    }
    public void StartDesigner()
    {
        SceneManager.LoadScene(1);
    }
    public void StartSpeciesFromGenomes()
    {
        SceneManager.LoadScene(4);
    }
}

