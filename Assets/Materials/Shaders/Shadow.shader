// A custom shader that creates a shadow effect using the stencil buffer
Shader "Custom/Shadow"
{
    // Define the properties that can be changed in the inspector
    Properties
    {
        // Define a texture property with the reference name _MainTex
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    // Define the subshader
    SubShader
    {
        // Use the Stencil command to configure the stencil buffer settings
        Stencil
        {
            // Set the reference value to 1
            Ref 1
            // Compare the reference value with the current value in the stencil buffer
            Comp notequal
            // Keep the current value in the stencil buffer if the comparison passes
            Pass keep
        }

        // Use a simple pass that renders a black color
        Pass
        {
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
            fixed4 frag(v2f i) : SV_Target
            {
                // Return a black color (this creates a shadow effect)
                return fixed4(0, 0, 0, 1);
            }
            ENDCG
        }
    }
}