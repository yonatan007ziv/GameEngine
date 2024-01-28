#version 400

layout(location=0) in vec3 positions;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
	gl_Position = vec4(positions, 1) * model * view * projection;
}