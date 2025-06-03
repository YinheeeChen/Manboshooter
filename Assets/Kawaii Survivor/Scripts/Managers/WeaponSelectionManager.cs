using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Transform conrainersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header("Data")]
    [SerializeField] private WeaponDataSO[] starterWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;
    
    public void GmaeStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                if (selectedWeapon == null) return;

                playerWeapons.AddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;

            case GameState.WEAPONSELECTION:
                Configure();
                break;

            default:
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void Configure()
    {
        conrainersParent.Clear();

        for (int i = 0; i < 3; i++)
            GenerateWeaponContainer();
    }

    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer weaponContainer = Instantiate(weaponContainerPrefab, conrainersParent);

        WeaponDataSO randomWeaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        int level = Random.Range(0, 4);

        weaponContainer.Configure(randomWeaponData.WeaponIcon, randomWeaponData.WeaponName, level, randomWeaponData);

        weaponContainer.Button.onClick.RemoveAllListeners();
        weaponContainer.Button.onClick.AddListener(() => WeaponSelectedCallback(weaponContainer, randomWeaponData, level));

    }

    private void WeaponSelectedCallback(WeaponSelectionContainer weaponContainer, WeaponDataSO weaponData, int level)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;

        foreach (WeaponSelectionContainer container in conrainersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == weaponContainer)
                container.Select();
            else
                container.Deselect();
        }
    }

}