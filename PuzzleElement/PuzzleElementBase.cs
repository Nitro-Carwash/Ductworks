using Godot;

namespace Ductworks.PuzzleElement;

public abstract partial class PuzzleElementBase : Node3D
{
	[Signal]
	public delegate void OnConnectionEstablishedEventHandler(PuzzleElementBase from, PuzzleElementBase to);
	
	[Signal]
	public delegate void OnConnectionRemovedEventHandler(PuzzleElementBase from, PuzzleElementBase to);
	
	[Export]
	public Texture2D[] OrientationSprites;

	[Export]
	public Vector2 GridPosition = new Vector2(-1, -1);

	[Export]
	public bool CanStartPowerConnection = false;
	
	[Export]
	public bool CanReceivePowerConnection = false;
	
	public int GetNumberOfOrientations => this.OrientationSprites?.Length ?? 0;

	public abstract void HandleOrientationChange(int newOrientation);

	public virtual int GetOrientation()
	{
		return 0;
	}
	
	public virtual Vector3 GetConnectionPoint()
	{
		return this.GlobalPosition;
	}
	
	public virtual void EstablishConnection(PuzzleElementBase other) {}
	
	public virtual void EndConnection(PuzzleElementBase other) { }
}
