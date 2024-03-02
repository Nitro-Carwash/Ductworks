using Godot;

namespace Ductworks.PuzzleElement;

public abstract partial class PuzzleElementBase : Node3D
{
	[Export]
	public Texture2D[] OrientationSprites;

	[Export]
	public Vector2 GridPosition = new Vector2(-1, -1);

	public virtual int GetOrientation()
	{
		return 0;
	}

	public int GetNumberOfOrientations => this.OrientationSprites?.Length ?? 0;

	public virtual Vector3 GetConnectionPoint()
	{
		return this.GlobalPosition;
	}

	public abstract void HandleOrientationChange(int newOrientation);
}
