Shader "MyEffect/Ocean"
{
    Properties
    {
        _MainTex("主贴图", 2D) = "white" {}
        _NoiseTex("噪声图", 2D) = "white" {}
        _Color("颜色",Color) = (1,1,1,1)
        _Light("亮度", Range(0, 10)) = 2
        _Intensity("扭曲强度", float) = 0.1
        _XSpeed("X偏移", float) = 0.1
        _YSpeed("Y偏移", float) = 0.1
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" }
        LOD 100
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _Color;
            float _Light;
            float _Intensity;
            float _XSpeed;
            float _YSpeed;
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                //根据时间和偏移速度获取噪音图的颜色作为uv偏移
                fixed4 noise_col = tex2D(_NoiseTex, i.uv + fixed2(_Time.y*_XSpeed, _Time.y*_YSpeed));
                //计算uv偏移的颜色和亮度和附加颜色计算
                fixed4 col = tex2D(_MainTex, i.uv + _Intensity * noise_col.rg)*_Light*_Color;
                UNITY_APPLY_FOG(i.fogCoord, col);
                col.a = _Color.a;
                return col;
            }
            ENDCG
        }
    }
}