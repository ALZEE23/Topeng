using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator enemyAnimator;
    public Animator playerAnimator;
    public bool isPlayerTurn = false;
    // public GameObject CameraGambling;
    public GameObject CameraPov;

    public bool isMaskedUsed = false;
    public bool isSpawnedDice = false;
    public bool isSpawnedEnemyDice = false;

    public Transform diceSpawnPoint;
    public Transform diceEnemySpawnPoint;

    public List<DiceObject> diceObjects;
    public int dificultyLevel = 1;

    public

    void Start()
    {
        // TurnOnGambling();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSpawnedEnemyDice == true)
        {
            OnMaskedUsed();
        }
    }

    public void TurnOnGambling()
    {
        isSpawnedDice = false;
        isSpawnedEnemyDice = false;
        Debug.Log("Gambling Turned On");
        if (isPlayerTurn)
        {
            OnShuffleDice(playerAnimator);
        }
        else
        {
            OnShuffleDice(enemyAnimator);
        }

    }

    public void OnMaskedUsed()
    {
        Debug.Log("Masked Used in Gambling");
        if (isPlayerTurn == true)
        {
            enemyAnimator.SetTrigger("UseMask");
        }
        else
        {
            playerAnimator.SetTrigger("UseMask");
            isMaskedUsed = true;
            CameraPov.SetActive(true);
        }
    }

    public void OnShuffleDice(Animator animator)
    {
        animator.SetTrigger("IsShuffle");

        StartCoroutine(WaitShuffleToSpawnDice());
        Debug.Log("Dice Shuffled");
    }

    public IEnumerator WaitShuffleToSpawnDice()
    {
        yield return new WaitForSeconds(2.5f);
        SpawnDice();
    }

    public void OnShowDiceResult()
    {
        Debug.Log("Dice Result Shown");
        if (isPlayerTurn == false)
        {
            isPlayerTurn = true;
        }
        else
        {
            isPlayerTurn = false;
        }
        // OnMaskedUsed();
        TurnOnGambling();
    }

    // player start gambling or guesing
    public void OnGambling()
    {
        Debug.Log("Gambling Started");
        // player 
        if (isPlayerTurn)
        {

        }
        else
        {

        }
    }

    public void OnUnmask()
    {
        Debug.Log("Unmasking");
        if (isPlayerTurn)
        {
            playerAnimator.SetTrigger("MaskOff");
            CameraPov.SetActive(false);
        }
        else
        {
            enemyAnimator.SetTrigger("MaskOff");
        }


        //testing 
        OnShowDiceResult();
    }

    public IEnumerator WaitUnmaskAfterSpawnDice()
    {
        yield return new WaitForSeconds(2.0f);
        OnUnmask();
    }

    // spawn prefab dice
    public void SpawnDice()
    {
        Debug.Log("Dice Spawned");
        if (isPlayerTurn)
        {
            isSpawnedDice = true;
            OnMaskedUsed();
            Debug.Log("Player Dice Spawned");
            //spawn logic here
            StartCoroutine(WaitUnmaskAfterSpawnDice());
        }
        else
        {
            isSpawnedEnemyDice = true;
            Debug.Log("Enemy Dice Spawned");
            StartCoroutine(WaitUnmaskAfterSpawnDice());
            //spawn logic here
        }
    }

}

[System.Serializable]
public class DiceObject
{
    public int diceValue;
    public GameObject diceGameObject;
}
