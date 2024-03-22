#version 400

in vec2 TexCoord;
in vec3 Normal;

uniform sampler2D _texture;

out vec4 FragColor;

void main()
{
    vec4 textureColor = texture(_texture, TexCoord);
    FragColor = textureColor;
}