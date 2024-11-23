using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private KeyCode _pickupKey = KeyCode.E;

    [Header("Settings")]
    [SerializeField] private Weapon[] _weapons;
    public Weapon CurrentWeapon;
    private int _currentWeaponIndex = -1;

    private void Start()
    {
        EquipWeapon(0);
    }

    private void Update()
    {
        HandleWeaponSwitch();
        HandlePickup();
    }

    private void HandleWeaponSwitch()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && i < _weapons.Length)
            {
                EquipWeapon(i);
                break;
            }
        }
    }

    private void HandlePickup()
    {
        if (Input.GetKeyDown(_pickupKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 2f))
            {
                Weapon weapon = hit.collider.GetComponent<Weapon>();
                if (weapon != null)
                {
                    EquipWeapon(weapon);
                }
            }
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= _weapons.Length || _weapons[index] == null) return;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }

        _currentWeaponIndex = index;
        CurrentWeapon = _weapons[_currentWeaponIndex];
        CurrentWeapon.gameObject.SetActive(true);
    }

    private void EquipWeapon(Weapon weapon)
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }

        CurrentWeapon = weapon;
        CurrentWeapon.transform.SetParent(_weaponHolder);
        CurrentWeapon.transform.localPosition = Vector3.zero;
        CurrentWeapon.transform.localRotation = Quaternion.identity;
        CurrentWeapon.gameObject.SetActive(true);
    }
}