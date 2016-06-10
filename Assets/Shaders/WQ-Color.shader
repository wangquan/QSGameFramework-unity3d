Shader "WQ/WQ-Color" {
	Properties {
		_Color ("Main Color", COLOR) = (1,1,1,1)
	}
	SubShader {
		Lighting Off
		Fog { Mode Off }
		Pass {
			Color [_Color]
		}
	}
}
