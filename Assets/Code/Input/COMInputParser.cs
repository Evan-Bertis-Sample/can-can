using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

namespace FormulaBoy.Input
{
    public class COMInputParser : MonoBehaviour
    {
        [Serializable]
        public struct PlayerInput
        {
            public int PlayerId;
            public float VerticalAxis;
            public float HorizontalAxis;
            public float RotationAxis;
            public byte ButtonBitmask;
        }

        [SerializeField] private int _baudRate = 9600;
        [SerializeField ]private string _portName = "COM3";
        [SerializeField] private int _maxPlayers = 8;

        private SerialPort _port;
        [SerializeField] private PlayerInput[] _playerInputs;

        private void Start()
        {
            _port = new SerialPort(_portName, _baudRate);
            _port.Open();
            _port.NewLine = "\n";
            _playerInputs = new PlayerInput[_maxPlayers];

            // open a new thread to read the serial port
            Thread thread = new Thread(ReadSerialPort);
            thread.IsBackground = false;
            thread.Start();
        }

        private void ReadSerialPort()
        {
            while (_port.IsOpen)
            {
                Debug.Log("Reading from serial port");
                try
                {
                    string message = _port.ReadLine();
                    // Debug.Log(message);

                    // the messages we see look like this:
                    // Player ID: 0,Vertical Axis: -0.380000,Horizontal Axis: -0.260000,Rotation Axis: 0.700000,Button Bitmask: 103
                    string[] parts = message.Split(',');

                    if (parts.Length != 5)
                    {
                        // Debug.LogError("Invalid message: " + message);
                        continue;
                    }

                    PlayerInput playerInput = new PlayerInput();

                    foreach (string part in parts)
                    {
                        string[] keyValue = part.Split(':');
                        if (keyValue.Length != 2) continue;

                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();

                        switch (key)
                        {
                            case "Player ID":
                                playerInput.PlayerId = int.Parse(value);
                                break;
                            case "Vertical Axis":
                                playerInput.VerticalAxis = float.Parse(value);
                                break;
                            case "Horizontal Axis":
                                playerInput.HorizontalAxis = float.Parse(value);
                                break;
                            case "Rotation Axis":
                                playerInput.RotationAxis = float.Parse(value);
                                break;
                            case "Button Bitmask":
                                playerInput.ButtonBitmask = byte.Parse(value);
                                break;
                        }
                    }

                    if (playerInput.PlayerId >= 0 && playerInput.PlayerId < _maxPlayers)
                    {
                        Debug.Log("Setting player input for player " + playerInput.PlayerId);
                        _playerInputs[playerInput.PlayerId] = playerInput;
                    }

                }
                catch (TimeoutException) {
                    Debug.Log("Timeout exception");

                }
            }
        }
    }
}