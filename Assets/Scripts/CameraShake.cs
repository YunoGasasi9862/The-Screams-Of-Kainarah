
using PlayerAnimationHandler;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    private PlayerAttackEnum.PlayerAttackSlash currentAttackState; 
    private Vector3 _cameraOldPosition;
    private CancellationToken _token;

    private void Start()
    {
        _token = new CancellationToken();
    }

    void Update()
    {
        RunAsyncCoroutineWaitForSeconds.RunTheAsyncCoroutine(shakeCamera(_mainCamera, .1f), _token);
    }

    private async IAsyncEnumerator<WaitForSeconds> shakeCamera(Camera _mainCamera, float timeForCameraShake) //do it tomorrow
    {
        float timeSpent = 0f;

        _cameraOldPosition = _mainCamera.transform.position;

        while (timeSpent < timeForCameraShake)
        {
            float randomXThrust = Random.Range(-1f, 1f);  //i didn't know this work too, i was using adding to x and y, this will simply shake/translate the camera up and down
            //only and only if there's a camera holder to keep the camera in place
            float randomYThrust = Random.Range(-1f, 1f);

            _mainCamera.transform.position = _cameraOldPosition + new Vector3(randomXThrust, randomYThrust, 0); //to avoid effecting the z-index

            timeSpent += Time.deltaTime;

            await Task.Delay(System.TimeSpan.FromSeconds(.1f));

        }
        _mainCamera.transform.position = _cameraOldPosition; //sets back the position

        yield return new WaitForSeconds(0f); //dummy return value for IAsync Type

    }
}