using UnityEngine;

[ExecuteInEditMode]
public class ScannerEffectDemo : MonoBehaviour
{
    [SerializeField] private GameObject ScannerOriginPosition;
    [SerializeField] private Vector3 scanPos;
    [SerializeField] private Material EffectMaterial;
    [SerializeField] private float ScanSpeed;
    [SerializeField] private float ScanMaxDistance;
    [SerializeField] private float ScanDistance;

    private bool _scanning;

    private Camera _camera;
	

    void OnEnable()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;
    }

    private void Update()
    {
        ScanInProgress();
    }


    [ContextMenu("Start Scanning")]
    void StartScan()
    {
        ScanDistance = 0;
        EffectMaterial.SetFloat("_ScanMaxDistance", ScanMaxDistance);

        scanPos = ScannerOriginPosition.transform.position;

        EffectMaterial.SetVector("_WorldSpaceScannerPos", scanPos);

        _scanning = true;
    }

    void ScanInProgress()
    {
        if (_scanning)
        {
            ScanDistance += Time.deltaTime * ScanSpeed;
        }
    }

    [ContextMenu("Stop Scanning")]
    void StopScan()
    {
        _scanning = false;

        ScanDistance = 0;
    }



    [ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
        EffectMaterial.SetVector("_WorldSpaceScannerPos", scanPos);
        EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		RaycastCornerBlit(src, dst, EffectMaterial);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _camera.farClipPlane;
		float camFov = _camera.fieldOfView;
		float camAspect = _camera.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_camera.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}
}
