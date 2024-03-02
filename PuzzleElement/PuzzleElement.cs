using Godot;

namespace Ductworks.PuzzleElement;

public abstract partial class PuzzleElement : Node3D
{
	[Export]
	public Texture2D[] OrientationSprites;

	public virtual int GetOrientation()
	{
		return -1;
	}

	public abstract void HandleOrientationChange(int newOrientation);
}
