using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.Misc
{
    public class CopyLocalRotation : MonoBehaviour
    {
        [SerializeField] private Transform _source;
        [SerializeField] private Transform _target;

        private void Update()
        {
            _target.localRotation = _source.localRotation;
        }
    }
}