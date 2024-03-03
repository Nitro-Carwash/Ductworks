using Godot;
using System.Collections.Generic;
using Ductworks.PuzzleElement;

public partial class ConnectionManager : Node
{
	[Export]
	private PackedScene laserMesh;
	
	private readonly List<Connection> connections = new List<Connection>();
	
	public void AttachConnection(PuzzleElementBase from, PuzzleElementBase to)
	{
		var newConnection = new Connection { from = from, to = to, node = (Node3D)this.laserMesh.Instantiate() };
		newConnection.node.Position = (from.GetConnectionPoint() + to.GetConnectionPoint()) / 2;
		((CylinderMesh)newConnection.Mesh.Mesh).Height = from.GetConnectionPoint().DistanceTo(to.GetConnectionPoint());
		this.AddChild(newConnection.node);
		newConnection.node.LookAt(to.GetConnectionPoint());
		this.connections.Add(newConnection);
	}
	
	public void RemoveConnection(PuzzleElementBase from, PuzzleElementBase to)
	{
		this.connections.RemoveAll(c => c.from == from && c.to == to);
	}
}

public struct Connection
{
	public PuzzleElementBase from, to;

	public Node3D node;

	public MeshInstance3D Mesh => (MeshInstance3D)this.node.GetChild(0);
}
