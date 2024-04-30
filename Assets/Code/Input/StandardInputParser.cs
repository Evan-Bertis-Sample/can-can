using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerInput = FormulaBoy.InputManagement.ApplicationInput.PlayerInput;

namespace FormulaBoy.InputManagement
{
    public class StandardInputParser : MonoBehaviour
    {
        [SerializeField] private int _maxPlayers = 8;
        [SerializeField] private PlayerInput _kbmPlayerInput;

        private bool KBMPlayerActive => _kbmPlayerInput.PlayerId >= 0;

        private void Start()
        {
            _kbmPlayerInput = new PlayerInput();
        }

        private void Update()
        {
            ListenToConn();
            if (KBMPlayerActive)
            {
                ReadInput();
            }
        }

        private void ListenToConn()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!KBMPlayerActive)
                {
                    _kbmPlayerInput.PlayerId = ApplicationInput.Instance.ClaimNextPlayerId();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (KBMPlayerActive)
                {
                    ApplicationInput.Instance.ReturnPlayerId(_kbmPlayerInput.PlayerId);
                    _kbmPlayerInput.PlayerId = -1;
                }
            }
        }

        private void ReadInput()
        {
            _kbmPlayerInput.VerticalAxis = Input.GetAxisRaw("Vertical");
            _kbmPlayerInput.HorizontalAxis = Input.GetAxisRaw("Horizontal");
            int rotation = 0;
            if (Input.GetKey(KeyCode.Q))
            {
                rotation += -1;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotation += 1;
            }
            _kbmPlayerInput.RotationAxis = rotation;
            _kbmPlayerInput.ButtonBitmask = 0;
            if (Input.GetButton("Fire1"))
            {
                _kbmPlayerInput.ButtonBitmask |= 1;
            }
            if (Input.GetButton("Fire2"))
            {
                _kbmPlayerInput.ButtonBitmask |= 2;
            }
            if (Input.GetButton("Fire3"))
            {
                _kbmPlayerInput.ButtonBitmask |= 4;
            }
        }

        public List<PlayerInput> GetPlayerInputs()
        {
            if (KBMPlayerActive)
            {
                return new List<PlayerInput> { _kbmPlayerInput };
            }

            return new List<PlayerInput>();
        }
    }
}
