using System;
using System.Collections.Generic;
using UnityEngine;

public class RollingDroplet // TODO? droplet as subclass? or compositi
{
    public Vector3 position { get; protected set; }
    public Vector3 velocity { get; protected set; }
    public Vector3 acceleration { get; protected set; }

    public ISurface lastSurface { get; protected set; }

    Vector3 downhill;
    Vector3 normal;
    Vector3 gravity;

    public float currErosion;
    public float sedimentMass;

    public List<Vector3> path;

    public RollingDroplet(Vector3 initialPosition, Vector3 initialVelocity)
    {
        currErosion = 0;
        sedimentMass = 0;
        position = initialPosition;
        velocity = initialVelocity;
        path = new List<Vector3>();
    }

    public void UpdateMovement(ISurface surface, float dt, float friction, float g)
    {
        lastSurface = surface;
        downhill = surface.Downhill;
        normal = surface.Normal;
        gravity = -Vector3.forward * g;

        Vector3 acc3d = Vector3.Dot(downhill, gravity) * downhill - friction * Vector3.Dot(-normal, gravity) * velocity; //
        acceleration = new Vector3(acc3d.x, acc3d.y);

        velocity += acceleration * dt;
        position += velocity * dt;
    }

    internal void Freeze()
    {
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }
}
