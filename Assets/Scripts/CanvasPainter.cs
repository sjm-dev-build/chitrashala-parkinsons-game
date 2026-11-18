using UnityEngine;

public class CanvasPainter : MonoBehaviour
{
    public Transform fingertip;
    public Renderer cursorRenderer;
    public Transform cursor;

    public int textureSize = 512;
    public int brushRadius = 6;
    public Color brushColor = Color.white;
    public Color backgroundColor = new Color(0.78f, 0.47f, 0.31f);
    public float touchDistance = 0.02f; // metres

    Texture2D tex;
    Vector2 lastUV;
    bool hasLast;

    void Start()
    {
        tex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        var px = new Color[textureSize * textureSize];
        for (int i = 0; i < px.Length; i++) px[i] = backgroundColor;
        tex.SetPixels(px);
        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;
    }

    void Update()
    {
        Vector3 local = transform.InverseTransformPoint(fingertip.position);
        bool inBounds = Mathf.Abs(local.x) <= 0.5f && Mathf.Abs(local.y) <= 0.5f;
        float dist = Mathf.Abs(Vector3.Dot(fingertip.position - transform.position, transform.forward));
        bool touching = inBounds && dist < touchDistance;

        if (cursor != null)
        {
            cursor.gameObject.SetActive(inBounds);
            cursor.position = transform.TransformPoint(new Vector3(local.x, local.y, 0));
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
        }
        else hasLast = false;
    }

    void Stamp(Vector2 uv)
    {
        int cx = (int)(uv.x * textureSize), cy = (int)(uv.y * textureSize);
        for (int x = -brushRadius; x <= brushRadius; x++)
            for (int y = -brushRadius; y <= brushRadius; y++)
                if (x * x + y * y <= brushRadius * brushRadius)
                {
                    int px = cx + x, py = cy + y;
                    if (px >= 0 && px < textureSize && py >= 0 && py < textureSize)
                        tex.SetPixel(px, py, brushColor);
                }
    }
}