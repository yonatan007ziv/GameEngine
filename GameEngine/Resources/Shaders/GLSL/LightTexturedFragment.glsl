#version 400

in vec2 TexCoord;
in vec3 Normal;

uniform sampler2D _texture;

out vec4 FragColor;

void main()
{
    vec3 lightDir =  vec3(1, 1, 1);
    float diffuse = max(dot(Normal, lightDir), 0.0);
    vec3 diffuseColor = vec3(1, 1, 1) * diffuse;
    vec4 textureColor = texture(_texture, TexCoord);
    vec4 finalColor = textureColor * vec4(diffuseColor, 1.0);
    FragColor = finalColor;
}