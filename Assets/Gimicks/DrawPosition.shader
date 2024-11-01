Shader "Unlit/DrawPosition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UVPos ("UV Pos", Vector) = (0, 0, 0, 0)
        _Scale ("Circle Scale", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _UVPos;
            float _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float frag (v2f i) : SV_Target
            {
                float col = tex2D(_MainTex, i.uv).r;
                if (distance(i.uv, _UVPos.xz) < _Scale)
                {
                    col = _UVPos.w;
                }
                return col;
            }
            ENDCG
        }
    }
}
