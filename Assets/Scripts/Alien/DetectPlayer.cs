﻿using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public abstract class DetectPlayer : MonoBehaviour
{

    #region Constants
    
    private const float ViewDistance = 7f;
    private const int ViewRange = 45;   // something between 0 and 180
    
    #endregion
    
    #region Variables
    
    protected GameObject Player;
    
    protected bool DetectedPlayer;
    private bool _check;
    
    #endregion
    
    private void Start()
    {
        
        #region Initialization
        
        GetComponent<CapsuleCollider2D>().size = new Vector2(ViewDistance / 2, 1f);
        Light2D coneLight = GetComponentsInChildren<Light2D>()[0];
        coneLight.pointLightOuterRadius = ViewDistance;
        coneLight.pointLightInnerAngle = 2 * ViewRange;
        coneLight.pointLightOuterAngle = 2 * ViewRange;
        
        #endregion

        SpecificStart();
        
    }

    private void Update()
    {
        
        #region Field of View
        
        if (_check && !DetectedPlayer)
        {
            
            Transform thisTransform = transform;
            Vector3 correctedPosition = thisTransform.position + new Vector3(0f, 0.38f, 0f);
            Quaternion rotation = thisTransform.rotation;
            
            Debug.DrawLine(correctedPosition, (Vector2) correctedPosition + Utils.AngleToVector(rotation.y == 0 ? -ViewRange : 180 + ViewRange) * ViewDistance, Color.yellow);
            Debug.DrawLine(correctedPosition, (Vector2) correctedPosition + Utils.AngleToVector(rotation.y == 0 ? 0 : 180) * ViewDistance, Color.yellow);
            Debug.DrawLine(correctedPosition, (Vector2) correctedPosition + Utils.AngleToVector(rotation.y == 0 ? ViewRange : 180 - ViewRange) * ViewDistance, Color.yellow);
        
            Vector2 alienToPlayerVector = ((Vector2) Player.transform.position - (Vector2) correctedPosition).normalized;
            double alienToPlayerAngle = Utils.VectorToAngle(alienToPlayerVector);
            bool looksRight = rotation.y == 0;
        
            if (looksRight ? (alienToPlayerAngle >= -ViewRange && alienToPlayerAngle <= ViewRange) : (alienToPlayerAngle >= 180 - ViewRange || alienToPlayerAngle <= - 180 + ViewRange))
            {
                Debug.DrawLine(correctedPosition, correctedPosition + (Vector3) alienToPlayerVector * ViewDistance, Color.red);
            
                RaycastHit2D hit = Physics2D.Raycast(correctedPosition, alienToPlayerVector, ViewDistance);
                if (hit && hit.collider.CompareTag("Player"))
                {
                    DetectAction();
                    DetectedPlayer = true;
                }
            }
            
        }
        
        #endregion
        
        SpecificUpdate();
        
    }
    
    #region Abstract Functions
    
    protected abstract void SpecificStart();
    
    protected abstract void SpecificUpdate();
    
    protected abstract void SpecificDetectAction();

    #endregion

    #region Helper Functions

    private void DetectAction()
    {
        
        GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
        Pathing pathing = GetComponent<Pathing>();
        if (pathing)
            pathing.enabled = false;
        Idle idle = GetComponent<Idle>();
        if (idle)
        {
            idle.StopAllCoroutines();
            idle.enabled = false;
        }
            
        SpecificDetectAction();
        
    }
    
    #endregion
    
    #region Event Functions
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.gameObject;
            _check = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!DetectedPlayer)
                Player = null;
            _check = false;
        }
    }
    
    #endregion
    
}
