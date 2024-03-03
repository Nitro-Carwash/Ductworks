using Ductworks.PuzzleElement;
using Godot;

namespace Ductworks.Player.Tablet;

public partial class ConnectionLine : Line2D
{
	private readonly float deathCutoffLength = 50f;
	private readonly float deathSegmentLerpRate = 5f;
	
	public bool enabled;
	
	public Area2D Area2D;

	public PuzzleElementBase puzzleElementA, PuzzleElementB;

	private Vector2 startPos;

	private bool isDead;

	private Line2D deathAssistant;

	private Vector2 deathA, deathB, deathSeverPointA, deathSeverPointB;

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
		else if (this.isDead)
		{
			this.ClearPoints();
			this.AddPoint(this.deathA);
			this.deathSeverPointA = this.deathSeverPointA.Lerp(this.deathA, (float)delta * this.deathSegmentLerpRate);
			this.AddPoint(this.deathSeverPointA);
			bool segmentADone = this.deathSeverPointA.DistanceSquaredTo(this.deathA) <= this.deathCutoffLength;
			
			this.deathAssistant.ClearPoints();
			this.deathAssistant.AddPoint(this.deathB);
			this.deathSeverPointB = this.deathSeverPointB.Lerp(this.deathB, (float)delta * this.deathSegmentLerpRate);
			this.deathAssistant.AddPoint(this.deathSeverPointB);
			bool segmentBDone = this.deathSeverPointB.DistanceSquaredTo(this.deathB) <= this.deathCutoffLength;

			if (segmentADone && segmentBDone)
			{
				this.QueueFree();
			}
		}
	}

	public void StartLine(Vector2 pos, PuzzleElementBase startPuzzleElement)
	{
		this.startPos = pos;
		this.puzzleElementA = startPuzzleElement;
		this.enabled = true;
	}

	public void FinishLine(Vector2 pos, PuzzleElementBase endPuzzleElement)
	{
		this.PuzzleElementB = endPuzzleElement;
		
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
		this.Area2D.CollisionLayer = 2;
		this.AddChild(this.Area2D);
		this.ToggleCollision(false);
		
		this.puzzleElementA.EstablishConnection(this.PuzzleElementB);
	}

	public void ToggleCollision(bool isToggled)
	{
		this.Area2D.InputPickable = isToggled;
		// No impact on visuals during an actual build, but makes debugging easier
		this.Area2D.Visible = isToggled;
	}

	public void Kill(Vector2 severPoint)
	{
		this.isDead = true;
		this.deathA = this.Points[0];
		this.deathB = this.Points[1];
		this.deathSeverPointA = this.deathSeverPointB = severPoint;
		this.deathAssistant = new Line2D();
		this.deathAssistant.Width = this.Width;
		this.AddChild(this.deathAssistant);
		this.ToggleCollision(false);
	}
}
