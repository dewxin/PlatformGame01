// Simplified SDF shader:
// - No Shading Option (bevel / bump / env map)
// - No Glow Option
// - Softness is applied on both side of the outline

Shader "TextMeshPro/Mobile/Distance Field Simple External Outline"
{

    Properties
    {
        _FaceColor ("Face Color", Color) = (1,1,1,1)

        _FaceColorAlpha ("Face Color Alpha", Range(0,1)) = 0

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)

        _OutlineColorAlpha ("Outline Color Alpha", Range(0,1)) = 0

        _MainTex ("Font Atlas", 2D) = "white" {}

    }

    CGINCLUDE
    #include "UnityCG.cginc"
    #include "UnityUI.cginc"


    struct vertex_t
    {
        float4 vertex : POSITION;
        float2 texcoord0 : TEXCOORD0;
    };

    struct pixel_t
    {
        float4 vertex : SV_POSITION;
        float2 texcoord0 : TEXCOORD0; // Texture UV
    };

    CBUFFER_START(UnityPerMaterial)
    sampler2D _MainTex;
    float4 _FaceColor;
    float _FaceColorAlpha;
    float4 _OutlineColor;
    float _OutlineColorAlpha;


    CBUFFER_END

    //seems that TextMeshPro will modify either input.vertex or input.textcorrd0,
    // causing rendering results different
    pixel_t VertShaderOutlined(vertex_t input)
    {
        pixel_t output;

        float4 vert = input.vertex;
        float4 vPosition = UnityObjectToClipPos(vert);


        // Populate structure for pixel shader
        output.vertex = vPosition;
        output.texcoord0 = input.texcoord0;
        return output;
    }

    float4 PixShader(pixel_t input) : SV_Target
    {
        float2 uv =  input.texcoord0.xy;

        float alpha = tex2D(_MainTex, uv).a;


        if(alpha>_FaceColorAlpha)
        {
            return _FaceColor;
        }
        else if(alpha > _OutlineColorAlpha)
        {
            float lerp = (alpha - _OutlineColorAlpha) / (_FaceColorAlpha - _OutlineColorAlpha);
            return _OutlineColor * (1 - lerp)  + _FaceColor * lerp;
        }

        clip(alpha - _OutlineColorAlpha);

        #ifdef OUTLINE_ON
        #endif

        return _FaceColor;
    }
    ENDCG

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }


        Cull [_CullMode]
        ZWrite Off
        Lighting Off
        Fog
        {
            Mode Off
        }
        ZTest [unity_GUIZTestMode]

        Blend One OneMinusSrcAlpha


        Pass
        {
            CGPROGRAM
            #pragma vertex VertShader
            #pragma fragment PixShader
            #pragma shader_feature __ OUTLINE_ON


            pixel_t VertShader(vertex_t input)
            {
                return VertShaderOutlined(input);
            }
            
            ENDCG
        }

    //    Pass
    //    {
    //        CGPROGRAM
    //        #pragma vertex VertShader
    //        #pragma fragment PixShader
    //        #pragma shader_feature __ OUTLINE_ON

    //        pixel_t VertShader(vertex_t input)
    //        {
    //            return VertShaderOutlined(input, 0);
    //        }

    //        ENDCG
    //    }
        }

}