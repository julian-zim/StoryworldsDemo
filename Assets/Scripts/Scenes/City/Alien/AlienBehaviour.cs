﻿using UnityEngine;

public class AlienBehaviour : MonoBehaviour
{
    
    #region Variables
    
    // Unity variables
    [SerializeField] private Collider2D innerCollider;
    [SerializeField] private int id;

    #endregion

    private void Start()
    {
        
        #region Initialization

        if (id >= 0 && GameData.Instance.DeadAliens[id])
            Destroy(gameObject);
        
        #endregion

    }
    
    #region Helper Functions

    private void MakeDead()
    {
        // TODO NTH: death animation
        Destroy(gameObject);
    }
    
    #endregion

    #region Public Functions

    public void Die()
    {
        if (id >= 0)
            GameData.Instance.DeadAliens[id] = true;
        MakeDead();
    }
    
    public bool CompareInnerCollider(Collider2D otherCollider)
    {
        return otherCollider.Equals(innerCollider);
    }

    #endregion
    
}
