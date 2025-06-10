using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration;

    private void Awake()
    {
       RangeWeapon.onBulletShot += ShakeCamera;
    }

    private void OnDestroy()
    {
        RangeWeapon.onBulletShot -= ShakeCamera;
    }

    public void ShakeCamera()
    {
        Vector2 direction = Random.onUnitSphere.With(z: 0).normalized;

        transform.localPosition = Vector3.zero;

        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, direction * shakeMagnitude, shakeDuration)
            .setEase(LeanTweenType.easeShake);
    }
}
