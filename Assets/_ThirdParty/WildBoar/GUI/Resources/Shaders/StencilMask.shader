Shader "Unlit/StencilMask"
{
	Properties 
	{
		[IntRange] _StencilRef("Stencil Ref", Range(0, 255)) = 1
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent-1"
		}
		Pass 
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend Zero One 


			Stencil
			{
				Ref[_StencilRef]
				Comp Always
				Pass Replace
			}

		}
	}

	Fallback Off
}
