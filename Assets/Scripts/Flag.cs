using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    public Image flagImage1;
    public Image flagImage2;
    public float flagMoveDuration = 1f;
    public float flagWaitDuration = 2f;
    
    private bool isFlag1Down = true;

    private void Update()
    {
        // 코루틴 시작
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            StartCoroutine(FlagAnimationCoroutine());
        }
    }
    
    private IEnumerator FlagAnimationCoroutine()
    {
        while (true)
        {
            if (isFlag1Down)
            {
                // 깃발 1 내려가기
                yield return MoveFlag(flagImage1, new Vector2(0f, -10f));
                isFlag1Down = false;
            }
            else
            {
                // 깃발 2 올라오기
                yield return MoveFlag(flagImage2, new Vector2(0f, 0f));
            }
        }
    }

    private IEnumerator MoveFlag(Image flagImage, Vector2 targetPosition)
    {
        Vector2 startPosition = flagImage.rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < flagMoveDuration)
        {
            // 깃발 위치 이동
            float t = elapsedTime / flagMoveDuration;
            flagImage.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // 깃발 위치 최종 설정
        flagImage.rectTransform.anchoredPosition = targetPosition;
    }
}
