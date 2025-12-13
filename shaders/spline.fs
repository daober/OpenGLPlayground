#version 330 core
out vec4 FragColor;

in vec3 WorldPos;

uniform vec4  splineCoeff;  // a, b, c, d
uniform float laneWidth;
uniform float laneLength;
uniform float laneDir;
uniform vec4  laneColor;

uniform vec3 carPos;        // base of lane (HUD anchor)
uniform vec3 carForward;    // normalized
uniform vec3 carRight;      // normalized

float cubicSpline(float z)
{
    return splineCoeff.x*z*z*z +
           splineCoeff.y*z*z +
           splineCoeff.z*z +
           splineCoeff.w;
}

void main()
{
    // transform fragment into car-local coordinates
    vec3 rel = WorldPos - carPos;

    float z = dot(rel, carForward); // forward distance
    float x = dot(rel, carRight);   // lateral offset

    if (z < 0.0 || z > laneLength)
        discard;

    float curveX = laneDir * cubicSpline(z);
    float dist   = abs(x - curveX);

    // soft lane edges
    float edge = smoothstep(laneWidth, laneWidth * 0.7, dist);

    // fade start & end
    float zn = z / laneLength;
    float fadeIn  = smoothstep(0.0, 0.1, zn);
    float fadeOut = smoothstep(1.0, 0.8, zn);

    float alpha = edge * fadeIn * fadeOut;

    if (alpha < 0.01)
        discard;

    FragColor = vec4(laneColor.rgb, laneColor.a * alpha);
}
