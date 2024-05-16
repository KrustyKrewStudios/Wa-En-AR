Shader "Imagine/Screenshot"
{
    Properties
    {
        _BaseTexture ("Base Texture", 2D) = "white" {}
        _OverlayTexture ("Overlay Texture", 2D) = "white" {}
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

            sampler2D _BaseTexture;
            sampler2D _OverlayTexture;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_BaseTexture, i.uv);
                fixed4 overlayColor = tex2D(_OverlayTexture, i.uv);
                float a = overlayColor.a;
                fixed4 finalColor = baseColor * (1-a) + overlayColor * a;
                return finalColor;
            }
            ENDCG
        }
    }
}