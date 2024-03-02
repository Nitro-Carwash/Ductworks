using Godot;

namespace Ductworks.PuzzleElement;

enum PipeLongOrientation
{
	NorthSouth = 0,
	EastWest = 1
}

public partial class PipeLong : Pipe
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override int GetOrientation()
	{
		return 0;
	}

	public override void HandleOrientationChange(int newOrientation)
	{
		
	}
}