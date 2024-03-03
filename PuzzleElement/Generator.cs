using System.Collections.Generic;
using Godot;

namespace Ductworks.PuzzleElement;

public partial class Generator : PuzzleElementBase
{
	[Export]
	private Node3D connectPoint;
	
	public override void HandleOrientationChange(int newOrientation) { }

	public override Vector3 GetConnectionPoint()
	{
		return this.connectPoint.GlobalPosition;
	}

	private List<PuzzleElementBase> connections = new List<PuzzleElementBase>();

	public override void EstablishConnection(PuzzleElementBase other)
	{
		this.EmitSignal(SignalName.OnConnectionEstablished, this, other);
	}
}
