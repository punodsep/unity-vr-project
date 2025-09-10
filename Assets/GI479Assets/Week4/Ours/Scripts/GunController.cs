using Oculus.Interaction.HandGrab;
using TheDeveloperTrain.SciFiGuns;
using UnityEngine;
using UnityEngine.Events;

public class GunController : MonoBehaviour, IHandGrabUseDelegate
{
    [Header("Fire")]
    [SerializeField]
    private Transform bulletSpawnPosition;

    [SerializeField, Range(0, 1)]
    private float fireThreshold = 0.7f;

    [SerializeField, Range(0, 1)]
    private float releaseThreshold = 0.2f;

    [SerializeField]
    private Bullet bullet;

    [Space]
    [SerializeField]
    private bool isAutoFireMode = true;

    [SerializeField]
    private float fireInterval = 0.25f;

    [Header("Trigger")]
    [SerializeField]
    private Transform triggerPivot;

    [SerializeField]
    private float triggerSpeed = 20;

    [Space]
    [SerializeField]
    private bool rotateXAxis;

    [SerializeField]
    private float triggerXRotation;

    [Space]
    [SerializeField]
    private bool moveZAxis;

    [SerializeField]
    private float triggerZPosition;

    public UnityEvent WhenShoot;

    private float lastUseTime;
    private bool wasFired;
    private float dampedUseStrength;
    private float fireTimer;

    #region IHandGrabUseDelegate
    public void BeginUse()
    {
        Debug.Log($"Begin use: {gameObject.name}");
        dampedUseStrength = 0;
        lastUseTime = Time.realtimeSinceStartup;
    }

    public float ComputeUseStrength(float strength)
    {
        var delta = Time.realtimeSinceStartup - lastUseTime;
        lastUseTime = Time.realtimeSinceStartup;
        if (strength > 0)
        {
            dampedUseStrength = Mathf.Lerp(dampedUseStrength, strength, triggerSpeed * delta);
        }
        else
        {
            dampedUseStrength = strength;
        }
        UpdateTriggerProgress(dampedUseStrength);
        return dampedUseStrength;
    }

    void Update()
    {
        if (!isAutoFireMode) return;

        if (wasFired && fireTimer <= 0)
        {
            ShootBullet();
            fireTimer = fireInterval;
        }
        fireTimer -= Time.deltaTime;
    }

    public void EndUse()
    {
        Debug.Log($"End use: {gameObject.name}");
    }

    #endregion

    private void UpdateTriggerProgress(float progress)
    {
        if (rotateXAxis)
            UpdateTriggerRotation(progress);

        if (moveZAxis)
            UpdateTriggerPosition(progress);

        if (progress >= fireThreshold && !wasFired)
        {
            wasFired = true;

            if (!isAutoFireMode)
            {
                ShootBullet();
            }
        }
        else if (progress <= releaseThreshold)
        {
            wasFired = false;
        }
    }

    private void UpdateTriggerRotation(float progress)
    {
        var value = triggerXRotation * progress;
        var angles = triggerPivot.localEulerAngles;
        angles.x = value;
        triggerPivot.localEulerAngles = angles;
    }

    private void UpdateTriggerPosition(float progress)
    {
        var value = triggerZPosition * progress;
        var position = triggerPivot.localPosition;
        position.z = value;
        triggerPivot.localPosition = position;
    }

    private void ShootBullet()
    {
        Debug.Log($"{gameObject.name} shoot a bullet.");
        Instantiate(bullet, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        WhenShoot?.Invoke();
    }
}
