using UnityEngine;
using UnityEngine.Networking;
public class WeaponManager : NetworkBehaviour
{
    // [SerializeField] private GameObject weaponGFX;
    [SerializeField] private string weaponLayerName = "Weapon";
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private PlayerWeapon PrimaryWeapon;
    private PlayerWeapon currentWeapon;

    void Start()
    {
        EquipWeapon(PrimaryWeapon);
    }
    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    private void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        if (isLocalPlayer)
            Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
    }
}