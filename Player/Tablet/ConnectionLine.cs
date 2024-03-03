using Godot;

namespace Ductworks.Player.Tablet;

public partial class ConnectionLine : Line2D
{
	public bool enabled;

	public Area2D Area2D;

	private Vector2 startPos;

	public override void _Ready()
	{
		this.Width = 5.0f;
	}

	public override void _Process(double delta)
	{
		if (this.enabled)
		{
			this.ClearPoints();
			this.AddPoint(this.startPos);
			this.AddPoint(this.GetGlobalMousePosition());
		}
	}

	public void StartLine(Vector2 pos)
	{
		this.startPos = pos;
		this.enabled = true;
	}

	public void FinishLine(Vector2 pos)
	{
		this.ClearPoints();
		this.AddPoint(this.startPos);
		this.AddPoint(pos);
		this.enabled = false;
		
		// Add collision
		var collision = new CollisionShape2D();
		var a = this.Points[0];
		var b = this.Points[1];
		collision.Position = (a + b) / 2;
		collision.Rotation = a.DirectionTo(b).Angle();
		
		var collisionShape = new RectangleShape2D();
		collisionShape.Size = new Vector2(a.DistanceTo(b), this.Width);
		collision.Shape = collisionShape;

		this.Area2D = new Area2D();
		this.Area2D.AddChild(collision);
		this.AddChild(this.Area2D);
	}
}
