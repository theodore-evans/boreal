using System;
using UnityEngine;

public class Relief: AbstractTileObservable
{
    private float _elevation = 0.5f;
    private Vector3 _normal = Vector3.forward;

    internal Relief(Tile parent) : base(parent) { }

    public float Elevation { get => _elevation; set => SetObservableProperty(ref _elevation, value); }
    public Vector3 Normal
    {
        get => _normal; set {
            _normal = value;
            Gradient = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(value, Vector3.forward));
        }
    }
    public float Gradient { get; protected set; }
}
