using System.Collections.Generic;
using Common.Weapon;
using Demo.Scripts.Manager;
using Photon.Pun;
using UnityEngine;

namespace Common.CharacterAbility
{
    public class ShootAbility : AbilityBase
    {
        [SerializeField] 
        private List<GameObject> projectilePrefabs;

        public override void HandleInput()
        {
            if (_inputManager.CurrentAimInputState.Equals(VirtualInputState.Up))
            {
                Vector3 from = transform.position;
                Vector3 to = (Vector3)_inputManager.MousePositionWorld;
                
                int toShoot = GetRandomProjectileIndex();

                object[] shootParams = {from, to, toShoot};

                _owner.photonView.RPC("RPC_Shoot", RpcTarget.AllBuffered, shootParams);
            
                _inputManager.ResetAimInput();
            }
        }
        
        private int GetRandomProjectileIndex()
        {
            return Random.Range(0, projectilePrefabs.Count);
        }

        [PunRPC]
        private void RPC_Shoot(Vector3 from, Vector3 to, int toShoot)
        {
            GameObject instance = Instantiate(projectilePrefabs[toShoot].gameObject, from, Quaternion.identity);
            Projectile projectile = instance.GetComponent<Projectile>();

            if (projectile == null) return;
                
            projectile.InitWithShooter(_owner);
            
            projectile.StartMove(from, to);
        }
    }
}

