using System.Collections.Generic;
using Godot;

namespace Ductworks.Player.Tablet;

public partial class SeverLine : Line2D
{
	[Export]
	private int capacity = 8;

	[Export]
	private float idleCursorSpacing = 1f;
	
	private readonly Queue<Vector2> pointQueue = new Queue<Vector2>();

	public bool Enabled;
	
	public override void _Process(double delta)
	{
		if (this.Enabled)
		{
			// Could be improved by modifying points in place (if that's possible?)
			this.pointQueue.Enqueue(this.GetGlobalMousePosition());
			// Use a while loop incase the capacity was updated during runtime (probably due to export var)
			while (this.pointQueue.Count > this.capacity)
			{
				this.pointQueue.Dequeue();
			}
		}
		else if (this.pointQueue.Count > 0)
		{
			this.pointQueue.Dequeue();
		}
		
		this.ClearPoints();
		if (this.pointQueue.Count > 0)
		{
			foreach (var point in this.pointQueue)
			{
				this.AddPoint(point);
			}

			if (this.Enabled && this.Points.Length > 1)
			{
				// Go over the points, and check if they're too close to each other
				float idleDistanceThreshold = 2 * this.idleCursorSpacing + 0.5f;
				Vector2 lastPoint = this.Points[1];
				bool allTooClose = true;
				for (int i = 1; i < this.GetPointCount(); i++)
				{
					if (lastPoint.DistanceTo(this.Points[i]) >= idleDistanceThreshold)
					{
						allTooClose = false;
						break;
					}
					lastPoint = this.Points[i];
				}

				if (allTooClose)
				{
					// Replace the queue until they move again so that it's not just a single spec
					this.pointQueue.Clear();
					this.pointQueue.Enqueue(this.GetGlobalMousePosition() + Vector2.Left * 2);
					this.pointQueue.Enqueue(this.GetGlobalMousePosition() + Vector2.Right * 2);
				}
			}
		}
	}
}
