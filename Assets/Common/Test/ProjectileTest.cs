using System;
using System.Collections;
using System.Collections.Generic;
using Common.Weapon;
using Demo.Scripts.Manager;
using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Projectile projectilePrefab;

    private void Update()
    {
        if (inputManager.CurrentAimInputState.Equals(VirtualInputState.Up))
        {
            GameObject instance = Instantiate(projectilePrefab.gameObject, transform.position, Quaternion.identity);
            Projectile projectile = instance.GetComponent<Projectile>();

            if (projectile == null) return;

            var position = transform.position;
            Vector2 from = new Vector2(position.x, position.y);
            Vector2 to = inputManager.MousePositionWorld;
            
            projectile.StartMove(from, to);
            
            inputManager.ResetAimInput();
        }
    }
}
