using GameEngine.Core.Components;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GameEngine.Components.Objects;

public class UIObject : IDisposable
{
    public int Id { get; }
    public readonly List<UIComponent> components = new List<UIComponent>();

    public bool SyncGraphics { get; set; } = true;
    public bool SyncPhysics { get; set; }
    public bool SyncSound { get; set; }

    public bool TransformDirty { get; private set; }
    public Transform Transform { get; }

    public bool MeshesDirty { get; private set; }
    public ObservableCollection<MeshData> Meshes { get; }

    public UIObject()
    {
        Id = IdGenerator.GenerateNext();

        Transform = new Transform();
        Transform.PropertyChanged += TransformChanged;

        Meshes = new ObservableCollection<MeshData>();
        Meshes.CollectionChanged += MeshesChanged;
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

    public void Dispose()
    {

    }
}