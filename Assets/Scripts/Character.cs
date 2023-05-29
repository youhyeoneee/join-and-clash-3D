using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    public bool isBlue = false;
    
    [Header("Animation")]
    public Animator anim;
    [SerializeField] private float      animationOffset;

    [Header("Head & Body")]
    [SerializeField] private SkinnedMeshRenderer head;
    [SerializeField] private SkinnedMeshRenderer body;
    
    [Header("Die Effect")]
    [SerializeField] private GameObject _effectObj;
    [SerializeField] private bool isDie = false;
    [SerializeField] private float delayTime = 3f;
    
    [Header("Material")]
    [SerializeField] private Material   blue;

    [Header("Rotation")]
    [SerializeField] private float      rotaionSpeed            = 10f;
    [SerializeField] private float      rotationSmoothness      = 20f;
    private Vector3     targetRotation; // 드래그 입력에 따라 조정되는 목표 회전값
    private float       rotationRange = 25f;
    private float       minRotation;
    private float       maxRotation;
    
    [Header("NavMesh")]
    [SerializeField] protected Transform[] targets;
    private NavMeshAgent agent;
    [SerializeField] private TagType enemy = TagType.Enemy;

    private GameState gameState;

    protected bool isBattleStart = false;
    private bool isDance = false;

    protected virtual void Start()
    {
        targetRotation = transform.rotation.eulerAngles;
        minRotation = targetRotation.y - rotationRange;
        maxRotation = targetRotation.y + rotationRange;
        
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        gameState = GameManager.Instance.gameState;

        if (isBlue)
        {
            switch (gameState)
            {
                case GameState.Run:
                    if (Input.GetMouseButtonDown(0))
                    {
                        animationOffset = Random.Range(0f, 1f);
                        anim.SetFloat(AnimType.offset.ToString(), animationOffset);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        anim.SetBool(AnimType.run.ToString(), false);
                    }

                    if (Input.GetMouseButton(0))
                    {
                        anim.SetBool(AnimType.run.ToString(), true);
                        Rotate();
                    }
                    break;
                case GameState.Battle:
                    EnableNav(targets[0].position);
                    if (!isBattleStart)
                    {
                        anim.SetBool(AnimType.ready.ToString(), true);
                        anim.SetBool(AnimType.run.ToString(), false);
                    }
                    break;
                case GameState.Conquer:
                    EnableNav(targets[1].position);
                    anim.SetBool(AnimType.run.ToString(), true);
                    break;
                case GameState.Ended:
                    if (!isDance)
                    {
                        agent.radius = 1f;
                        StartCoroutine(RotateCharacterSmoothly());
                        anim.SetBool(AnimType.dance.ToString(), true);
                        isDance = true;
                        DisableNav();

                    }
                   
                    break;
            }
        }
    }


    void Rotate()
    {
        // 회전 
        float rotateY = Input.GetAxis("Mouse X") * rotaionSpeed;
        targetRotation.y = Mathf.Clamp(targetRotation.y + rotateY, minRotation, maxRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * rotationSmoothness);
    }
    
    IEnumerator RotateCharacterSmoothly()
    {
        yield return new WaitForSeconds(0.5f);

        Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotaionSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
    public void ChangeToPlayer()
    {
        head.material = blue;
        body.material = blue;
        gameObject.tag = TagType.Player.ToString();
        isBlue = true;
    }



    public IEnumerator Die()
    {
        DisableNav();
        transform.SetParent(null);
        _effectObj.SetActive(true);
        _effectObj.transform.SetParent(null);
       
        yield return new WaitForSeconds(0.4f);
        
        head.enabled = false;
        body.enabled = false;
        gameObject.SetActive(false);
    }
    
    public void DisableNav()
    {
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.velocity = Vector3.zero;
    }

    protected void EnableNav(Vector3 pos)
    {
        agent.SetDestination(pos);
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }
    

    protected virtual void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if (hitObject.CompareTag(enemy.ToString()) && !isBattleStart)
        {
            isBattleStart = true;
            anim.SetBool(AnimType.ready.ToString(), false);
            StartCoroutine(IncreaseRadius());
            DisableNav();
        }
    }

    IEnumerator IncreaseRadius()
    {
        float elapsedTime = 0f;
        float startRadius = agent.radius;
        float currentRadius = startRadius;
        float duration = 1f;
        float targetRadius = 1.5f;

        while (elapsedTime < duration && currentRadius < targetRadius)
        {
            elapsedTime += Time.deltaTime;

            // 증가 비율을 계산
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 서서히 반경 값을 증가시킴
            currentRadius = Mathf.Lerp(startRadius, targetRadius, t);
            agent.radius = currentRadius;

            yield return null;
        }

        // 최종 반경 값을 설정
        agent.radius = targetRadius;
        
        yield return new WaitForSeconds(delayTime);

        if (isDie)
            StartCoroutine(Die());
        else
            DisableNav();
    }

}
