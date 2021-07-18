using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroyParticle : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private float _lifeTime = 0f;
        private float _timer = 0f;
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _lifeTime = _particleSystem.main.startLifetime.constant;
        }

        private void Update()
        {
            if (_timer < _lifeTime)
            {
                _timer += Time.deltaTime;
                return;
            }
        
            Destroy(gameObject);
        }
    }
}
