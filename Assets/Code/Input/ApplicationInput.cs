using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.InputManagement
{
    [RequireComponent(typeof(COMInputParser), typeof(StandardInputParser))]
    public class ApplicationInput : MonoBehaviour
    {
        [System.Serializable]
        public class PlayerInput
        {
            public int PlayerId;
            public float VerticalAxis;
            public float HorizontalAxis;
            public float RotationAxis;
            public byte ButtonBitmask;

            public PlayerInput()
            {
                PlayerId = -1;
                VerticalAxis = 0;
                HorizontalAxis = 0;
                RotationAxis = 0;
                ButtonBitmask = 0;
            }
        }

        public static ApplicationInput Instance { get; private set; }

        private COMInputParser _comInputParser;
        private StandardInputParser _standardInputParser;
        private int _maxPlayers = 8;
        private int[] _playerIds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _comInputParser = GetComponent<COMInputParser>();
            _standardInputParser = GetComponent<StandardInputParser>();
            _playerIds = new int[_maxPlayers];
        }

        public int ClaimNextPlayerId()
        {
            for (int i = 0; i < _playerIds.Length; i++)
            {
                if (_playerIds[i] == 0)
                {
                    _playerIds[i] = 1;
                    return i;
                }
            }
            return -1;
        }

        public void ReturnPlayerId(int playerId)
        {
            if (playerId >= 0 && playerId < _playerIds.Length)
            {
                _playerIds[playerId] = 0;
            }
        }

        public PlayerInput GetPlayerInput(int playerId)
        {
            if (playerId >= 0 && playerId < _playerIds.Length && _playerIds[playerId] == 1)
            {
                List<PlayerInput> playerInputs = new List<PlayerInput>();
                playerInputs.AddRange(_comInputParser.GetPlayerInputs());
                playerInputs.AddRange(_standardInputParser.GetPlayerInputs());

                foreach (PlayerInput playerInput in playerInputs)
                {
                    if (playerInput.PlayerId == playerId)
                    {
                        return playerInput;
                    }
                }
            }
            return null;
        }
    }
}