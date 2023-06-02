using UnityEngine;
[ExecuteAlways]
public class MeshBoundViewer : MonoBehaviour
{
    [SerializeField] private RectTransform uiFrameRt; //UI��

    private Vector3 cachedScale;
    private Pose cachedPose;

    private Mesh mesh;

    private void Start()
    {
        cachedPose = new Pose(transform.position, transform.rotation);
        cachedScale = transform.localScale;
        mesh = GetComponent<MeshFilter>().sharedMesh;
        Execute(uiFrameRt, mesh.bounds, transform, 50, 60f);
    }

    private void Update()
    {
        if (transform.position != cachedPose.position || transform.rotation != cachedPose.rotation || transform.localScale != cachedScale)
        {
            cachedPose = new Pose(transform.position, transform.rotation);
            cachedScale = transform.localScale;
            Execute(uiFrameRt, mesh.bounds, transform, 50, 60f);
        }
    }

    /// <summary>
    /// ʹUI���Χס3D����
    /// </summary>
    /// <param name="uiFrameRt">UI�� RectTransform</param>
    /// <param name="bounds">3D����߽��</param>
    /// <param name="tf">3D����Transform</param>
    /// <param name="paddingHorizontal">ˮƽ�����ϵı߾�</param>
    /// <param name="paddingVertical">��ֱ�����ϵı߾�</param>
    public void Execute(RectTransform uiFrameRt, Bounds bounds, Transform tf, float paddingHorizontal, float paddingVertical)
    {
        Vector3[] points = GetVertexFromBounds(bounds, tf);
        points = World2ScreenPoints(points);
        GetBoundaryPos(points, out float minX, out float maxX, out float minY, out float maxY);
        //���ÿ��
        uiFrameRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxX - minX + paddingHorizontal);
        //���ø߶�
        uiFrameRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY - minY + paddingVertical);
        //����ê��
        uiFrameRt.anchorMin = Vector2.zero;
        uiFrameRt.anchorMax = Vector2.zero;
        //����λ��
        uiFrameRt.anchoredPosition3D = new Vector2((minX + maxX) * .5f, (minY + maxY) * .5f);
    }

    //��ȡ�߽�еĶ�������
    private Vector3[] GetVertexFromBounds(Bounds bounds, Transform tf)
    {
        Vector3[] verts = new Vector3[8];
        verts[0] = tf.TransformPoint(bounds.center + new Vector3(-bounds.size.x, -bounds.size.y, -bounds.size.z) * 0.5f);
        verts[1] = tf.TransformPoint(bounds.center + new Vector3(bounds.size.x, -bounds.size.y, -bounds.size.z) * 0.5f);
        verts[2] = tf.TransformPoint(bounds.center + new Vector3(bounds.size.x, -bounds.size.y, bounds.size.z) * 0.5f);
        verts[3] = tf.TransformPoint(bounds.center + new Vector3(-bounds.size.x, -bounds.size.y, bounds.size.z) * 0.5f);
        verts[4] = tf.TransformPoint(bounds.center + new Vector3(-bounds.size.x, bounds.size.y, -bounds.size.z) * 0.5f);
        verts[5] = tf.TransformPoint(bounds.center + new Vector3(bounds.size.x, bounds.size.y, -bounds.size.z) * 0.5f);
        verts[6] = tf.TransformPoint(bounds.center + new Vector3(bounds.size.x, bounds.size.y, bounds.size.z) * 0.5f);
        verts[7] = tf.TransformPoint(bounds.center + new Vector3(-bounds.size.x, bounds.size.y, bounds.size.z) * 0.5f);
        return verts;
    }

    //��ȡ�߽�ֵ
    private void GetBoundaryPos(Vector3[] points, out float minX, out float maxX, out float minY, out float maxY)
    {
        minX = float.MaxValue;
        maxX = float.MinValue;
        minY = float.MaxValue;
        maxY = float.MinValue;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 point = points[i];
            minX = point.x < minX ? point.x : minX;
            maxX = point.x > maxX ? point.x : maxX;
            minY = point.y < minY ? point.y : minY;
            maxY = point.y > maxY ? point.y : maxY;
        }
    }

    //����ת��Ļ����
    private Vector3[] World2ScreenPoints(Vector3[] points)
    {
        //��ȡ�����
        Camera mainCamera = Camera.main;
        if (mainCamera == null) mainCamera = FindObjectOfType<Camera>();
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 point = points[i];
            points[i] = mainCamera.WorldToScreenPoint(point);
        }
        return points;
    }
}