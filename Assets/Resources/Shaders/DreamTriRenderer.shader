Shader "Custom/DreamTriRenderer" {
Properties {
        _RoomTexture ("roomTexture", 2D) = "white" {}
        _CubeMap( "Cube Map" , Cube ) = "white" {}
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
      #include "Chunks/hsv.cginc"


      uniform sampler2D _RoomTexture;
      uniform sampler2D _Audio;
      uniform samplerCUBE _CubeMap;

      uniform float smoothedSection;
      uniform float3 ArtifactPos;




      StructuredBuffer<Vert> _vertBuffer;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos 			: SV_POSITION;
          float3 nor 			: TEXCOORD0;
          float2 uv  			: TEXCOORD1;
          float3 eye      : TEXCOORD5;
          float3 worldPos : TEXCOORD6;
          float3 debug    : TEXCOORD7;
          float3 art      : TEXCOORD8;
          float3 og       : TEXCOORD9;

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
    	o.art = ArtifactPos - o.worldPos;
    	o.og = v.pos - v.targetPos;
	
		o.nor = v.nor;
		o.uv = v.uv;

        o.debug = v.debug;//float3( clamp( v.section - 3 ,0 ,1)  , clamp( v.section -4 ,0 ,1) , clamp( v.section -5 ,0 ,1));

        return o;


      }
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {

        float3 roomCol = tex2D( _RoomTexture , v.uv ).xyz;
        float3 aCol = tex2D( _Audio , v.uv ).xyz;

        float3 refl = reflect( normalize( v.eye) , v.nor );



        float3 cubeCol = texCUBE( _CubeMap , refl ).xyz;


        float match = dot( v.nor , normalize( v.eye ));
        float m = 1;//dot( v.nor , normalize( v.art ));
        float3 pulseCol = m * roomCol * (1 / (2*(1.5 + .9*sin(4*_Time.y))*length( v.art)+ .1));


      	float3 col = lerp( roomCol , pulseCol , clamp(smoothedSection,0,1) );// v.nor * .5 + .5;
       

        float3 rainbowCol = normalize( v.nor ) * .5 + .5;
        rainbowCol *= cubeCol;
        float3 gooeyCol = lerp( roomCol , rainbowCol , clamp((length(v.og)-.2) * 2,0,1));
        col = lerp( col , gooeyCol , clamp(smoothedSection-1,0,1));

        
        float3 hue = hsv( (length( v.og )-.2) * .1 , 1 , 1 );
        float3 aCol2 = tex2D(_Audio , float2( match * match , 0 )).xyz;

        if( smoothedSection >= 8  ){
        	col = lerp( col , (roomCol + hue ) * col + roomCol + aCol2, clamp(smoothedSection-8,0,1));
        }

        //col = v.debug;
        //col = aCol;//cubeCol;
        return float4( col , 1. );


      }

      ENDCG

    }
  }

  Fallback Off
  
}