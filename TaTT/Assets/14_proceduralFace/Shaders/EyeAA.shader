// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Procedural Eyes/EyeAA"
{
	SubShader
	{
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
		}


		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 inCol : COLOR;
                float4 outCol : TANGENT;
                float3 data : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 inCol : COLOR;
                float4 outCol : TANGENT;
                float thickness : FLOAT;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
                o.uv = v.uv;
                o.uv1 = float2(v.data.r, v.data.g);
                o.inCol = v.inCol;
                o.outCol = v.outCol;
                o.thickness = v.data.b;
                o.pos = UnityObjectToClipPos(v.vertex );
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float radius = length(i.uv);
                float mask = max(i.uv.g-i.uv1.r, i.uv1.g-i.uv.g)+1;
                mask = max(radius, mask);

                //Antialiasing for border
                float borderAA = fwidth(mask);
                float border = smoothstep(i.thickness-borderAA, i.thickness, mask);

                float4 col = lerp(i.inCol, i.outCol, border);

                float alphaAA = fwidth(radius);
                float alpha = smoothstep( 1, 1-alphaAA, radius);

                return fixed4(col.rgb, col.a*alpha);
			}
			ENDCG
		}
	}
}
