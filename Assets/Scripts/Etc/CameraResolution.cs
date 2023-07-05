using UnityEngine;

public class CameraResolution : BehaviorSingleton<CameraResolution>
{
    #region Monobehaviour
    private void Awake()
    {
        SetCameraResolution(9f, 16f);
    }
    #endregion

    #region Set Camera Resolution
    public void SetCameraResolution(float width, float height)
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = (float)Screen.width / Screen.height / (width / height);    // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }
    #endregion
}
