using UnityEngine;

[System.Serializable]
public class MovementController
{
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _airControlMultiplier = 0.5f;

    private CharacterController _characterController;

    private Transform _head;

    public void Initialize(CharacterController characterController)
    {
        _characterController = characterController;
        _head = Camera.main.transform;
    }

    public void Move(bool isGrounded)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = _head.right * horizontal + _head.forward * vertical;

        float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;

        if (!isGrounded)
        {
            speed *= _airControlMultiplier;
        }

        _characterController.Move(moveDirection * speed * Time.deltaTime);
    }
}