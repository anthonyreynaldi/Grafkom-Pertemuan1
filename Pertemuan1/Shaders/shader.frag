#version 330

out vec4 outputColor;

in vec4 vertexColor;

uniform vec4 ourColor;

void main()
{
//	outputColor = vec4(1.0, 1.0, 1.0, 1.0);		//white color
	//outputColor = vertexColor;		//white color
	outputColor = ourColor;		//white color
}