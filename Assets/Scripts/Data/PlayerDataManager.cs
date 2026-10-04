using Unity.VisualScripting;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{

    public MeshRenderer myPlayerMesh;
    public Material[] availableSkins;
    public int currentWeaponCategory;
    private void Start()
    {
        LoadCosmetics();
        LoadWeaponCategory();
    }
    private void LoadCosmetics()
    {
        int savedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);
        if (myPlayerMesh != null && availableSkins.Length > savedSkinIndex)
        {
            myPlayerMesh.material = availableSkins[savedSkinIndex];
        }
    }
    private void LoadWeaponCategory() 
    {
        currentWeaponCategory = PlayerPrefs.GetInt("SelectedWeaponCategory", 0);
    }
}