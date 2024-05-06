using UnityEngine;

    //body element selector dzia³a z buttonami z menu, zapamiêtuje ostatnio wybran¹ czêœæ cia³a i przekazuje do bodyCreatora
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
