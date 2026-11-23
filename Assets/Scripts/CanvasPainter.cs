using UnityEngine;

public class CanvasPainter : MonoBehaviour
{
    public Transform fingertip;
    public Renderer cursorRenderer;
    public Transform cursor;

    public int textureSize = 512;
    public int brushRadius = 8;
    public Color brushColor = Color.white;
    public Color backgroundColor = new Color(0.71f, 0.35f, 0.23f);
    public Color guideColor = new Color(0.85f, 0.6f, 0.5f);
    public float touchDistance = 0.06f;

    [Header("Trace target")]
    public Texture2D guideArt;
    public float alphaThreshold = 0.3f;
    public int tolerance = 12;       // pixels around the art that don't count as mistakes
    public float winCoverage = 0.7f;

    [Header("Fallback ring (used if no art is assigned)")]
    public float ringRadius = 0.30f;
    public float ringThickness = 0.05f;

    Texture2D tex;
    bool[] isTarget, isNear, isPainted;
    int targetCount, paintedTarget, paintedOutside;
    bool done;
    Vector2 lastUV;
    bool hasLast;

    void Start()
    {
        int n = textureSize;
        tex = new Texture2D(n, n, TextureFormat.RGBA32, false);
        isTarget = new bool[n * n];
        isPainted = new bool[n * n];
        var px = new Color[n * n];

        for (int y = 0; y < n; y++)
            for (int x = 0; x < n; x++)
            {
                float u = (x + 0.5f) / n, v = (y + 0.5f) / n;
                bool t;
                if (guideArt != null)
                    t = guideArt.GetPixelBilinear(u, v).a > alphaThreshold;
                else
                {
                    float d = Mathf.Sqrt((u - 0.5f) * (u - 0.5f) + (v - 0.5f) * (v - 0.5f));
                    t = Mathf.Abs(d - ringRadius) <= ringThickness * 0.5f;
                }
                int i = y * n + x;
                isTarget[i] = t;
                if (t) targetCount++;
                px[i] = t ? guideColor : backgroundColor;
            }

        isNear = Dilate(isTarget, tolerance);
        tex.SetPixels(px);
        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;
    }

    bool[] Dilate(bool[] src, int r)
    {
        int n = textureSize;
        var tmp = new bool[n * n];
        var dst = new bool[n * n];
        for (int y = 0; y < n; y++)
            for (int x = 0; x < n; x++)
                if (src[y * n + x])
                    for (int k = -r; k <= r; k++)
                    {
                        int xx = x + k;
                        if (xx >= 0 && xx < n) tmp[y * n + xx] = true;
                    }
        for (int y = 0; y < n; y++)
            for (int x = 0; x < n; x++)
                if (tmp[y * n + x])
                    for (int k = -r; k <= r; k++)
                    {
                        int yy = y + k;
                        if (yy >= 0 && yy < n) dst[yy * n + x] = true;
                    }
        return dst;
    }

    void Update()
    {
        if (done) return;

        Vector3 local = transform.InverseTransformPoint(fingertip.position);
        bool inBounds = Mathf.Abs(local.x) <= 0.5f && Mathf.Abs(local.y) <= 0.5f;
        float dist = Mathf.Abs(Vector3.Dot(fingertip.position - transform.position, transform.forward));
        bool touching = inBounds && dist < touchDistance;

        if (cursor != null)
        {
            cursor.gameObject.SetActive(inBounds);
            cursor.position = transform.TransformPoint(new Vector3(local.x, local.y, 0));
            if (cursorRenderer != null)
                cursorRenderer.material.color = touching ? brushColor : Color.gray;
        }

        if (touching)
        {
            Vector2 uv = new Vector2(local.x + 0.5f, local.y + 0.5f);
            if (hasLast)
            {
                int steps = Mathf.CeilToInt(Vector2.Distance(lastUV, uv) * textureSize);
                for (int i = 0; i <= steps; i++)
                    Stamp(Vector2.Lerp(lastUV, uv, steps == 0 ? 0 : (float)i / steps));
            }
            else Stamp(uv);
            tex.Apply();
            lastUV = uv;
            hasLast = true;

            if (Coverage() >= winCoverage)
            {
                done = true;
                Debug.Log("Level complete. Coverage " + Coverage() + ", outside pixels " + paintedOutside);
            }
        }
        else hasLast = false;
    }

    float Coverage()
    {
        return targetCount == 0 ? 0f : (float)paintedTarget / targetCount;
    }

    void Stamp(Vector2 uv)
    {
        int n = textureSize;
        int cx = (int)(uv.x * n), cy = (int)(uv.y * n);
        for (int x = -brushRadius; x <= brushRadius; x++)
            for (int y = -brushRadius; y <= brushRadius; y++)
                if (x * x + y * y <= brushRadius * brushRadius)
                {
                    int px = cx + x, py = cy + y;
                    if (px < 0 || px >= n || py < 0 || py >= n) continue;
                    int idx = py * n + px;
                    if (!isPainted[idx])
                    {
                        isPainted[idx] = true;
                        if (isTarget[idx]) paintedTarget++;
                        else if (!isNear[idx]) paintedOutside++;
                    }
                    tex.SetPixel(px, py, brushColor);
                }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 28;
        GUI.Label(new Rect(20, 20, 600, 40), "Traced: " + Mathf.RoundToInt(Coverage() * 100) + "%");
        if (done) GUI.Label(new Rect(20, 60, 600, 40), "Level complete!");
    }
}