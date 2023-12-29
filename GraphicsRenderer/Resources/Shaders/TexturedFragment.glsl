#version 400

in vec2 TexCoord;

uniform sampler2D _texture;

out vec4 FragColor;

void main()
{
    FragColor = texture(_texture, TexCoord);
}