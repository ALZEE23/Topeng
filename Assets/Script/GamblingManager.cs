using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator enemyAnimator;
    public Animator playerAnimator;
    public bool isPlayerTurn = false;
    void Start()
    {
        // TurnOnGambling();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOnGambling()
    {
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

    public void OnMaskedUsed(bool OnOff)
    {
        Debug.Log("Masked Used in Gambling");
        if (isPlayerTurn)
        {
            playerAnimator.SetBool("isMasked", OnOff);
        }
        else
        {
            enemyAnimator.SetBool("isMasked", OnOff);
        }
    }

    public void OnShuffleDice(Animator animator)
    {
        animator.SetTrigger("isShuffle");
        Debug.Log("Dice Shuffled");
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
        OnMaskedUsed(false);
        TurnOnGambling();
    }

    public void SpawnDice()
    {
        Debug.Log("Dice Spawned");
    }
}
