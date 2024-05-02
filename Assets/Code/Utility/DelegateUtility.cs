using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.Utility
{
    public static class DelegateUtility
    {
        public static void UnsubscribeAllListeners(Delegate del)
        {
            if (del == null)
            {
                return;
            }

            var invocationList = del.GetInvocationList();
            foreach (var handler in invocationList)
            {
                del = Delegate.Remove(del, handler);
            }
        }
    }

}