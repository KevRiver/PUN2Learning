using System.Collections;
using Common.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Common.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Transform model;
        
        [SerializeField] private ParticleSystem hitParticle;

        private Rigidbody2D _rigidbody2D;

        private Collider2D _collider;

        private Character _shooter;

        private Vector2 _normalizedDirection = Vector2.zero;
        
#region Public Fields
        
        public float lifeTimeLimit = 2f;
        
        public float projectileSpeed = 10;
        
        public LayerMask ignoringLayerMask;

        public int damageOnHit = 10;
        
#endregion
        
        public void InitWithShooter(Character shooter)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.isKinematic = true;

            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
            
            SetShooter(shooter);
            AddIgnoringLayer(gameObject.layer);
            AddIgnoringLayer(shooter.gameObject.layer);
        }

        private void SetShooter(Character shooter)
        {
            _shooter = shooter;
        }

        private void AddIgnoringLayer(int ignoringLayer)
        {
            ignoringLayerMask |= 1 << ignoringLayer;
        }

        public void StartMove(Vector2 from, Vector2 to)
        {
            _normalizedDirection = (to - from).normalized;
            SetModelHeadDirection(from, to);
            StartCoroutine(ProjectileMove());
        }

        public void StartMoveTest()
        {
            Vector2 from = transform.position;
            Vector2 to = from + Vector2.right;
            _normalizedDirection = (to - from).normalized;
            SetModelHeadDirection(from, to);
            StartCoroutine(ProjectileMove());
        }

        private void SetModelHeadDirection(Vector2 from, Vector2 to)
        {
            Vector2 diff = to - from;
            float shootAngle = Vector2.Angle(Vector2.right, diff);
            if (diff.y < 0)
                shootAngle *= -1;
            model.Rotate(Vector3.forward, shootAngle);
        }

        public void StartMoveInDirectionTest()
        {
            _normalizedDirection = Vector2.right;
            StartCoroutine(ProjectileMove());
        }

        private IEnumerator ProjectileMove()
        {
            float livingTime = 0;
            while (livingTime < lifeTimeLimit)
            {
                livingTime += Time.deltaTime;
                transform.Translate(_normalizedDirection * (projectileSpeed * Time.deltaTime));

                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            StopCoroutine(ProjectileMove());
            if (IsIgnoringLayer(other.gameObject.layer))
                return;
            
            OnProjectileHit();
            
            Health health = other.gameObject.GetComponent<Health>();
            if (health is null) return;
            
            if(health.Owner.isLocalPlayer)
                health.Damage(damageOnHit);
        }

        private void OnProjectileHit()
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }

        private bool IsIgnoringLayer(int layer)
        {
            return ((1 << layer) & ignoringLayerMask) > 0;
        }
    }
}

