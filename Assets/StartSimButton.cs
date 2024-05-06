using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSimButton : MonoBehaviour
{
    public SpeciesManager speciesManager;
    public BodyCreator creator;
    private void Start()
    {
        speciesManager = FindObjectOfType<SpeciesManager>();
        creator = FindObjectOfType<BodyCreator>();

    }
    public void LoadScene()
    {
        if(speciesManager.createdSpecies.Count == 0) 
        {
            creator.SaveOrganism();
        }
        SceneManager.LoadScene(0);
    }
}
