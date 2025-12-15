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

// NEW
uniform float time;
uniform float animSpeed;

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

    if (z < 0.0 || z > laneLength)
        discard;

    // smooth animation factor (0..1)
    float anim = sin(time * animSpeed) * 0.5 + 0.5;

    float curveX = laneDir * cubicSpline(z) * mix(0.2, 1.0, anim);
    float dist   = abs(x - curveX);

    float edge = smoothstep(laneWidth, laneWidth * 0.7, dist);

    float zn = z / laneLength;
    float fadeIn  = smoothstep(0.0, 0.1, zn);
    float fadeOut = smoothstep(1.0, 0.8, zn);

    float alpha = edge * fadeIn * fadeOut;

    if (alpha < 0.01)
        discard;

    FragColor = vec4(laneColor.rgb, laneColor.a * alpha);
}
