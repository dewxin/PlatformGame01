Shader "Hidden/MiniMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Color", COLOR) = (0.66,0.266,0.0031,0.1)
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
		}

		Pass
		{
			HLSLPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			CBUFFER_END

			struct appdata
			{
				float4 positionOS : Position;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 positionCS : SV_Position;
				float2 uv : TEXCOORD0;
			};


			v2f vert (appdata v)
			{
				v2f o;
				o.positionCS = TransformObjectToHClip(v.positionOS);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float4 frag (v2f i) : SV_TARGET
			{
				float4 textureSample = tex2D(_MainTex, i.uv);
				return textureSample * 0;
			}

			ENDHLSL
		}

    }    

   // Fallback "Unlit/Color"
}
