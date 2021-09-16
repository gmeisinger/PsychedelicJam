Shader "FeedbackPixelGlitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            #define PIXEL_SIZE 6.
            uniform float _TripFactor;

            int byte(int val, int byteNum)
            {
                const int shiftAmt = 8 * byteNum;
                return val & (0xFF << shiftAmt) >> shiftAmt;
            }
            int fnv(int currentHash, float newData) 
            {
                const int MIXER = 0x100001b3;
                int data = floatToIntBits(newData);

                for (int i = 0; i < 4; i++)
                {
                    currentHash = currentHash ^ byte(data, i);
                    currentHash *= MIXER;
                }

                return currentHash;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float minBlockSize = PIXEL_SIZE;
                float maxBlockSize = PIXEL_SIZE * 5.;

                // Chunk random(ish)ly
                float2 blockSize = max(float2(minBlockSize),
                                       float2(fmod(_Time * 173., maxBlockSize),
                                              fmod(_Time * 241., maxBlockSize)));
                blockSize = floor(point / PIXEL_SIZE) * PIXEL_SIZE;
                float2 blockCoord = floor(i.uv / blockSize) * blockSize;

                // 256 == never decay
                float decayChance = 246. + 10. / (maxBlockSize * maxBlockSize) * (blockSize.x * blockSize.y);
                decayChance *= _TripFactor;

                // Randomize decay using noise
                int hash = fnv(fnv(fnv(0, blockCoord.x), blockCoord.y), _Time);
                hash = byte(hash, 0) ^ byte(hash, 1) ^ byte(hash, 2) ^ byte(hash, 3);
                float decay = step(float(hash), decayChance);

                // Lerp chunk between transparent & texture based on decay (1 or 0)
                fixed4 col = tex2D(_MainTex, i.uv);
                return lerp(fixed4(0.0), col, decay);
            }
            ENDCG
        }
    }
}
