#version 330 core
out vec4 FragColor;

in vec3 WorldPos;

/* Mock cubic spline: x = f(z) */
float spline(float z)
{
    float a =  0.02;
    float b = -0.15;
    float c =  0.0;
    float d =  0.0;
    return a*z*z*z + b*z*z + c*z + d;
}

void main()
{
    float x = WorldPos.x;
    float z = WorldPos.z;

    float curveX = spline(z);
    float dist   = abs(x - curveX);

    float width = 0.15;

    if (dist > width)
        discard;

    FragColor = vec4(0.1, 0.4, 1.0, 0.7);
}
