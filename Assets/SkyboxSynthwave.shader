Shader "Custom/SynthwaveSkybox"
{
    Properties
    {
        _GradientColor ("Gradient Base Color", Color) = (1, 0, 0.5, 1) // Base gradient color
        _SkyTopColor ("Sky Top Color", Color) = (0, 0, 0, 1) // Deep space color
        _GlowIntensity ("Glow Intensity", Range(0, 2)) = 1.0
        _RingColor ("Ring Color", Color) = (1, 0.5, 1, 1) // Neon glow
        _RingSize ("Ring Size", Range(0.1, 0.5)) = 0.3
        _RingSharpness ("Ring Sharpness", Range(0, 10)) = 5
        _SunColor ("Sun Color", Color) = (1, 0.5, 0.2, 1) // Warm neon sun
        _SunSize ("Sun Size", Range(0, 0.3)) = 0.1
        _SunOffset ("Sun Offset", Range(-0.5, 0.5)) = 0.2
        _MainTex ("Cubemap (HDR)", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "Queue"="Background" }
        Lighting Off
        ZWrite Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float3 texcoord : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            samplerCUBE _MainTex;
            float4 _GradientColor;
            float4 _SkyTopColor;
            float _GlowIntensity;
            float4 _RingColor;
            float _RingSize;
            float _RingSharpness;
            float4 _SunColor;
            float _SunSize;
            float _SunOffset;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 skyColor = texCUBE(_MainTex, i.texcoord);

                // 🎨 Smooth Gradient Effect (Horizon to Top)
                float gradientFactor = smoothstep(-0.2, 0.5, i.texcoord.y);
                fixed4 finalColor = lerp(_GradientColor, _SkyTopColor, gradientFactor);

                // 🔵 Glowing Ring Effect (Better Smoothing)
                float ringDist = abs(i.texcoord.y - 0.0);
                float ringGlow = exp(-pow((ringDist - _RingSize) * _RingSharpness, 2.0));
                finalColor += _RingColor * ringGlow * (_GlowIntensity * 0.8); // Less overpowering

                // ☀️ Static Sun Effect (Only in Front Half of the Skybox)
                if (i.texcoord.z > 0.0) { // Ensure sun appears only in front-facing hemisphere
                    float2 sunPos = float2(0.0, _SunOffset); // Sun position with proper offset
                    float2 sunCoord = i.texcoord.xy / i.texcoord.z; // Perspective correction

                    float sunGlow = smoothstep(_SunSize + 0.05, _SunSize, length(sunCoord - sunPos));
                    fixed4 sunColor = _SunColor * sunGlow; // Use defined Sun color
                    finalColor += sunColor;
                }



                return skyColor * finalColor;
            }
            ENDCG
        }
    }
}
