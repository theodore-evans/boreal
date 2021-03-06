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
    float initialMass;

    public float currErosion;
    public float sedimentMass { get; set; }

    public float mass { get => initialMass; }

    public List<Vector3> path;

    public RollingDroplet(Vector3 initialPosition, Vector3 initialVelocity, float initialMass)
    {
        currErosion = 0;
        sedimentMass = 0;
        position = initialPosition;
        velocity = initialVelocity;
        this.initialMass = initialMass;
        path = new List<Vector3>();
    }

    public void UpdateMovement(ISurface surface, float dt, float friction, float g)
    {
        lastSurface = surface;
        downhill = surface.Downhill;
        normal = surface.Normal;
        gravity = -Vector3.forward * g;

        Vector3 force3d = Vector3.Dot(downhill, gravity) * downhill - friction * Vector3.Dot(-normal, gravity) * velocity; //
        Vector3 force = new Vector3(force3d.x, force3d.y);

        acceleration = force / mass;
        velocity += acceleration * dt;
        position += velocity * dt;
    }

    internal void Freeze()
    {
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }
}
