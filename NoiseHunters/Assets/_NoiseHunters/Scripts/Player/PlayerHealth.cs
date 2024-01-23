using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private ThirdPersonController playerController;

    public Action OnPlayerDeath;


    protected override void Awake()
    {
        base.Awake();

        playerController = GetComponent<ThirdPersonController>();
    }

    private void OnEnable()
    {
        OnEntityDeath += PlayerIsDead;
    }

    private void OnDisable()
    {
        OnEntityDeath -= PlayerIsDead;
    }


    private void PlayerIsDead(LivingEntity playerDied)
    {
        playerController.constrain_Move = true;
        playerController.constrain_Rotate = true;
        playerController.constrain_Aim = true;
        playerController.c_CharacterCombat.constrain_Attack = true;
        playerController.OnDisable();


        playerController.c_CharacterController.enabled = false;
        playerController.c_CharacterCapsuleCollider.enabled = false;
        playerController.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        playerController.c_CharacterAnimator.Death();

        StartCoroutine(DeathReset());
    }

    private IEnumerator DeathReset()
    {
        yield return new WaitForSecondsRealtime(2f);
        _LevelManager._Instance.ResetScene();
    }
}
