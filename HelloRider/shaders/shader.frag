#version 330 core

out vec4 FragColor;

in vec3 _color;
in vec2 _tex;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform vec4 color2;

void main()
{
    FragColor = mix(texture(texture0, _tex), texture(texture1, _tex), 0.75) * vec4(_color, 1.0) * color2;
}