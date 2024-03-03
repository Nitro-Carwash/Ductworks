using Godot;

namespace Ductworks.Player.Tablet;

public partial class ConnectionLine : Line2D
{
	public bool enabled;

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
}
