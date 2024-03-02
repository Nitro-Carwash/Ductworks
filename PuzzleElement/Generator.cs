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
	
	public void ConnectTo(PuzzleElementBase target)
	{
		GD.Print(target.GetConnectionPoint().ToString());
		GD.Print(this.GetConnectionPoint().ToString());
		
		OrmMaterial3D material = new OrmMaterial3D();
		material.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
		material.AlbedoColor = Colors.Aquamarine;
		
		var immediateMesh = new ImmediateMesh();
		immediateMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, material);
		immediateMesh.SurfaceAddVertex(this.GetConnectionPoint());
		immediateMesh.SurfaceAddVertex(target.GetConnectionPoint());
		immediateMesh.SurfaceEnd();

		var meshInstance = new MeshInstance3D();
		meshInstance.Mesh = immediateMesh;
		meshInstance.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;

		var oldTransform = meshInstance.GlobalPosition;
		this.AddChild(meshInstance);
		meshInstance.GlobalPosition = oldTransform;
	}
}
