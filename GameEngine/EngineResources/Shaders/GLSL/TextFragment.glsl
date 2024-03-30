#version 400

out vec4 FragColor;

uniform vec4 textColor; // Text color

void main()
{
    FragColor = textColor;
}