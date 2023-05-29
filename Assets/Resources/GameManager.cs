using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{

    public bool isEndRun = false;
    public bool isEndConquer = false;
    public GameState gameState = GameState.NotStart;
    [SerializeField] private GameObject[] dyingCharacters;
    
    [Header("Ending")]
    [SerializeField] private GameObject particleSystemObject; 
    [SerializeField] private GameObject ragdollEnemy; 
    
    #region instance
    // 매니저 싱글톤의  Prefab 경로
    private static readonly string managerPrefabPath = "Managers/GameManager";

    public static GameManager Instance;

    // 이미 싱글 톤이 존재한다면 그것을 돌려주고, 없는 경우 만든다.
    private static GameManager instance
    {
        get
        {
            if (Instance != null) return Instance;
            if (GameManager.instance == null)
            {
                var resource = Resources.Load(managerPrefabPath);
                Object.Instantiate(resource);
            }
            Instance = GameManager.instance;
            return Instance;
        }
    }
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
        // NotStart -> Run
        if (Input.GetMouseButtonDown(0))
            gameState = GameState.Run;
        
        // Run -> Battle
        if (isEndRun)
            gameState = GameState.Battle;
        
        // Battle -> Conquer
        if (IsAllDie())
            gameState = GameState.Conquer;
        
        // Conquer -> End
        if (isEndConquer)
        {
            gameState = GameState.Ended;
            StartCoroutine(Ending());
        }

    }
    
    bool IsAllDie()
    {
        for (int i=0; i<dyingCharacters.Length; i++)
        {
            if (dyingCharacters[i].activeSelf) return false;
        }

        return true;
    }
    
    IEnumerator Ending()
    {

        yield return new WaitForSeconds (2f);

        particleSystemObject.SetActive(true);

        yield return new WaitForSeconds (0.5f);

        ragdollEnemy.SetActive(true);
   
    }
}
