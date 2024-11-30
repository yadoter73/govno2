using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _camera;

    private WeaponManager _weaponManager;

    private void Awake()
    {
        _weaponManager = GetComponent<WeaponManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0))
        {
            Weapon currentWeapon = _weaponManager.CurrentWeapon;
            if (currentWeapon != null)
            {
                Vector3 cameraPoint = new Vector3(Screen.width / 2, Screen.height / 2);
                Ray ray = _camera.ScreenPointToRay(cameraPoint);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    Vector3 direction = (hit.point - currentWeapon.FirePoint.position).normalized;
                    currentWeapon.Shoot(_camera.ScreenToWorldPoint(cameraPoint), direction);
                }
                else
                {
                    Vector3 direction = _camera.transform.forward;
                    currentWeapon.Shoot(_camera.ScreenToWorldPoint(cameraPoint), direction);
                }
            }
        }
    }
}