using UnityEngine;

[System.Serializable]
public class JumpController
{
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -9.81f;

    public float Gravity => _gravity;

    public void HandleJump(ref Vector3 velocity, bool isGrounded)
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
}