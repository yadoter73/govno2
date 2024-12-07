using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class WeaponController : MonoBehaviour
{
    [Header("References")]

    private WeaponManager _weaponManager;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1"))
        {
            Weapon currentWeapon = _weaponManager.CurrentWeapon;
            if (currentWeapon != null)
            {
                Vector3 cameraPoint = new Vector3(Screen.width / 2, Screen.height / 2);
                Ray ray = Camera.main.ScreenPointToRay(cameraPoint);
                if (Physics.Raycast(ray, out RaycastHit hit, currentWeapon.Range))
                {
                    Vector3 direction = (hit.point - ray.origin).normalized;
                    currentWeapon.Shoot(Camera.main.ScreenToWorldPoint(cameraPoint), direction);
                }
                else
                {
                    currentWeapon.Shoot(Camera.main.ScreenToWorldPoint(cameraPoint), ray.direction);
                }
            }
        }
        if (Input.GetButton("Reload"))
        {
            StartCoroutine(_weaponManager.CurrentWeapon.Reload());
        }
    }
}