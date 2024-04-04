// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Procedural Eyes/Pupil"
{
	SubShader
	{
		Tags {
			"RenderType"="Transparent"
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
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 mask : TANGENT;
                float4 col : COLOR;
                //float3 cutOff : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 mask : TANGENT;
                float4 col : COLOR;
                //float3 cutOff : NORMAL;
			};

			
			v2f vert (appdata v)
			{
				v2f o = (v2f)0;
                o.uv0 = v.uv0;
                o.uv1 = v.uv1;
                o.uv2 = v.uv2;
                o.col = v.col;
                o.mask = v.mask;
                //o.cutOff = v.cutOff;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//Circle shape (dist from center)
                float radius = length(i.uv0);
                //Clip to eye's radius
                float mask1 = length(i.mask.rg);
                //Clip based on how closed the eye is

                float mask2 = max(i.mask.b, i.mask.a);

                //float mask3 = distance(i.uv0, float2(i.cutOff.x, i.cutOff.y));

                float clipping = max(radius, max(mask1, mask2));
                //Multiply by ceil(1-(mask3*i.cutOff.z)) for cool effect


                float alpha = ceil(1-clipping);

                return fixed4(i.col.rgb, i.col.a*alpha);
			}
			ENDCG
		}
	}
}
