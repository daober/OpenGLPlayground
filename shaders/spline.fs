#version 330 core
out vec4 FragColor;

in vec3 WorldPos;

uniform vec4  splineCoeff;   // a,b,c,d
uniform float laneWidth;
uniform float laneLength;
uniform float laneDir;
uniform vec4  laneColor;

uniform vec3 carPos;
uniform vec3 carForward;
uniform vec3 carRight;

uniform float time;
uniform float animSpeed;

uniform float laneTime;
uniform float laneAnimDuration;

float cubicSpline(float z)
{
    return splineCoeff.x*z*z*z +
           splineCoeff.y*z*z +
           splineCoeff.z*z +
           splineCoeff.w;
}

void main()
{
    vec3 rel = WorldPos - carPos;

    float z = dot(rel, carForward);
    float x = dot(rel, carRight);

    // Smooth growth 0 → laneLength
    float grow01 = clamp(laneTime / laneAnimDuration, 0.0, 1.0);
    float currentLength = laneLength * smoothstep(0.0, 1.0, grow01);

    if (z < 0.0 || z > currentLength)
        discard;

    float curveX = laneDir * cubicSpline(z);
    float dist   = abs(x - curveX);

    float edge = smoothstep(laneWidth, laneWidth * 0.7, dist);

    float tipFade = smoothstep(
        currentLength,
        currentLength - 0.4,
        z
    );

    float alpha = edge * tipFade;

    if (alpha < 0.01)
        discard;

    FragColor = vec4(laneColor.rgb, laneColor.a * alpha);
}