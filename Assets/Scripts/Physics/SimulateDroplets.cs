using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulateDroplets : MonoBehaviour
{
    SpaceGrid<Tile> world;

    [SerializeField] [Range(0, 1000)] float gravity = 10f;
    [SerializeField] [Range(0.001f, 0.2f)] float friction = 0.02f;
    [SerializeField] [Range(0,1000)] int numberOfParticles = 15;
    [SerializeField] [Range(0, 1)] float erosionStrength = 1.0f;
    [SerializeField] [Range(0, 1)] float maxErosion = 0.75f;
    [SerializeField] [Range(0, 1f)] float depositionLimit = 0.1f;
    [SerializeField] [Range(0, 1f)] float waterPerDroplet = 0.5f;
    [SerializeField] bool tracePaths = true;

    bool simulate = false;

    internal List<RollingDroplet> droplets;
    SimulateWaterFlow simulateFlow;

    public void StartSimulation()
    {
        world = GetComponent<WaterSimController>().world;
        simulateFlow = GetComponent<SimulateWaterFlow>();

        droplets = new List<RollingDroplet>();

        for (int i = 0; i < numberOfParticles; i++) {
            Vector3 randomPosition = new Vector3(Random.Range(0, world.GridSizeX), Random.Range(0, world.GridSizeY), -0.1f);
            droplets.Add(new RollingDroplet(randomPosition, Vector3.zero));
        }

        StartCoroutine(TracePath());
        simulate = true;
    }

    public void StopSimulation()
    {
        simulate = false;
    }

    public void DropAllWater()
    {
        foreach (RollingDroplet droplet in droplets.ToList()) {
            DropWater(droplet);
        }
    }

    IEnumerator TracePath()
    {
        for (; ; ) {
            if (gameObject.activeSelf && simulate && tracePaths) {
                foreach (RollingDroplet droplet in droplets) {
                    droplet.path.Add(droplet.position);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FixedUpdate()
    {
        if (simulate) {
            foreach (RollingDroplet droplet in droplets.ToList()) {

                Tile currTile = world.GetNodeAt(droplet.position);

                if (currTile != null) {
                    Tile lastTile = (Tile)droplet.lastSurface;
                    droplet.UpdateMovement(currTile, Time.fixedDeltaTime, friction, gravity);
                    ErodeSurface(droplet, currTile, lastTile);

                    if (currTile.Type == "Water") DropWater(droplet);
                }
                else {
                    DropWater(droplet);
                }
            }
        }
    }

    void DropWater(RollingDroplet droplet)
    {
        Tile endTile = world.GetNodeAt(droplet.position);

        if (endTile != null) {
            endTile.WaterDepth += waterPerDroplet;
            simulateFlow.openSet.Add(endTile);
            droplets.Remove(droplet);
        }
    }

    void ErodeSurface(RollingDroplet droplet, Tile currTile, Tile lastTile)
    {
        float currErosion = droplet.currErosion;
        droplet.currErosion = currErosion + erosionStrength * droplet.velocity.magnitude;

        if (lastTile != null && currTile != lastTile) {
            ApplyErosion(droplet, currTile, lastTile);
            DepositSediment(droplet, currTile, lastTile);
        }
    }

    void ApplyErosion(RollingDroplet droplet, Tile newTile, Tile oldTile)
    {
        float erosion = Mathf.Clamp(droplet.currErosion, 0, maxErosion);
        float heightChange = Mathf.Lerp(0, oldTile.Altitude - newTile.Altitude, erosion);

        oldTile.Altitude -= heightChange;
        droplet.sedimentMass += heightChange;

        droplet.currErosion = 0;
    }

    void DepositSediment(RollingDroplet droplet, Tile newTile, Tile oldTile)
    {
        float dropletSpeed = droplet.velocity.magnitude;
        if (dropletSpeed < depositionLimit) {
            float deposition = Mathf.Clamp(droplet.sedimentMass /  dropletSpeed, 0, maxErosion);
            float heightChange = Mathf.Lerp(0, oldTile.Altitude - newTile.Altitude, deposition);
            newTile.Altitude += heightChange;
            droplet.sedimentMass -= heightChange;
        }
    }

    public void OnDrawGizmos()
    {
        if (droplets != null) {
            foreach (RollingDroplet droplet in droplets) {

                Gizmos.DrawSphere(droplet.position, 0.25f);
                if (tracePaths) {
                    for (int i = 1; i < droplet.path.Count; i++) {
                        Gizmos.DrawLine(droplet.path[i - 1], droplet.path[i]);
                    }
                }
            }
        }
    }
}
