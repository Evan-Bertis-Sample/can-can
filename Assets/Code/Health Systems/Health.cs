using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FormulaBoy.Utility;

namespace FormulaBoy.HealthSystems
{
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float HealthVal { get; set; }

        [field: SerializeField] private List<HurtBox> HurtBoxes = new List<HurtBox>();

        private int _invisibleSum = 0;
        public bool IsInvincible => _invisibleSum > 0;
        public delegate void OnInvincibilityHandler(bool isInvincible);
        public OnInvincibilityHandler OnInvincibility;

        public delegate void OnDamageTakenHandler(float damage);
        public OnDamageTakenHandler OnDamageTaken;

        public delegate void OnDamageTakenDetailedHandler(HitBox hitBox, float damage, bool isCrit);
        public OnDamageTakenDetailedHandler OnDamageTakenDetail;

        public delegate void OnDeathHandler();
        public OnDeathHandler OnDeath;

        public delegate void OnKnockbackHandler(Vector2 knockback);
        public OnKnockbackHandler OnKnockback;

        public enum HealthDelegateType
        {
            OnDamageTaken,
            OnDeath,
            OnKnockback,
            OnBecomeInvincibility,
            OnLoseInvincibility
        }

        // Start is called before the first frame update
        void Start()
        {
            HealthVal = MaxHealth;
            foreach (HurtBox hurtBox in HurtBoxes)
            {
                hurtBox.HurtBoxHealth = this;
            }

            // DamageNumbersManager.Instance.RegisterHealth(this);
        }

        void OnValidate()
        {
            HealthVal = MaxHealth;
            foreach (HurtBox hurtBox in HurtBoxes)
            {
                hurtBox.HurtBoxHealth = this;
            }
        }


        public void TakeDamage(HitBox hitBox)
        {
            Vector2 knockback = hitBox.GetKnockback(transform);
            bool isCrit = hitBox.IsCriticalHit;
            if (isCrit) knockback *= hitBox.CriticalHitMultiplier;
            OnKnockback?.Invoke(knockback);
            float damage = isCrit ? hitBox.Damage * hitBox.CriticalHitMultiplier : hitBox.Damage;

            HealthVal -= damage;
            if (HealthVal > MaxHealth)
            {
                HealthVal = MaxHealth;
            }
            OnDamageTaken?.Invoke(damage);
            OnDamageTakenDetail?.Invoke(hitBox, damage, isCrit);

            if (HealthVal <= 0f)
            {
                Die();
            }
        }

        public void TakeHit(HitBox hitBox, float invincibilityTime = 0f)
        {
            if (IsInvincible) return;
            // Grant invincibility after taking damage so that TakeDamage can check the old _invincible value
            TakeDamage(hitBox);
            RequestInvicibilityTime(invincibilityTime);
        }

        // Call this for invincibility, like the dash
        private void MakeInvincible(float time)
        {
            StartCoroutine(Invincibility(time));
        }

        // forceOverrideTime forces the invincibility time to be set, regardless of whether or not the health component is currently invincible
        public bool RequestInvicibilityTime(float seconds, bool forceOverideTime = false)
        {
            // I'm not sure what bool you want to return here. I'm guessing it's whether or not the player was invincible before the request.
            bool was_invincible = IsInvincible;
            if (forceOverideTime)
            {
                _invisibleSum = 0;
            }
            MakeInvincible(seconds);
            return was_invincible;
        }

        // * Don't use StopCoroutine. It will probably make the health invincible forever.
        private IEnumerator Invincibility(float time)
        {
            if (time <= 0f) yield break;

            if (_invisibleSum == 0) OnInvincibility?.Invoke(true);
            _invisibleSum++;

            yield return new WaitForSeconds(time);
            // If the health is no longer invincible, invoke the event with false
            // Needs to compare with 1 to avoid the case when the health was already vulnerable and made vulnerable again.
            if (_invisibleSum == 1) OnInvincibility?.Invoke(false);
            _invisibleSum = System.Math.Max(0, _invisibleSum - 1);

        }


        private void Die()
        {
            // for some reason this class overrides on destroy for enemy
            // so have to add kill count thing here as well
            // GameStateInfo.KillCount += 1;
            OnDeath?.Invoke();
        }

        void OnDrawGizmosSelected()
        {
            foreach (HurtBox hurtBox in HurtBoxes)
            {
                hurtBox.DrawGizmos();
            }
        }

        private void OnDisable()
        {
            // DamageNumbersManager.Instance.UnregisterHealth(this);
        }

    }
}