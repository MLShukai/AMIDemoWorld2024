Shader "Unlit/DisplayPosition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
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

            float3 hsv2rgb(float h, float s, float v)
            {
                return lerp(1, saturate(abs(frac(h + float3(0, 1, 2) / 3.) * 6 - 3) - 1), s) * v;
            }

            float4 frag (v2f i) : SV_Target
            {
                float col = tex2Dlod(_MainTex, float4(i.uv, 0, 0)).r;
                clip (col == 0 ? -1 : 0);
                return float4(hsv2rgb(col / _Time.y, 1, 1), 1);
            }
            ENDCG
        }
    }
}
