#version 400

in vec2 texCoord;

uniform sampler2D _texture;

out vec4 FragColor;

void main()
{
    FragColor = vec4(0, 1, 0, 1);
}