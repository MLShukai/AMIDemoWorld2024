
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DrawPosition : UdonSharpBehaviour
{
    [SerializeField]
    private int _renderTextureResolution = 2048;
    [SerializeField]
    private RenderTexture _renderTexture;
    [SerializeField]
    private RenderTexture _bufferTexture;
    [SerializeField]
    private Transform _centerPosition;
    [SerializeField]
    private float _Scale = 50;
    [SerializeField]
    private MeshRenderer _drawMaterial;
    [SerializeField]
    private MeshRenderer _displayMaterial;
    [SerializeField]
    private Transform _testTransform;
    [SerializeField, Tooltip("Draw time interval in seconds")]
    private float _drawTimeInterval = 60f;


    private void Start()
    {
        _renderTexture = new RenderTexture(_renderTextureResolution, _renderTextureResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
        _renderTexture.filterMode = FilterMode.Point;
        _renderTexture.Create();
        _bufferTexture = new RenderTexture(_renderTextureResolution, _renderTextureResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
        _bufferTexture.filterMode = FilterMode.Point;
        _bufferTexture.Create();
        _displayMaterial.material.SetTexture("_MainTex", _renderTexture);
    }

    private void Update()
    {
        var relPos = _testTransform.position - _centerPosition.position;
        var uvPos = relPos / _Scale + Vector3.one * 0.5f;
        var uvPosVector4 = new Vector4(uvPos.x, uvPos.y, uvPos.z, Time.time);
        _drawMaterial.material.SetVector("_UVPos", uvPosVector4);
        VRCGraphics.Blit(_renderTexture, _bufferTexture, _drawMaterial.material);
        VRCGraphics.Blit(_bufferTexture, _renderTexture);

        _displayMaterial.material.SetFloat("_DrawInterval", _drawTimeInterval);
        _displayMaterial.material.SetFloat("_CurrentTime", Time.time);
    }
}
