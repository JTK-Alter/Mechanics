using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputScript : MonoBehaviour
{
    public static PlayerInputScript Instance { get; private set; }
    public PlayerInput _playerInput;
    public PlayerLocomotion _playerLocomotion;

    private void Awake()
    {
        Instance = this;
        
        // Get the PlayerInput component
        _playerInput = GetComponent<PlayerInput>();
        _playerLocomotion = GetComponent<PlayerLocomotion>();

        // Register to input action events
        _playerInput.actions["Move"].started += OnMoveStarted;
        _playerInput.actions["Move"].performed += OnMovePerformed;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;

        _playerInput.actions["Jump"].started += OnJumpStarted;
        _playerInput.actions["Jump"].performed += OnJumpPerformed;
        _playerInput.actions["Jump"].canceled += OnJumpCanceled;
    }

    private void OnDestroy()
    {
        // Unregister from input action events
        _playerInput.actions["Move"].started -= OnMoveStarted;
        _playerInput.actions["Move"].performed -= OnMovePerformed;
        _playerInput.actions["Move"].canceled -= OnMoveCanceled;

        _playerInput.actions["Jump"].started -= OnJumpStarted;
        _playerInput.actions["Jump"].performed -= OnJumpPerformed;
        _playerInput.actions["Jump"].canceled -= OnJumpCanceled;
    }


    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        _playerLocomotion.inputDir = context.ReadValue<Vector2>();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _playerLocomotion.inputDir = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _playerLocomotion.inputDir = context.ReadValue<Vector2>();
    }
    

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        Debug.Log($"JUMP START");
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"JUMP PERFORMED");
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("CANCELED JUMP");
    }
}