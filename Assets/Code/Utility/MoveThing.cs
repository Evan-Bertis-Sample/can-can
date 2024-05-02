using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FormulaBoy.Utility
{
    public static class MoveThing
    {
        // I wanted to move everything in the same way.
        // If this way of moving things doesn't work, we can change it here.
        // I'm concerned that Copilot keeps wantint to multiply acceleration by Time.deltaTime.
        public static void Move(Rigidbody2D rb, Vector2 velocity, float acceleration)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, velocity, acceleration);
        }

        public static void MoveTowardsUntilClose(Rigidbody2D rb, Vector2 target, float speed, float acceleration, float closeDistance, float slowAcceleration){
            Vector2 direction = target - (Vector2)rb.transform.position;
            if (direction.sqrMagnitude < closeDistance * closeDistance){
                Move(rb, Vector2.zero, slowAcceleration);
            } else {
                Move(rb, speed * direction.normalized, acceleration);
            }
        }

        public static IEnumerator TryToMoveThisWayForTime(Rigidbody2D rb, Vector2 v, float acceleration, float time){
            float t = 0;
            while(t < time){
                t += Time.deltaTime;
                MoveThing.Move(rb, v, acceleration);
                yield return new WaitForEndOfFrame();
            }
        }

        public static IEnumerator TryToStayInPlace(Rigidbody2D rb, float acceleration, float time){
            yield return TryToMoveThisWayForTime(rb, Vector2.zero, acceleration, time);
        }
            
    }
}