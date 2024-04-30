using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FormulaBoy.InputManagement;

namespace FormulaBoy.Player
{
    [RequireComponent(typeof(TankVisualsController), typeof(Rigidbody))]
    public class TankController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _rotationSpeed = 5;
        [SerializeField] private AnimationCurve _movementSpeedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _accelerationSpeed = 0.25f;

        [Header("Input Settings")]
        [SerializeField] private int _playerId = 0;
        private ApplicationInput.PlayerInput _playerInput;

        [Header("Visuals Settings")]
        [SerializeField] private TankVisualsController _tankVisualsController;

        // References
        private Rigidbody _rigidbody;

        // State Variables
        private float _timeMoving = 0;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _tankVisualsController ??= GetComponent<TankVisualsController>();
        }

        private void Update()
        {
            ReadInput();
            Move();
        }

        private void ReadInput()
        {
            _playerInput = ApplicationInput.Instance.GetPlayerInput(0);
        }

        private void Move()
        {
            if (_playerInput == null)
            {
                return;
            }

            Vector3 movement = new Vector3(_playerInput.HorizontalAxis, 0, _playerInput.VerticalAxis);
            
            if (movement.magnitude > 0)
            {
                _timeMoving += Time.deltaTime;
            }
            else
            {
                _timeMoving = 0;
            }

            float speed = _movementSpeedCurve.Evaluate(_timeMoving / _accelerationSpeed) * _movementSpeed;
            _rigidbody.velocity = movement.normalized * speed;

            _tankVisualsController.SetBodyVelocity(movement);

            _tankVisualsController.SetMovementSpeed(movement.magnitude);
            // _tankVisualsController.SetLookatPosition(transform.position + movement);
        }
    }
}
