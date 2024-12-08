#version 330 core

in vec3 aVert;
in vec2 aTex;
in vec3 aColor;

out vec2 _tex;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(aVert, 1.0) * model * view * projection;
    _tex = aTex;
}