using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameWeapons gameWeapons;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject defaultWeapon; // preventive

    private HeroClass heroClass;
    private string weaponId;
    private int weaponTier;
    private GameObject weaponClone;
    private WeaponData weapon;

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

        gameWeapons.AssignWeaponId();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupWeapon();
    }

    void Update()
    {
        Debug.Log(Inventory.Instance.IsFull());
    }

    public void UpgradeWeapon(int toWeaponTier)
    {
        weaponTier = toWeaponTier;
        SetupWeapon();
    }

    public void EquipWeapon(WeaponData weapon)
    {
        Inventory.Instance.RemoveItem(weapon.item); // remove item first

        if (Inventory.Instance.IsFull())
        {
            _ = Notification.Instance.ShowNotification("Inventory will be full!");
            Inventory.Instance.AddItem(weapon.item); // add back weapon since inventory is full
            return;
        }

        // return currently equipped weapon to inventory
        Inventory.Instance.AddItem(this.weapon.item);

        // set up values for new weapon
        weaponId = weapon.weaponId;
        weaponTier = weapon.weaponTier;

        SetupWeapon();
    }

    private void SetupWeapon()
    {
        // prevent multiple weapons at once
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }

        // foreach (ClassWeaponEntry cwe in gameWeapons.weaponList)
        // {
        //     if (cwe.heroClass == heroClass)
        //     {
        //         foreach (WeaponEntry we in cwe.classWeapons)
        //         {
        //             if (we.weaponData.weaponTier == weaponTier)
        //             {
        //                 if (weaponClone)
        //                 {
        //                     Destroy(weaponClone);
        //                 }
        //                 weaponClone = Instantiate(we.weaponData.weaponGobj, weaponHolder);
        //                 weapon = we.weaponData;
        //             }
        //         }
        //     }
        // }

        foreach (ClassWeaponEntry cwe in gameWeapons.weaponList)
        {
            if (cwe.heroClass == heroClass)
            {
                foreach (WeaponEntry we in cwe.classWeapons)
                {
                    if (we.weaponId == weaponId)
                    {
                        foreach (WeaponData wd in we.weaponTiers)
                        {
                            if (wd.weaponTier == weaponTier) // x_x
                            {
                                if (weaponClone)
                                {
                                    Destroy(weaponClone);
                                }
                                weaponClone = Instantiate(wd.weaponGobj, weaponHolder);
                                weapon = wd;

                                HeroPanel.Instance.SetupWeaponSlot(weapon.item);
                            }
                        }
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
    public void SetWeaponTier(HeroClass heroClass, string weaponId, int weaponTier)
    {
        this.heroClass = heroClass;
        this.weaponId = weaponId;
        this.weaponTier = weaponTier;
    }

    public HeroClass GetHeroClass()
    {
        return heroClass;
    }

    public int GetWeaponTier()
    {
        return weaponTier;
    }

    public WeaponData GetWeapon()
    {
        return weapon;
    }

    public string GetWeaponId()
    {
        return weaponId;
    }
    #endregion
}
