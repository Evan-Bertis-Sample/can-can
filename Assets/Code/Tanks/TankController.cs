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
        [SerializeField] private AnimationCurve _rotationSpeedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _accelerationRotation = 0.25f;

        [Header("Shooting Settings")]
        [SerializeField] private float _shootCooldown = 1;

        [Header("Special Action Settings")]
        [SerializeField] private float _specialCooldown = 1;

        [Header("Input Settings")]
        [SerializeField] private int _playerId = 0;
        private ApplicationInput.PlayerInput _playerInput;

        [Header("Visuals Settings")]
        [SerializeField] private TankVisualsController _tankVisualsController;

        // References
        private Rigidbody _rigidbody;

        // State Variables
        private float _timeMoving = 0;
        private float _timeRotating = 0;
        private bool _canShoot = true;
        private bool _canSpecial = true;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _tankVisualsController ??= GetComponent<TankVisualsController>();
        }

        private void Update()
        {
            ReadInput();
            if (_playerInput == null || _playerInput.PlayerId != _playerId)
            {
                return;
            }
            Move();
            PerformSpecialActions();
        }

        private void ReadInput()
        {
            _playerInput = ApplicationInput.Instance.GetPlayerInput(0);
        }

        private void Move()
        {
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

            if (_playerInput.RotationAxis != 0)
            {
                _timeRotating += Time.deltaTime;
            }
            else
            {
                _timeRotating = 0;
            }

            float rotation = _rotationSpeedCurve.Evaluate(_timeRotating / _accelerationRotation) * _rotationSpeed * _playerInput.RotationAxis;
            _tankVisualsController.RotateHead(rotation);

        }

        private void PerformSpecialActions()
        {
            if (_playerInput.ShootButton && _canShoot)
            {
                _canShoot = false;
                _tankVisualsController.PlayShootAnimation(0.1f, 0.1f, null, () => 
                {
                    // wait for the cooldown to finish
                    Invoke(nameof(ResetShoot), _shootCooldown);
                });
            }
            if (_playerInput.SpecialButton && _canSpecial)
            {
                _canSpecial = false;
                // perform special action
                _tankVisualsController.PlaySpecialActionAnimation(0.1f, 0.1f, null, () => 
                {
                    // wait for the cooldown to finish
                    Invoke(nameof(ResetSpecial), _specialCooldown);
                });
            }
        }

        private void ResetShoot()
        {
            _canShoot = true;
        }

        private void ResetSpecial()
        {
            _canSpecial = true;
        }
    }
}
