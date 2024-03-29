﻿using System.Collections;
using UnityEngine;

public class Bernd : Interactable
{
    
    public GameObject projectile;
    public MessageBox messageBox;
    public Sprite alive;
    public Sprite dead;
    public Sprite weapon;
    public Sprite playerLooksLeftSprite;
    public GameObject WeaponPos;
    public Transform PlayerPos;
    public static bool MoveToWeapon;

    private void Awake()
    {
        if (GameData.Instance.BerndDead)
        {
            gameObject.tag = "Untagged";
            gameObject.GetComponent<SpriteRenderer>().sprite = dead;
        }
    }

    protected override void SpecificAction()
    {
        WeaponPos.tag = "Interactable";
        UndoAction();
    }

    protected override void UndoSpecificAction()
    {
        messageBox.ShowMessages(Texts.BerndDialogueSpeakers, Texts.BerndDialogue);
        StartCoroutine(StartBernd());
    }

    protected override void SpecificUpdate()
    {
        if (MoveToWeapon)
        {
            Vector3 dir = WeaponPos.transform.position - gameObject.transform.position;
            dir = dir.normalized;
            Vector2 weapon = new Vector2(WeaponPos.transform.position.x, WeaponPos.transform.position.y);
            if (gameObject.transform.position.x < weapon.x && gameObject.transform.position.y > weapon.y)
            {
                gameObject.transform.Translate(dir * (4 * Time.deltaTime));
            }
        }
    }
    
    public void Kill()
    {
        GameData.Instance.BerndDead = true;
        gameObject.tag = "Untagged";
        gameObject.GetComponent<SpriteRenderer>().sprite = dead;
        PlayerMovement.CanMove = true;
        PlayerShoot.AllowInput = true;
        PlayerPos.gameObject.GetComponent<SpriteRenderer>().sprite = playerLooksLeftSprite;
        messageBox.ShowMonologue("Jordan", Texts.SurvivedBerndMonologue);
    }

    private IEnumerator StartBernd()
    {  
        yield return new WaitWhile(()=>messageBox.GetMessageActive());
        MoveToWeapon = true;
    }

    public IEnumerator KillPlayer()
    {
        GetComponent<SpriteRenderer>().sprite = weapon;
        messageBox.ShowMessages(Texts.KilledByBerndDialogueSpeakers, Texts.KilledByBerndDialogue);
        yield return new WaitWhile(() => messageBox.GetMessageActive());
        Vector2 projectileDir = PlayerPos.position - transform.position;
        float angle = Mathf.Acos(Vector2.Dot(projectileDir, Vector2.up) / projectileDir.magnitude) * Mathf.Rad2Deg;
        if (projectileDir.x > 0)
        {
            angle *= - 1;
        }
        GameObject temp = Instantiate(projectile, transform);
        temp.transform.Rotate(0f,0f,angle);
        temp.GetComponent<Rigidbody2D>().AddForce(projectileDir.normalized * 15, ForceMode2D.Impulse);
    }
    
    
    
}
