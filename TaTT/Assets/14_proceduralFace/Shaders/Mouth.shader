// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Procedural Mouth/Mouth"
{

    SubShader
    {
        Tags
        {
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

            v2f vert(appdata v)
            {
                v2f o;
                // map uv from [0,1] to [-1,1] range
                // o.uv = (v.uv * 2.0f) - 1.0f;
                o.uv = v.uv;
                o.uv1 = float2(v.data.r, v.data.g);
                o.inCol = v.inCol;
                o.outCol = v.outCol;
                o.thickness = v.data.b;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float sdCircle(float2 p, float r)
            {
                return length(p) - r;
            }

            float sdTunnel(in float2 p, in float2 wh)
            {
                p.x = abs(p.x);
                p.y = -p.y;
                float2 q = p - wh;

                float d1 = dot(max(q.x, 0.0), q.y);
                q.x = (p.y > 0.0) ? q.x : length(p) - wh.x;
                float d2 = dot(q.x, max(q.y, 0.0));
                float d = sqrt(min(d1, d2));

                return (max(q.x, q.y) < 0.0) ? -d : d;
            }

            float sdTunnel2(float2 p, float2 wh)
            {
                p.x = abs(p.x);
                p.y = -p.y;
                float2 q = p - wh;

                float d1 = max(q.x, 0.0);
                d1 = sqrt(d1 * d1 + q.y * q.y);
                q.x = (p.y > 0.0) ? q.x : length(p) - wh.x;
                float d2 = max(q.y, 0.0);
                d2 = sqrt(d2 * d2 + q.x * q.x);
                float d = min(d1, d2);

                return (max(q.x, q.y) < 0.0) ? -d : d;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                // return fixed4(i.uv,0,255);
                float2 p = i.uv * 2;
                float dist = 100000;
                dist = sdTunnel2(p, float2(1, 0));
                //dist = sdCircle(p, 1);
                float normDist = saturate(1.0 - dist);

                return dist <= 0 ? normDist * i.inCol : 0 * i.inCol;
                return normDist * i.inCol;

                float radius = length(i.uv);
                float mask = max(i.uv.g - i.uv1.r, i.uv1.g - i.uv.g) + 1;
                mask = max(radius, mask);

                //Antialiasing for border
                float borderAA = fwidth(mask);
                float border = smoothstep(i.thickness - borderAA, i.thickness, mask);

                float4 col = lerp(i.inCol, i.outCol, border);

                float alphaAA = fwidth(radius);
                float alpha = smoothstep(1, 1 - alphaAA, radius);

                return fixed4(col.rgb, col.a * alpha);
            }
            ENDCG
        }
    }
}