Shader "Unlit/DarknessShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("Color", Color) = (0.0,0.0,0.0,1.0)
        _LightColor("LightColor", Color) = (0.0,0.0,0.0,1.0)
        _CirclePosition ("CirclePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
        _CircleRadius ("Radius", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZTest Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPos :TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 _CirclePosition;
            float _CircleRadius;
            float4 _MainColor;
            float4 _LightColor;
            float4 _CirclePositions[30];

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _MainColor;

                [unroll]
                for(int j = 0; j < 30; j++){

                    float dis = distance(i.worldPos.xyz, _CirclePositions[j].xyz);

                    col = dis > _CircleRadius && col != _LightColor ? _MainColor : _LightColor;
                }

                //float dis = distance(i.worldPos.xyz, _CirclePosition.xyz);

                //fixed4 col = dis < _CircleRadius ? (0.0, 0.0, 0.0, 0.0) : _MainColor;

                return col;
            }
            ENDCG
        }
    }
}
