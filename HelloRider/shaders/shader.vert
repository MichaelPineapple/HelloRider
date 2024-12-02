#version 330 core

in vec3 aVert;
in vec2 aTex;
in vec3 aColor;

out vec3 _color;
out vec2 _tex;

uniform vec3 offset;

void main()
{
    
    gl_Position = vec4(aVert + offset, 1.0);
    _color = aColor;
    _tex = aTex;
}