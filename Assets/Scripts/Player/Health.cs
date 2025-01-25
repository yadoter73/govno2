using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;

    [SerializeField] private Ragdoll _ragdoll;
    [SerializeField] private Image _healthImage;

    public float CurrentHealth => _currentHealth;
    public bool IsAlive => _currentHealth > 0;

    public event Action OnHit;
    public event Action OnDie;
    
    private float HealthParts()
    {
        return _currentHealth / _maxHealth;
    }    
    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthImage.fillAmount = HealthParts();
        _ragdoll.DisableRagdoll();
        HealthParts();

        foreach (var rb in _ragdoll.rbs)
        {
            if (rb.gameObject.layer != 3)
            {
                HitBox hb = rb.gameObject.AddComponent<HitBox>();
                hb.Health = this;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        OnHit?.Invoke();
        _currentHealth -= amount;
        _healthImage.fillAmount = HealthParts();
        HealthParts();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return;

        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        _healthImage.fillAmount = HealthParts();
        HealthParts();
    }

    private void Die()
    {
        OnDie?.Invoke();
        _ragdoll.EnableRagdoll();
    }
}