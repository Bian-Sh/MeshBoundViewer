using UnityEngine;
[ExecuteAlways]
public class MeshBoundViewer : MonoBehaviour
{
    [SerializeField] private RectTransform uiFrameRt; //UI框

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
    /// 使UI框包围住3D物体
    /// </summary>
    /// <param name="uiFrameRt">UI框 RectTransform</param>
    /// <param name="bounds">3D物体边界盒</param>
    /// <param name="tf">3D物体Transform</param>
    /// <param name="paddingHorizontal">水平方向上的边距</param>
    /// <param name="paddingVertical">垂直方向上的边距</param>
    public void Execute(RectTransform uiFrameRt, Bounds bounds, Transform tf, float paddingHorizontal, float paddingVertical)
    {
        Vector3[] points = GetVertexFromBounds(bounds, tf);
        points = World2ScreenPoints(points);
        GetBoundaryPos(points, out float minX, out float maxX, out float minY, out float maxY);
        //设置宽度
        uiFrameRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxX - minX + paddingHorizontal);
        //设置高度
        uiFrameRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxY - minY + paddingVertical);
        //设置锚点
        uiFrameRt.anchorMin = Vector2.zero;
        uiFrameRt.anchorMax = Vector2.zero;
        //设置位置
        uiFrameRt.anchoredPosition3D = new Vector2((minX + maxX) * .5f, (minY + maxY) * .5f);
    }

    //获取边界盒的顶点数组
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

    //获取边界值
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

    //世界转屏幕坐标
    private Vector3[] World2ScreenPoints(Vector3[] points)
    {
        //获取主相机
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