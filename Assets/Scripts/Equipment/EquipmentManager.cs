using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public void SelectWeaponCategory(int category) 
    {
        PlayerPrefs.SetInt("SelectedWeaponCategory", category);
        PlayerPrefs.Save();
    }
}


