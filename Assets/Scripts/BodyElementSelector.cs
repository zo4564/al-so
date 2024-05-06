using UnityEngine;

    //body element selector dzia�a z buttonami z menu, zapami�tuje ostatnio wybran� cz�� cia�a i przekazuje do bodyCreatora
public class BodyElementSelector : MonoBehaviour
{
    public GameObject selectedElementPrefab;
    public bool destroy = false;

    public void ChangeMode()
    {
        destroy = !destroy;
    }
    public void SelectElement(GameObject elementPrefab)
    {
        selectedElementPrefab = elementPrefab;
        destroy = false;
    }
}
