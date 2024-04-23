Shader "Unlit/Cel Shading"
{
    Properties {
        _Color ("Diffuse Color", Color) = (1,1,1,1)
        _UnlitColor ("Unlit Diffuse Color", Color) = (0.5,0.5,0.5,1)
        _DiffuseThreshold ("Threshold for Diffuse Colors", Range(0,1)) = 0.1
        _DiffuseTransition ("Diffuse Transition", Range(0.001, 1.0)) = 0.1
        _DiffuseTransitionCorrection ("Diffuse Transition Correction", Range(0.001, 1.0)) = 0.1
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    SubShader {
        Pass {    
            Tags { "LightMode" = "ForwardBase" "PassFlags" = "OnlyDirectional" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform float4 _LightColor0; // color of light source (from "Lighting.cginc")

            uniform float4 _Color;
            uniform float4 _UnlitColor;
            uniform float _DiffuseThreshold;
            uniform float _DiffuseTransition;
            uniform float _DiffuseTransitionCorrection;
            uniform sampler2D _MainTex;
         
            struct vertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct vertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float2 uv : TEXCOORD0;
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;
                float4x4 modelMatrix = unity_ObjectToWorld;
                float4x4 modelMatrixInverse = unity_WorldToObject;
                
                output.posWorld = mul(modelMatrix, input.vertex);
                output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
                output.pos = UnityObjectToClipPos(input.vertex);
                
                output.uv = input.texcoord;
                
                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                float3 normalDirection = normalize(input.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
                float3 lightDirection;
                // float attenuation;
                
                lightDirection = normalize(_WorldSpaceLightPos0.xyz);

                // if (0.0 == _WorldSpaceLightPos0.w) // directional light?
                // {
                //     attenuation = 1.0; // no attenuation
                //     lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                // }
                // else // point or spot light
                // {
                //     float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
                //     float distance = length(vertexToLightSource);
                //     attenuation = 1.0 / distance; // linear attenuation
                //     lightDirection = normalize(vertexToLightSource);
                // }

                float ndotl = dot(normalDirection, lightDirection);
                float smoothNDot = lerp(0.0, _DiffuseThreshold, ndotl * 0.5);

                float3 light = smoothNDot * _LightColor0;

                float diffuseBlend = smoothNDot / _DiffuseTransition + _DiffuseTransitionCorrection;
                
                float3 lightningColor = lerp(_UnlitColor, light, 0.5);
                float3 color = lerp(lightningColor, _Color, diffuseBlend);
                return float4(color, 1.0) * tex2D(_MainTex, input.uv);
            }

           ENDCG
        }
    }

    Fallback "Diffuse"
}
