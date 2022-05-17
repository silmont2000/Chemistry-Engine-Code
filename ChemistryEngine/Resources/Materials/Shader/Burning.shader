Shader "MyEffect/Burning" {
    Properties {
        _Color ("颜色", Color) = (1,1,1,1)
        _MainTex ("主贴图 (RGB)", 2D) = "white" {}
        _Glossiness ("平滑度", Range(0,1)) = 0.5
        _Metallic ("金属性", Range(0,1)) = 0.0
        _NoiseTex ("噪声贴图 (R)",2D) = "white"{}  
        _EdgeWidth("边缘宽度",Range(0,0.5)) = 0.1  
        _EdgeColor("边缘颜色",Color) =  (1,1,1,1)  
        _EdgeThresholdValue ("硬边缘阈值(0为不使用)",Range(0,1)) = 0.5  
        _DissolvePercentage ("溶解百分比",Range(0,1)) = 0  
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        //原编译指令
        //#pragma surface surf Standard fullforwardshadows
        //增加addshadow以获得正确的阴影
        #pragma surface surf Standard fullforwardshadows addshadow
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        sampler2D _MainTex;
        sampler2D _NoiseTex;  
        float _EdgeWidth;  
        float4 _EdgeColor;  
        float _EdgeThresholdValue;  
        float _DissolvePercentage;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 _NoiseTex_ST;

        void surf (Input IN, inout SurfaceOutputStandard o) {

            // Albedo comes from a texture tinted by color
            // fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = _Color.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            // o.Alpha = c.a;
            o.Alpha = 1.0;
            float DissolveFactor = saturate(_DissolvePercentage);  
            //使用固定坐标
            // float noiseValue = tex2D(_NoiseTex,IN.uv_MainTex).r;   
            //使用世界坐标
            float2 uv = IN.worldPos.xy * _NoiseTex_ST.xy +_NoiseTex_ST.zw;
            float noiseValue = tex2D(_NoiseTex,uv).r;

            //如果该值对应噪声图的值更大,则抛弃

            if(noiseValue <= DissolveFactor)  
            {  
                discard;  
            }   
            float4 texColor = _Color;
            float EdgeFactor = saturate((noiseValue - DissolveFactor)/(_EdgeWidth*DissolveFactor));  
            float4 BlendColor = texColor * _EdgeColor;  

            if(_EdgeThresholdValue>0){
                //不使用渐变{硬边缘）
                float HardEdgeFactor=EdgeFactor;
                if(HardEdgeFactor>_EdgeThresholdValue){
                    HardEdgeFactor=1;
                    o.Emission =0;  
                    }else{ 
                    HardEdgeFactor=0;
                    o.Emission =_EdgeColor; 
                }
                o.Albedo = lerp(texColor,BlendColor,1-EdgeFactor);  
                }else{
                o.Emission =0; 
                //使用渐变{软边缘）
                if(_EdgeThresholdValue>=1){
                    o.Albedo = BlendColor; 
                    o.Alpha=0;
                    }else{
                    o.Albedo = lerp(texColor,BlendColor,1 - EdgeFactor);  
                }
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}