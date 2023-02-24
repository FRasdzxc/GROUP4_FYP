using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameWeapons gameWeapons;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject defaultWeapon; // preventive

    private HeroClass heroClass;
    private int weaponTier;
    private GameObject weaponClone;

    private static WeaponManager instance;
    public static WeaponManager Instance
    {
        get => instance;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupWeapon();
    }

    public void UpgradeWeapon(int toWeaponTier)
    {
        weaponTier = toWeaponTier;
        SetupWeapon();
    }

    private void SetupWeapon()
    {
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (ClassWeaponEntry cwe in gameWeapons.weaponList)
        {
            if (cwe.heroClass == heroClass)
            {
                foreach (WeaponEntry we in cwe.classWeapons)
                {
                    if (we.weaponData.weaponTier == weaponTier)
                    {
                        if (weaponClone)
                        {
                            Destroy(weaponClone);
                        }
                        weaponClone = Instantiate(we.weaponData.weaponGobj, weaponHolder);
                    }
                }
            }
        }

        if (!weaponClone) // preventive
        {
            weaponClone = Instantiate(defaultWeapon, weaponHolder);
        }
    }

    #region Setters/Getters
    public void SetWeaponTier(HeroClass heroClass, int weaponTier)
    {
        this.heroClass = heroClass;
        this.weaponTier = weaponTier;
    }

    public int GetWeaponTier()
    {
        return weaponTier;
    }
    #endregion
}
