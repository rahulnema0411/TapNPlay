using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public Telekinesis player;
    public List<Transform> checkPoints;
    public List<Enemy> enemies;
    public int currentLevel;
    public Canvas gameWonCanvas;

    private List<Vector3> checkkPointPos;

    private void Start() {
        currentLevel = 0;
        SetCheckpoints();
        InitiateGame();
        GameEventSystem.EventHandler += HandleGameEvents;
    }

    private void InitiateGame() {
        TakePlayerCurrentToLevel();
    }

    private void TakePlayerCurrentToLevel() {
        if(currentLevel >= 0) {
            player.transform.position = (checkkPointPos[currentLevel]);
            player.SetLineRenderer();
            if (currentLevel < checkkPointPos.Capacity - 1) {
                player.EnemyPos = checkkPointPos[currentLevel + 1];
            }
        }
        Debug.LogError(currentLevel);
        if(currentLevel == -1) {
            gameWonCanvas.gameObject.SetActive(true);
        }
    }

    private void SetCheckpoints() {
        checkkPointPos = new List<Vector3>();
        foreach (Transform t in checkPoints) {
            checkkPointPos.Add(t.position);
        }
    }

    private void OnEnemyKill() {
        currentLevel = FindMinEnemyLevel();
        TakePlayerCurrentToLevel();
    }

    private int FindMinEnemyLevel() {

        List<Enemy> aliveEnemies = enemies.FindAll(x => x.isKilled == false);

        int minLevel = -1;
        if(aliveEnemies.Capacity > 0) {
            minLevel = aliveEnemies[0].level;
            foreach(Enemy enemy in aliveEnemies) {
                if(enemy.level < minLevel) {
                    minLevel = enemy.level;
                }
            }
        }
        return minLevel;
    }

    private void HandleGameEvents(GAME_EVENT_TYPE type, System.Object data = null) {
        if(type == GAME_EVENT_TYPE.ENEMY_KILL) {
            OnEnemyKill();
        }
    }

    private void OnDestroy() {
        GameEventSystem.EventHandler -= HandleGameEvents;
    }
}