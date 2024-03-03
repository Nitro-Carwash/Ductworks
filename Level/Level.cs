using Godot;
using System.Collections.Generic;
using System.Linq;
using Ductworks.PuzzleElement;

public partial class Level : Node3D
{
	[Signal]
	public delegate void PuzzleElementLoadedEventHandler(PuzzleElementBase puzzleElementsBase, int tileCountX, int tileCountZ);
	
	[Signal]
	public delegate void LevelLoadedEventHandler(Vector2 mapSize);
	
	[Export]
	private MeshInstance3D Floor;

	[Export]
	private ConnectionManager connectionManager;

	private int tileCountX;
	private int tileCountZ;

	private IEnumerable<PuzzleElementBase> puzzleElements;
	
	public override void _Ready()
	{
		Vector2 floorSize = new Vector2(((PlaneMesh)this.Floor.Mesh).Size.X, ((PlaneMesh)this.Floor.Mesh).Size.Y);
		this.tileCountX = (int)floorSize.X / 4;
		this.tileCountZ = (int)floorSize.Y / 4;

		this.puzzleElements = this.GetChildren().OfType<PuzzleElementBase>();
		Vector2I positionToGridOffset = new Vector2I(2, 2);
		Vector2I gridOffset = new Vector2I((this.tileCountX / 2) - 1, (this.tileCountZ / 2) - 1);
		foreach (PuzzleElementBase p in this.puzzleElements)
		{
			Vector2I newPosition = new Vector2I((int)p.Position.X, (int)p.Position.Z);
			newPosition += positionToGridOffset;
			newPosition /= 4;
			newPosition += gridOffset;
			p.GridPosition = newPosition;
			p.OnConnectionEstablished += this.connectionManager.AttachConnection;
			
			this.EmitSignal(SignalName.PuzzleElementLoaded, p, this.tileCountX, this.tileCountZ);
		}
		this.EmitSignal(SignalName.LevelLoaded, floorSize);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
