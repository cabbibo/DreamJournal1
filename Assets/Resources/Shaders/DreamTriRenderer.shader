Shader "Custom/DreamTriRenderer" {
Properties {
        _RoomTexture ("roomTexture", 2D) = "white" {}
    }
  SubShader{

  	


    Cull off
    Pass{


      CGPROGRAM
      #pragma target 5.0

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "Chunks/DreamVertStruct.cginc"


      uniform sampler2D _RoomTexture;




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


      #include "Chunks/hash.cginc"

      varyings vert (uint id : SV_VertexID){

        varyings o;


        Vert v = _vertBuffer[ id ];
    


        float3 fPos = v.pos;

        //fPos = float3( hash( id ) , hash( id +1 ), hash( id+2));

       //mul( b.transform, float4(fPos,1) ).xyz;


		o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));
		o.worldPos = fPos;
		o.eye = _WorldSpaceCameraPos - o.worldPos;
	
		o.nor = v.nor;
		o.uv = v.uv;

        o.debug = float3( clamp( v.section -3 ,0 ,1)  , clamp( v.section -4 ,0 ,1) , clamp( v.section -5 ,0 ,1));

        return o;


      }
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      	float3 col = tex2D( _RoomTexture , v.uv ).xyz;// v.nor * .5 + .5;

        return float4( col , 1. );


      }

      ENDCG

    }
  }

  Fallback Off
  
}