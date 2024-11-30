using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private string _weaponName;
    [SerializeField] private int _maxAmmo = 30;
    [SerializeField] private float _damage = 10f;
    public float FireRate = 0.2f;

    [Header("Shooting Settings")]
    public Transform FirePoint;
    [SerializeField] private float _range = 50f;

    private int _currentAmmo;
    private bool _canShoot = true;

    private void Awake()
    {
        _currentAmmo = _maxAmmo;
    }

    public string WeaponName => _weaponName;
    public int CurrentAmmo => _currentAmmo;
    public int MaxAmmo => _maxAmmo;

    public void Shoot(Vector3 position, Vector3 direction)
    {
        if (!_canShoot || _currentAmmo <= 0) return;

        _currentAmmo--;
        PerformRaycast(position, direction);

        StartCoroutine(FireCooldown());
    }

    public void Reload()
    {
        _currentAmmo = _maxAmmo;
    }

    private void PerformRaycast(Vector3 position, Vector3 direction)
    {
        Ray ray = new Ray(position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, _range))
        {
            Debug.DrawLine(position, hit.point, Color.red, 1f);

            Health targetHealth = hit.collider.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(_damage);
            }
        }
    }

    private System.Collections.IEnumerator FireCooldown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(FireRate);
        _canShoot = true;
    }
}