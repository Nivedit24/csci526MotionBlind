// A custom shader that creates a spotlight effect using the stencil buffer
Shader "Custom/Spotlight"
{
    // Define the properties that can be changed in the inspector
    Properties
    {
        // Define a texture property with the reference name _MainTex
        _MainTex ("Texture", 2D) = "white" {}
    }
    // Define the subshader
    SubShader
    {
        // Define the tags for rendering order and lighting mode
        Tags
        {
            "RenderType"="Opaque" "Queue"="Geometry-1"
        }
        // Disable color and depth writing
        ColorMask 0
        ZWrite off

        // Use the Stencil command to configure the stencil buffer settings
        Stencil
        {
            // Set the reference value to 1
            Ref 1
            // Compare the reference value with the current value in the stencil buffer
            Comp always
            // Replace the current value in the stencil buffer with the reference value if the comparison passes
            Pass replace
        }

        // Define the pass
        Pass
        {
            // Cull back faces
            Cull Back
            // Perform depth testing with less function
            ZTest Less

            // Use CG language for shader code
            CGPROGRAM
            // Use the default vertex shader
            #pragma vertex vert
            // Use this function as the fragment shader
            #pragma fragment frag

            // Declare the input structure for the vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
            };

            // Declare the output structure for the vertex shader
            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            // Define the vertex shader function
            v2f vert(appdata v)
            {
                v2f o;
                // Transform the vertex position from object space to clip space
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // Define the fragment shader function
            half4 frag(v2f i) : COLOR
            {
                // Return a transparent color (this is not visible, but it writes to the stencil buffer)
                return half4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}