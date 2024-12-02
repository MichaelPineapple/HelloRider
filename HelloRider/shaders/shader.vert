#version 330 core

layout (location = 0) in vec3 vert;
layout (location = 1) in vec2 tex;
layout (location = 2) in vec3 color;

out vec3 _color;
out vec2 _tex;

void main()
{
    gl_Position = vec4(vert, 1.0);
    _color = color;
    _tex = tex;
}