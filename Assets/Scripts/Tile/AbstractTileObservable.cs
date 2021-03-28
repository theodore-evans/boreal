using System;
using Extensions;

public abstract class AbstractTileObservable
{
    internal Tile parent;
    private float callbackTriggerThreshold = 0.0001f;

    protected AbstractTileObservable(Tile parent)
    {
        this.parent = parent;
    }

    internal void SetObservableProperty(ref float observedProperty, float newValue)
    {
        if (observedProperty.Similar(newValue, callbackTriggerThreshold) == false) {
            observedProperty = newValue;
            NotifyObservableChanged();
        }
    }

    internal void SetObservableProperty<T>(ref T observedProperty, T newValue)
    {
        if (observedProperty.Equals(newValue) == false) {
            observedProperty = newValue;
            NotifyObservableChanged();
        }
    }

    private void NotifyObservableChanged() => parent.OnObservableChanged();
}
