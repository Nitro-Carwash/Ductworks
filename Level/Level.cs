using Godot;
using System.Collections.Generic;
using System.Linq;
using Ductworks.PuzzleElement;

public partial class Level : Node3D
{
	[Export]
	private MeshInstance3D Floor;
	
	[Signal]
	public delegate void PuzzleElementLoadedEventHandler(PuzzleElement puzzleElements, int tileCountX, int tileCountZ);

	private int tileCountX;
	private int tileCountZ;

	private IEnumerable<PuzzleElement> puzzleElements;
	
	public override void _Ready()
	{
		this.tileCountX = (int)((PlaneMesh)this.Floor.Mesh).Size.X / 4;
		this.tileCountZ = (int)((PlaneMesh)this.Floor.Mesh).Size.Y / 4;

		this.puzzleElements = this.GetChildren().OfType<PuzzleElement>();
		Vector2I positionToGridOffset = new Vector2I(2, 2);
		Vector2I gridOffset = new Vector2I((this.tileCountX / 2) - 1, (this.tileCountZ / 2) - 1);
		foreach (PuzzleElement p in this.puzzleElements)
		{
			Vector2I newPosition = new Vector2I((int)p.Position.X, (int)p.Position.Z);
			newPosition += positionToGridOffset;
			newPosition /= 4;
			newPosition += gridOffset;
			p.GridPosition = newPosition;
			
			this.EmitSignal(SignalName.PuzzleElementLoaded, p, this.tileCountX, this.tileCountZ);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
