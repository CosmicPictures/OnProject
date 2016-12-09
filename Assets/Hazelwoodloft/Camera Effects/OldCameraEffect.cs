using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu("Image Effects/Old Camera")]
public class OldCameraEffect : MonoBehaviour
{

	public bool monochrome = true;
	public float grainIntensityMin = 0.5f;
	public float grainIntensityMax = 1.0f;
	public float grainSize = 1.0f;
	public Texture grainTexture;
	public Texture Vignette;
	public Shader   OldCameraEffectShader;
	private Material m_OldCam;
	public float    OffsetR=0.7f;

	


	
	protected void Start ()
	{
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}

	}
	
	protected Material material {
		get {
			if( m_OldCam == null ) {
				m_OldCam = new Material( OldCameraEffectShader );
				m_OldCam.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_OldCam;
		}
	}
	
	protected void OnDisable() {
		if( m_OldCam )
			DestroyImmediate( m_OldCam );
	}
	
	private void SanitizeParameters()
	{
		grainIntensityMin = Mathf.Clamp( grainIntensityMin, 0.0f, 5.0f );
		grainIntensityMax = Mathf.Clamp( grainIntensityMax, 0.0f, 5.0f );
		grainSize = Mathf.Clamp( grainSize, 0.1f, 50.0f );
	}

	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		SanitizeParameters();
		

		Material mat = material;

		mat.SetTexture("_GrainTex", grainTexture);
		mat.SetFloat("_Scale",OffsetR);
		mat.SetTexture("_Vignette", Vignette);
		float grainScale = 1.0f / grainSize; // we have sanitized it earlier, won't be zero
		mat.SetVector("_GrainOffsetScale", new Vector4(
			Random.value,
			Random.value,
			(float)Screen.width / (float)grainTexture.width * grainScale,
			(float)Screen.height / (float)grainTexture.height * grainScale
		));
		mat.SetFloat("_Intensity",Random.Range(grainIntensityMin, grainIntensityMax));
		ImageEffects.BlitWithMaterial( mat, source, destination );
	}
}
