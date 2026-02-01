using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GamblingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator enemyAnimator;
    public Animator playerAnimator;
    public Animator cowAnimator;
    public bool isPlayerTurn = false;
    // public GameObject CameraGambling;
    public GameObject CameraPov;

    public bool isMaskedUsed = false;
    public bool isSpawnedDice = false;
    public bool isSpawnedEnemyDice = false;

    public Transform diceSpawnPoint;
    public Transform diceEnemySpawnPoint;

    public List<DiceObject> diceObjects;
    public int diceObjectsPlayerCount;
    public int diceObjectsEnemyCount;

    public GameObject maskParent;
    public GameObject maskEnemyParent;

    public List<GameObject> maskObjectsPlayer;
    public List<GameObject> maskObjectsEnemy;

    public bool isWaitingPlayerToGuess = false;
    public bool isWaitingEnemyToGuess = false;
    public bool isGameOver = false;
    public bool isWinner = false;

    public GameObject LosePanel;
    public GameObject WinPanel;

    void Start()
    {
        TurnOnGambling();
        CheckMaskPlayer();
        CheckMaskEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isSpawnedEnemyDice == true)
        {
            OnMaskedUsedPlayer();
        }

        EndGame();
    }

    void SwitchTurn()
    {
        if (isGameOver || isWinner) return;
        isPlayerTurn = !isPlayerTurn;

        if (isPlayerTurn)
            OnShuffleDice(playerAnimator);
        else
            OnShuffleDice(enemyAnimator);
    }

    public void EndGame()
    {
        if (maskObjectsPlayer.Count <= 0)
        {
            Debug.Log("Enemy Win");
            isGameOver = true;
            PlayAnimationEnd(playerAnimator);
            cowAnimator.SetTrigger("Moo");
            LosePanel.SetActive(true);
        }

        if (maskObjectsEnemy.Count <= 0)
        {
            Debug.Log("Player Win");
            isWinner = true;
            PlayAnimationEnd(enemyAnimator);
            WinPanel.SetActive(true);
        }
    }

    public void PlayAnimationEnd(Animator animator)
    {
        animator.SetTrigger("HeadShake");
    }


    public void TurnOnGambling()
    {
        if (isGameOver || isWinner) return;
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

    public void OnMaskedUsedPlayer()
    {
        playerAnimator.SetTrigger("UseMask");
        isMaskedUsed = true;
        CameraPov.SetActive(true);
    }

    public void OnMaskedUsedEnemy()
    {
        enemyAnimator.SetTrigger("UseMask");
    }

    public void OnShuffleDice(Animator animator)
    {
        if (isGameOver || isWinner) return;
        animator.SetTrigger("IsShuffle");

        StartCoroutine(WaitShuffleToSpawnDice());
        Debug.Log("Dice Shuffled");
    }

    public IEnumerator WaitShuffleToSpawnDice()
    {
        yield return new WaitForSeconds(3.5f);
        if (isGameOver || isWinner) yield break;
        SpawnDice();
    }

    public void ButtonActionOver()
    {
        if (isGameOver || isWinner) return;
        if (isMaskedUsed)
        {
            OnUnmaskPlayer();
        }
        if (diceObjectsEnemyCount > 5)
        {
            SwitchTurn();
        }
        else
        {
            OnShuffleDice(enemyAnimator);
            ReduceMaskPlayer();
        }
    }

    public void ButtonActionFit()
    {
        if (isGameOver || isWinner) return;
        if (isMaskedUsed)
        {
            OnUnmaskPlayer();
        }
        if (diceObjectsEnemyCount == 5)
        {
            SwitchTurn();
        }
        else
        {
            OnShuffleDice(enemyAnimator);
            ReduceMaskPlayer();
        }
    }

    public void ButtonActionUnder()
    {
        if (isGameOver || isWinner) return;
        if (isMaskedUsed)
        {
            OnUnmaskPlayer();
        }
        if (diceObjectsEnemyCount < 5)
        {
            SwitchTurn();
        }
        else
        {
            OnShuffleDice(enemyAnimator);
            ReduceMaskPlayer();
        }
    }

    public void EnemyGuess()
    {
        if (isGameOver || isWinner) return;
        int guess = UnityEngine.Random.Range(0, 3);

        bool correct = false;

        if (guess == 0 && diceObjectsPlayerCount > 7) correct = true;
        if (guess == 1 && diceObjectsPlayerCount == 7) correct = true;
        if (guess == 2 && diceObjectsPlayerCount < 7) correct = true;

        if (correct)
        {
            SwitchTurn();
        }
        else
        {
            StartCoroutine(EnemyLoseTurn());
            ReduceMaskEnemy();
        }

    }


    public void CheckMaskPlayer()
    {
        foreach (Transform child in maskParent.transform)
        {
            maskObjectsPlayer.Add(child.gameObject);
        }
    }

    public void CheckMaskEnemy()
    {
        foreach (Transform child in maskEnemyParent.transform)
        {
            maskObjectsEnemy.Add(child.gameObject);
        }
    }

    public void ReduceMaskPlayer()
    {
        if (maskObjectsPlayer.Count > 0)
        {
            GameObject maskToRemove = maskObjectsPlayer[maskObjectsPlayer.Count - 1];
            maskObjectsPlayer.RemoveAt(maskObjectsPlayer.Count - 1);
            Destroy(maskToRemove);
        }
    }

    public void ReduceMaskEnemy()
    {
        if (maskObjectsEnemy.Count > 0)
        {
            GameObject maskToRemove = maskObjectsEnemy[maskObjectsEnemy.Count - 1];
            maskObjectsEnemy.RemoveAt(maskObjectsEnemy.Count - 1);
            Destroy(maskToRemove);
        }
    }

    public void OnUnmaskEnemy()
    {
        enemyAnimator.SetTrigger("MaskOff");
        isPlayerTurn = true;
    }

    public IEnumerator EnemyLoseTurn()
    {
        enemyAnimator.SetTrigger("MaskOff");
        yield return new WaitForSeconds(1.5f); // waktu animasi
        if (isGameOver || isWinner) yield break; // Stop coroutine
        SwitchTurn();
    }


    public void OnUnmaskPlayer()
    {
        playerAnimator.SetTrigger("MaskOff");
        OnShuffleDice(enemyAnimator);
    }

    public void OnOpenMaskPlayer()
    {
        playerAnimator.SetTrigger("OpenMask");
        CameraPov.SetActive(false);
        isMaskedUsed = false;

    }

    public void OnOpenMaskEnemy()
    {
        enemyAnimator.SetTrigger("OpenMask");
        EnemyGuess();
    }

    public void EnemyHitPlayer()
    {
        Debug.Log("Enemy Hits Player!");
        ReduceMaskPlayer();
        enemyAnimator.SetTrigger("HitPlayer");
    }

    public IEnumerator WaitOpenMaskPlayerAfterSpawnDice()
    {
        yield return new WaitForSeconds(0.18f);
        if (isGameOver || isWinner) yield break; // Stop coroutine
        if (!isMaskedUsed)
        {
            EnemyHitPlayer();
            yield break;
        }
        yield return new WaitForSeconds(1.5f); // tunggu animasi hit selesai
        OnOpenMaskPlayer();
        isWaitingPlayerToGuess = true;
    }

    public IEnumerator WaitOpenMaskEnemyAfterSpawnDice()
    {
        yield return new WaitForSeconds(2.0f);
        if (isGameOver || isWinner) yield break; // Stop coroutine
        OnOpenMaskEnemy();
        isWaitingEnemyToGuess = true;
    }

    // spawn prefab dice
    public async Task SpawnDice()
    {
        if (isGameOver || isWinner) return;
        Debug.Log("Dice Spawned");

        // Random pilih 1-3 DiceObject
        int numDice = UnityEngine.Random.Range(1, 4); // 1, 2, or 3
        int totalCount = 0;

        if (isPlayerTurn)
        {
            isSpawnedDice = true;
            OnMaskedUsedEnemy();

            // await Task.Delay(1000); // delay sedikit biar animasi mask enemy keliatan

            // Spawn dan hitung total
            for (int i = 0; i < numDice; i++)
            {
                DiceObject selectedDice = diceObjects[UnityEngine.Random.Range(0, diceObjects.Count)];
                totalCount += selectedDice.diceValue; // Jumlahkan diceValue

                // SPAWN DENGAN RANDOM OFFSET KECIL - BIAR GAK TEMBUS
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-0.05f, 0.05f),
                    UnityEngine.Random.Range(0f, 0.05f),
                    UnityEngine.Random.Range(-0.05f, 0.05f)
                );
                Vector3 spawnPos = diceSpawnPoint.position + randomOffset;
                GameObject spawnedDice = Instantiate(selectedDice.diceGameObject, spawnPos, diceSpawnPoint.rotation);

                // Freeze position agar tidak kepental
                Rigidbody rb = spawnedDice.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Matikan physics
                }

                // Auto disable setelah 2 detik
                StartCoroutine(DisableDiceAfterDelay(spawnedDice, 2.0f));
            }

            diceObjectsPlayerCount = totalCount; // Simpan total
            Debug.Log($"Player Dice Spawned - Total Count: {diceObjectsPlayerCount}");
            StartCoroutine(WaitOpenMaskEnemyAfterSpawnDice());
        }
        else
        {
            isSpawnedEnemyDice = true;

            await Task.Delay(800);

            // Spawn dan hitung total
            for (int i = 0; i < numDice; i++)
            {
                DiceObject selectedDice = diceObjects[UnityEngine.Random.Range(0, diceObjects.Count)];
                totalCount += selectedDice.diceValue; // Jumlahkan diceValue

                // SPAWN DENGAN RANDOM OFFSET KECIL - BIAR GAK TEMBUS
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-0.05f, 0.05f),
                    UnityEngine.Random.Range(0f, 0.05f),
                    UnityEngine.Random.Range(-0.05f, 0.05f)
                );
                Vector3 spawnPos = diceEnemySpawnPoint.position + randomOffset;
                GameObject spawnedDice = Instantiate(selectedDice.diceGameObject, spawnPos, diceEnemySpawnPoint.rotation);

                // Freeze position agar tidak kepental
                Rigidbody rb = spawnedDice.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true; // Matikan physics
                }

                // Auto disable setelah 2 detik
                StartCoroutine(DisableDiceAfterDelay(spawnedDice, 2.0f));
            }

            diceObjectsEnemyCount = totalCount; // Simpan total
            Debug.Log($"Enemy Dice Spawned - Total Count: {diceObjectsEnemyCount}");
            StartCoroutine(WaitOpenMaskPlayerAfterSpawnDice());
        }
    }

    private IEnumerator DisableDiceAfterDelay(GameObject dice, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (dice != null)
        {
            dice.SetActive(false);
        }
    }

}

[System.Serializable]
public class DiceObject
{
    public int diceValue;
    public GameObject diceGameObject;
}

public enum AnimationParameter
{
    IsShuffle,
    UseMask,
    MaskOff,
    OpenMask
}
