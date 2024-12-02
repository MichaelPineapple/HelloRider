#version 330 core

out vec4 FragColor;

in vec3 _color;
in vec2 _tex;

uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    FragColor = mix(texture(texture0, _tex), texture(texture1, _tex), 0.2);
}