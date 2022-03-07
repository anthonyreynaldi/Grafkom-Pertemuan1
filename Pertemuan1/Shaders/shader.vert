#version 330 core

		//location		type data     in artinya nanti masuk ke shader.vert
layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec3 aColor;

out vec4 vertexColor;

void main (void)
{
	//harus vec4 supaya bisa diterima x y z w. w -> 1
	gl_Position = vec4(aPosition, 1.0);

	//vertexColor = vec4(aColor, 1.0);
	vertexColor = vec4(1.0, 1.0, 0.0, 1.0);
}