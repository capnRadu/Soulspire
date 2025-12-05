using System.Collections;
using UnityEngine;

public class CameraMenuTransition : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform targetCameraPivot;
    [SerializeField] private Camera targetCamera;

    public float transitionDuration = 1.0f;
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 gamePos;
    private Quaternion gameRot;
    private float gameSize;

    private Coroutine currentTransition;
    private int targetState = 0;

    private void Start()
    {
        gamePos = cameraPivot.position;
        gameRot = cameraPivot.rotation;
        gameSize = mainCamera.orthographicSize;
        
        targetState = 0;
    }

    public void OpenMenuCamera()
    {
        if (targetState == 1)
        {
            return;
        }

        targetState = 1;

        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }

        currentTransition = StartCoroutine(AnimateCamera(targetCameraPivot.position, targetCameraPivot.rotation, targetCamera.orthographicSize));
    }

    public void CloseMenuCamera()
    {
        if (targetState == 0)
        {
            return;
        }

        targetState = 0;

        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }

        currentTransition = StartCoroutine(AnimateCamera(gamePos, gameRot, gameSize));
    }

    private IEnumerator AnimateCamera(Vector3 targetPos, Quaternion targetRot, float targetSize)
    {
        float timeElapsed = 0f;

        cameraPivot.GetPositionAndRotation(out Vector3 startPos, out Quaternion startRot);
        float startSize = mainCamera.orthographicSize;

        while (timeElapsed < transitionDuration)
        {
            float t = timeElapsed / transitionDuration;

            float smoothT = transitionCurve.Evaluate(t);

            cameraPivot.SetPositionAndRotation(Vector3.Lerp(startPos, targetPos, smoothT), Quaternion.Lerp(startRot, targetRot, smoothT));
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, smoothT);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        cameraPivot.SetPositionAndRotation(targetPos, targetRot);
        mainCamera.orthographicSize = targetSize;

        currentTransition = null;
    }
}