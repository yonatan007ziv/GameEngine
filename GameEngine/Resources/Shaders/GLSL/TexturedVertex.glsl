#version 400

layout(location=0) in vec3 positions;
layout(location=1) in vec2 texCoord;
layout(location=2) in vec3 normal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec2 TexCoord;
out vec3 Normal;

void main()
{
	gl_Position = vec4(positions, 1) * model * view * projection;
	TexCoord = texCoord;
	Normal = mat3(model) * normal;
}