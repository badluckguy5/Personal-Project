using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabs : MonoBehaviour
{
    public GameObject[] panels;

    private GameObject currentPanel;

    public void switchPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }


}
