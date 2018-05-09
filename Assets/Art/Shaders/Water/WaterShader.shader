// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water/WaterShader"
{
	Properties
	{
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_ShallowWaterDistance("ShallowWaterDistance", Range( 0 , 1)) = 0
		_FarDeepDistance("FarDeepDistance", Range( 0 , 1)) = 0
		_MidWaterDistance("MidWaterDistance", Range( 0 , 1)) = 0
		_OverallDepth("OverallDepth", Range( 0 , 3)) = 0
		_ShallowColor1("ShallowColor1", Color) = (0.3368836,0.6120428,0.6838235,0)
		_MidWaterColor1("MidWaterColor1", Color) = (0.466317,0.4689327,0.8455882,0)
		_DeepColor1("DeepColor1", Color) = (0.466317,0.4689327,0.8455882,0)
		_FarDeepColor1("FarDeepColor1", Color) = (0.466317,0.4689327,0.8455882,0)
		_Refractindex("Refract index", Range( 0 , 10)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#pragma surface surf Standard keepalpha finalcolor:RefractionF noshadow exclude_path:deferred 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _ShallowColor1;
		uniform float4 _MidWaterColor1;
		uniform float4 _DeepColor1;
		uniform float4 _FarDeepColor1;
		uniform sampler2D _CameraDepthTexture;
		uniform float _OverallDepth;
		uniform float _FarDeepDistance;
		uniform float _MidWaterDistance;
		uniform float _ShallowWaterDistance;
		uniform float _Smoothness;
		uniform sampler2D _GrabTexture;
		uniform float _ChromaticAberration;
		uniform float _Refractindex;

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.00000000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( _GrabTexture, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			color.rgb = color.rgb + Refraction( i, o, refract( -ase_worldViewDir , ase_worldNormal , _Refractindex ).x, _ChromaticAberration ) * ( 1 - color.a );
			color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth73 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth73 = abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _OverallDepth ) );
			float temp_output_7_0 = abs( distanceDepth73 );
			float clampResult85 = clamp( ( temp_output_7_0 * (0 + (_FarDeepDistance - 0) * (6 - 0) / (1 - 0)) ) , 0 , 1 );
			float4 lerpResult81 = lerp( _DeepColor1 , _FarDeepColor1 , clampResult85);
			float clampResult119 = clamp( _MidWaterDistance , _FarDeepDistance , _ShallowWaterDistance );
			float clampResult43 = clamp( ( temp_output_7_0 * (0.1 + (clampResult119 - 0) * (6 - 0.1) / (1 - 0)) ) , 0 , 1 );
			float4 lerpResult41 = lerp( _MidWaterColor1 , lerpResult81 , clampResult43);
			float clampResult118 = clamp( _ShallowWaterDistance , _MidWaterDistance , 1 );
			float clampResult30 = clamp( ( temp_output_7_0 * (0.1 + (clampResult118 - 0) * (6 - 0.1) / (1 - 0)) ) , 0 , 1 );
			float4 lerpResult11 = lerp( _ShallowColor1 , lerpResult41 , clampResult30);
			o.Albedo = lerpResult11.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15001
2050;177;1586;824;-1753.344;-319.636;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;74;-3296,-528;Float;False;Property;_OverallDepth;OverallDepth;6;0;Create;True;0;0;False;0;0;0.003;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2672,-736;Float;False;Property;_MidWaterDistance;MidWaterDistance;5;0;Create;True;0;0;False;0;0;0.639;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-2560,-1056;Float;False;Property;_FarDeepDistance;FarDeepDistance;4;0;Create;True;0;0;False;0;0;0.091;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;73;-2992,-528;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2607.359,-142.1163;Float;False;Property;_ShallowWaterDistance;ShallowWaterDistance;3;0;Create;True;0;0;False;0;0;0.024;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;7;-2096,-528;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;83;-1968,-1008;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;119;-2208,-736;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;118;-2175.359,-142.1163;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;45;-1968,-736;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.1;False;4;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-1664,-800;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1664,-528;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;85;-1248,-784;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;80;-1872,-1664;Float;False;Property;_DeepColor1;DeepColor1;10;0;Create;True;0;0;False;0;0.466317,0.4689327,0.8455882,0;0.5996972,0.6268256,0.6911765,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-1728,-1200;Float;False;Property;_FarDeepColor1;FarDeepColor1;11;0;Create;True;0;0;False;0;0.466317,0.4689327,0.8455882,0;0.3823529,0.3618393,0.3176903,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;32;-1967.359,-126.1162;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.1;False;4;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1599.359,-174.1163;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;43;-1087.359,-446.1162;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;81;-976,-1072;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;42;-960,-1376;Float;False;Property;_MidWaterColor1;MidWaterColor1;9;0;Create;True;0;0;False;0;0.466317,0.4689327,0.8455882,0;0.7149654,0.7844291,0.8529412,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;173;1725.107,588.9203;Float;False;World;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;41;-80,-576;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;30;-1247.36,-142.1163;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-64,-800;Float;False;Property;_ShallowColor1;ShallowColor1;8;0;Create;True;0;0;False;0;0.3368836,0.6120428,0.6838235,0;0.9852941,0.9852941,0.9852941,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;175;1719.107,781.9203;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;174;1951.107,630.9203;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;177;1973.107,882.9203;Float;False;Property;_Refractindex;Refract index;15;0;Create;True;0;0;False;0;0;6.33;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;144;-328.3563,850.3149;Float;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;8f9abab6cb0d2c44ca012a0fd94c41b3;8f9abab6cb0d2c44ca012a0fd94c41b3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;157;910.2516,526.9816;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;162;455.9095,1142.157;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;168;824.5226,1144.897;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;135;-1196.514,403.1649;Float;True;Property;_Texture1;Texture 1;14;0;Create;True;0;0;False;0;8f9abab6cb0d2c44ca012a0fd94c41b3;8f9abab6cb0d2c44ca012a0fd94c41b3;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;687.9097,1143.157;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;161;301.8287,1080.77;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;773.6387,525.2416;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;150;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;147;-393.9529,658.2413;Float;False;Property;_TimeScale;TimeScale;13;0;Create;True;0;0;False;0;1;2.8;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;151;387.5579,462.8549;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;0.833252,1214.718;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.TanOpNode;163;567.9098,1141.157;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;166;548.6305,963.0394;Float;False;Constant;_Color1;Color 1;14;0;Create;True;0;0;False;0;0.5019608,0.5019608,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;133;-297.0688,237.9853;Float;True;Property;_T_WaterDrop;T_WaterDrop;12;0;Create;True;0;0;False;0;8f9abab6cb0d2c44ca012a0fd94c41b3;8f9abab6cb0d2c44ca012a0fd94c41b3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TanOpNode;155;653.6387,523.2416;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;514.6387,524.2416;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;779.1816,1034.491;Float;False;Constant;_Float1;Float 1;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;134;-1074.38,214.8705;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;169;1345.435,580.7208;Float;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;167;1031.353,909.2815;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;35.79106,650.7671;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;160;28.68692,1085.59;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;-160,-880;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;159;634.3593,345.1239;Float;False;Constant;_Color0;Color 0;14;0;Create;True;0;0;False;0;0.5019608,0.5019608,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;158;864.9105,416.5753;Float;False;Constant;_Float0;Float 0;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;1273.872,201.0645;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;149;1117.082,291.3661;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;136;-657.6912,669.7039;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;11.41569,551.2922;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;35.63867,763.2416;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;124;-293.2936,429.1283;Float;True;Property;_T_WaterDropRamp;T_WaterDropRamp;11;0;Create;True;0;0;False;0;0630d93062525384c9a6b26e4be9099a;0630d93062525384c9a6b26e4be9099a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;129;-1197.742,641.7263;Float;True;Property;_Texture0;Texture 0;12;0;Create;True;0;0;False;0;0630d93062525384c9a6b26e4be9099a;0630d93062525384c9a6b26e4be9099a;False;white;Auto;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-588.282,831.8267;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;1.5,1.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-3843.225,-2668.656;Float;False;Property;_ColorChange;ColorChange;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RefractOpVec;176;2162.107,709.9203;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;143;-324.5811,1041.458;Float;True;Property;_TextureSample0;Texture Sample 0;11;0;Create;True;0;0;False;0;0630d93062525384c9a6b26e4be9099a;0630d93062525384c9a6b26e4be9099a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;145;-894.228,817.4979;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;150;114.4162,467.6751;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2969.551,502.3519;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Water/WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;SrcAlpha;One;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;0;-1;0;0;0;False;0;0;0;False;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;73;0;74;0
WireConnection;7;0;73;0
WireConnection;83;0;82;0
WireConnection;119;0;46;0
WireConnection;119;1;82;0
WireConnection;119;2;31;0
WireConnection;118;0;31;0
WireConnection;118;1;46;0
WireConnection;45;0;119;0
WireConnection;84;0;7;0
WireConnection;84;1;83;0
WireConnection;44;0;7;0
WireConnection;44;1;45;0
WireConnection;85;0;84;0
WireConnection;32;0;118;0
WireConnection;27;0;7;0
WireConnection;27;1;32;0
WireConnection;43;0;44;0
WireConnection;81;0;80;0
WireConnection;81;1;9;0
WireConnection;81;2;85;0
WireConnection;41;0;42;0
WireConnection;41;1;81;0
WireConnection;41;2;43;0
WireConnection;30;0;27;0
WireConnection;174;0;173;0
WireConnection;144;0;135;0
WireConnection;144;1;146;0
WireConnection;157;0;156;0
WireConnection;162;0;161;0
WireConnection;162;1;171;0
WireConnection;168;0;164;0
WireConnection;164;0;163;0
WireConnection;161;0;160;0
WireConnection;161;1;152;0
WireConnection;156;0;155;0
WireConnection;151;0;150;0
WireConnection;151;1;148;0
WireConnection;171;0;143;2
WireConnection;163;0;162;0
WireConnection;133;0;135;0
WireConnection;133;1;134;0
WireConnection;155;0;153;0
WireConnection;153;0;151;0
WireConnection;153;1;172;0
WireConnection;169;0;149;0
WireConnection;169;1;167;0
WireConnection;167;0;144;0
WireConnection;167;1;166;0
WireConnection;167;2;168;0
WireConnection;148;0;136;0
WireConnection;148;1;147;0
WireConnection;160;0;143;1
WireConnection;11;0;10;0
WireConnection;11;1;41;0
WireConnection;11;2;30;0
WireConnection;149;0;133;0
WireConnection;149;1;159;0
WireConnection;149;2;157;0
WireConnection;172;0;124;2
WireConnection;152;0;148;0
WireConnection;124;0;129;0
WireConnection;124;1;134;0
WireConnection;146;0;145;0
WireConnection;176;0;174;0
WireConnection;176;1;175;0
WireConnection;176;2;177;0
WireConnection;143;0;129;0
WireConnection;143;1;146;0
WireConnection;150;0;124;1
WireConnection;0;0;11;0
WireConnection;0;4;66;0
WireConnection;0;8;176;0
ASEEND*/
//CHKSM=85F21BFBCBCA30B99CA6B86FE319B2E7A0E30A74