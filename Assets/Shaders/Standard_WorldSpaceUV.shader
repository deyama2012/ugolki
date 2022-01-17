Shader "Custom/Standard_WorldSpaceUV"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _TextureIntensity ("Texture Intensity", Range(0,1)) = 0.5

        [Space(10)]
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        float4 _MainTex_ST;

        struct Input
        {
            float2 texcoord;
        };

        void vert(inout appdata_full v, out Input output)
        { 
            UNITY_INITIALIZE_OUTPUT(Input, output);
            float2 uv = mul(unity_ObjectToWorld, v.vertex).xz;
            output.texcoord = uv* _MainTex_ST.xy + _MainTex_ST.zw;
        }

        UNITY_INSTANCING_BUFFER_START(Props)
            fixed4 _Color;
            half _Glossiness;
            half _Metallic;
            half _TextureIntensity;
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 col = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            fixed4 c = lerp(col, tex2D(_MainTex, IN.texcoord) * col, UNITY_ACCESS_INSTANCED_PROP(Props, _TextureIntensity));
            o.Albedo = c.rgb;
            o.Metallic = UNITY_ACCESS_INSTANCED_PROP(Props, _Glossiness);
            o.Smoothness = UNITY_ACCESS_INSTANCED_PROP(Props, _Metallic);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
