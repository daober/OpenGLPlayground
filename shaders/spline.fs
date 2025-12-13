#version 330 core
out vec4 FragColor;

in vec3 WorldPos;

uniform vec4 splineCoeff;   // cubic spline coefficients (a,b,c,d)
uniform float laneWidth;    // half-width of the lane
uniform vec4 laneColor;     // RGBA color (ignore alpha if fully opaque)
uniform float laneDir;      // -1.0 = left, +1.0 = right
uniform float laneOffsetZ; 

float spline(float z)
{
    float x = splineCoeff.x * z*z*z +
              splineCoeff.y * z*z +
              splineCoeff.z * z +
              splineCoeff.w;

    return laneDir * x;
}

void main()
{
    float x = WorldPos.x;
    float z = WorldPos.z;

    float curveX = spline(z - laneOffsetZ);
    float dist   = abs(x - curveX);

    if (dist > laneWidth)
        discard;

    // fully opaque
    FragColor = vec4(laneColor.rgb, 1.0);
}
