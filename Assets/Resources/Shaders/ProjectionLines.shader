Shader "Custom/ProjectionLines" {
  SubShader{

  	


    Cull off
    Pass{

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha


      CGPROGRAM
      #pragma target 5.0

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "Chunks/DreamVertStruct.cginc"




      StructuredBuffer<Vert> _vertBuffer;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos 			: SV_POSITION;
          float3 nor 			: TEXCOORD0;
          float2 uv  			: TEXCOORD1;
          float3 eye      : TEXCOORD5;
          float3 worldPos : TEXCOORD6;
          float3 debug    : TEXCOORD7;

      };

      uniform float3 CenterPos;
      uniform float raysOn;


      #include "Chunks/hash.cginc"

      varyings vert (uint id : SV_VertexID){

        varyings o;


        int idDiv = (id /2);
        int mVal = id % 2;

        Vert v = _vertBuffer[ idDiv * 16 ];
    

        float3 dir = v.pos - v.artifactPos;
    		float3 fPos = CenterPos + dir * raysOn * clamp( (v.section -3.1) * 10, 0, 1);
        if( mVal == 1 ){

        	fPos = v.artifactPos;
        }

       // float3 fPos = v.pos;

        //fPos = float3( hash( id ) , hash( id +1 ), hash( id+2));

       //mul( b.transform, float4(fPos,1) ).xyz;


				o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));
				o.worldPos = fPos;
				o.eye = _WorldSpaceCameraPos - o.worldPos;
	
				o.nor = normalize( dir );//float3(0,0,0);
				o.uv = float2(0,0);
        o.debug = float3( 0 , 0 , 0);

        return o;


      }
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      	float3 col = v.nor * .5 + .5;

        return float4( col , .1 );


      }

      ENDCG

    }
  }

  Fallback Off
  
}