using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Ductworks.Player.Tablet;

public partial class ConnectionLineManager : Node2D
{
	public bool IsBlockingHovers => this.isInSeveringMode || (this.currentLine?.enabled ?? false);
	
	public SeverLine SeverLine;

	private List<ConnectionLine> connections = new List<ConnectionLine>();

	private ConnectionLine currentLine;

	private bool isInSeveringMode;

	public override void _PhysicsProcess(double delta)
	{
		// Connections are too thin to be reliable severed with MouseEnter events, so use a raycast using the
		// pointer history in SeverLine
		if (this.isInSeveringMode && this.SeverLine.TryGetLastTwoPoints(out Vector2 a, out Vector2 b))
		{
			Array<Rid> exceptions = new Array<Rid>();
			Vector2 start = a;
			bool hasRemainingSpaceToCheck = true;
			while (hasRemainingSpaceToCheck)
			{
				hasRemainingSpaceToCheck = false;
				
				var spaceState = this.GetWorld2D().DirectSpaceState;
				var query = PhysicsRayQueryParameters2D.Create(start, b, 1 << 1, exceptions);
				query.CollideWithAreas = true;
				var result = spaceState.IntersectRay(query);
				if (result.Count > 0)
				{
					hasRemainingSpaceToCheck = true;
					start = (Vector2 )result["position"];
					exceptions.Add((Rid)result["rid"]);
					var collider = (Node2D)result["collider"];
					this.RemoveConnection((ConnectionLine)collider.GetParent(), start);
				}
			}
		}
	}

	public void HandleClickOnNothing()
	{
		if (this.currentLine != null)
		{
			this.currentLine.ClearPoints();
			this.currentLine.enabled = false;
		}
		this.ToggleSeveringMode(true);
	}

	public bool HandleClickOnButton(TabletButton target)
	{
		bool wasConnectionMade = false;
		if (target.PuzzleElement.CanStartPowerConnection)
		{
			if (this.currentLine == null)
			{
				this.currentLine = new ConnectionLine();
				this.currentLine.ZIndex = -10;
				this.AddChild(this.currentLine);
			}
			if (!this.currentLine.enabled)
			{
				this.currentLine.StartLine(target.Position);
			}
			this.ToggleSeveringMode(false);
		}
		else if (target.PuzzleElement.CanReceivePowerConnection)
		{
			if (this.currentLine != null && this.currentLine.enabled)
			{
				this.currentLine.FinishLine(target.Position);
				this.connections.Add(this.currentLine);
				this.currentLine = null;
				wasConnectionMade = true;
			}
		}
		else
		{
			// Do this if we have new tablet buttons that should cancel
			this.HandleClickOnNothing();
		}

		return wasConnectionMade;
	}

	public void HandleReleaseOnNothing()
	{
		if (this.currentLine != null)
		{
			this.currentLine.ClearPoints();
			this.currentLine.enabled = false;
		}
		this.ToggleSeveringMode(false);
	}
	
	public bool HandleReleaseOnButton(TabletButton button)
	{
		return this.HandleClickOnButton(button);
	}

	private void RemoveConnection(ConnectionLine connectionRemoved, Vector2 severPosition)
	{
		this.connections.Remove(connectionRemoved);
		connectionRemoved.Kill(severPosition);
	}

	private void ToggleSeveringMode(bool isToggled)
	{
		this.isInSeveringMode = isToggled;
		this.SeverLine.Enabled = isToggled;
		foreach (var connection in this.connections)
		{
			connection.ToggleCollision(isToggled);
		}
	}
}
