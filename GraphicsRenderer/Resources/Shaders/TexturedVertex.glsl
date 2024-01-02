#version 400

layout(location=0) in vec3 positions;
layout(location=1) in vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 TexCoord;

void main()
{
	gl_Position = vec4(positions, 1) * model * view * projection;
	TexCoord = texCoord;
}