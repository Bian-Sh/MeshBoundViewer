using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTransform : MonoBehaviour
{
    public Camera mainCamera;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float minPositionX = -8f;
    public float maxPositionX = 8f;
    public float minPositionY = -4.5f;
    public float maxPositionY = 4.5f;
    public float minRotation = -180f;
    public float maxRotation = 180f;
    public float scaleSpeed = 0.1f;
    public float rotationSpeed = 0.1f;
    public float positionSpeed = 0.1f;

    private Vector3 targetScale;
    private Quaternion targetRotation;
    private Vector3 targetPosition;

    void Start()
    {
        // ��ʼ��Ŀ�����š���ת��λ��ֵ
        targetScale = transform.localScale;
        targetRotation = transform.rotation;
        targetPosition = transform.position;
    }

    void Update()
    {
        // �������
        if (Vector3.Distance(transform.localScale, targetScale) < 0.1f)
        {
            float randomScale = Random.Range(minScale, maxScale);
            targetScale = new Vector3(randomScale, randomScale, randomScale);
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

        // �����ת
        if (Quaternion.Angle(transform.rotation, targetRotation) < 5f)
        {
            float randomRotationX = Random.Range(minRotation, maxRotation);
            float randomRotationY = Random.Range(minRotation, maxRotation);
            float randomRotationZ = Random.Range(minRotation, maxRotation);
            targetRotation = Quaternion.Euler(randomRotationX, randomRotationY, randomRotationZ);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ���λ��
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            float randomPositionX = Random.Range(minPositionX, maxPositionX);
            float randomPositionY = Random.Range(minPositionY, maxPositionY);
            float randomPositionZ = transform.position.z; // ���ı� z ����
            targetPosition = new Vector3(randomPositionX, randomPositionY, randomPositionZ);
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionSpeed * Time.deltaTime);

        // �����Ϸ�����Ƿ�����Ļ��
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            // ����Ϸ�����ƶ�����Ļ��
            transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, viewportPoint.z));
            targetPosition = transform.position;
        }
    }
}
