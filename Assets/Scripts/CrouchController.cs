using UnityEngine;

[System.Serializable]
public class CrouchController
{
    [SerializeField] private float _crouchHeight = 1f;
    [SerializeField] private float _standingHeight = 2f;
    [SerializeField] private float _crouchTransitionSpeed = 5f;

    private CharacterController _characterController;
    private bool _isCrouching;

    public void Initialize(CharacterController characterController)
    {
        _characterController = characterController;
    }

    public void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isCrouching = false;
        }

        float targetHeight = _isCrouching ? _crouchHeight : _standingHeight;
        _characterController.height = Mathf.Lerp(_characterController.height, targetHeight, Time.deltaTime * _crouchTransitionSpeed);
    }
}