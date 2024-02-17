using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;

namespace GameEngine.Components.Objects;

public class WorldObject : IDisposable
{
    public int Id { get; }
    public readonly List<WorldComponent> components = new List<WorldComponent>();

    public bool SyncGraphics { get; set; } = true;
    public bool SyncPhysics { get; set; }
    public bool SyncSound { get; set; }

    public bool TransformDirty { get; private set; }
    public Transform Transform { get; }

    public bool MeshesDirty { get; private set; }
    public ObservableCollection<MeshData> Meshes { get; }

    public bool ForcesDirty { get; private set; }
    public ObservableCollection<Vector3> Forces { get; }

    public bool ImpulseVelocitiesDirty { get; private set; }
    public ObservableCollection<Vector3> ImpulseVelocities { get; }

    public WorldObject()
    {
        Id = IdGenerator.GenerateNext();

        Transform = new Transform();
        Transform.PropertyChanged += TransformChanged;

        Meshes = new ObservableCollection<MeshData>();
        Meshes.CollectionChanged += MeshesChanged;

        Forces = new ObservableCollection<Vector3>();
        Forces.CollectionChanged += ForcesChanged;

        ImpulseVelocities = new ObservableCollection<Vector3>();
        ImpulseVelocities.CollectionChanged += ImpulseVelocitiesChanged;
    }

    private void TransformChanged(object? sender, PropertyChangedEventArgs e)
    {
        TransformDirty = true;
        SyncPhysics = true;
        SyncGraphics = true;
    }

    private void MeshesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        MeshesDirty = true;
        SyncGraphics = true;
    }

    private void ForcesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ForcesDirty = true;
        SyncPhysics = true;
    }

    private void ImpulseVelocitiesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ImpulseVelocitiesDirty = true;
        SyncPhysics = true;
    }

    internal void ResetPhysicsSoundDirty()
    {
        ForcesDirty = false;
        ImpulseVelocitiesDirty = false;
        SyncPhysics = false;
        SyncSound = false;
    }

    public void Dispose()
    {
        Meshes.Clear();
        Forces.Clear();
        ImpulseVelocities.Clear();
    }
}