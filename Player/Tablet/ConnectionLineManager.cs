using System.Collections.Generic;
using Godot;

namespace Ductworks.Player.Tablet;

public class ConnectionLineManager
{
	public Node2D Owner;

	public bool IsBlockingHovers => this.isInSeveringMode || (this.currentLine?.enabled ?? false);
	
	public SeverLine SeverLine;

	private List<ConnectionLine> connections = new List<ConnectionLine>();

	private ConnectionLine currentLine;

	private bool isInSeveringMode;
	
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
				this.Owner.AddChild(this.currentLine);
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
				this.currentLine.ConnectionRemoved += this.HandleConnectionRemoval;
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
		this.ToggleSeveringMode(false);
	}
	
	public bool HandleReleaseOnButton(TabletButton button)
	{
		return this.HandleClickOnButton(button);
	}

	private void HandleConnectionRemoval(ConnectionLine connectionRemoved)
	{
		this.connections.Remove(connectionRemoved);
		connectionRemoved.Kill(this.Owner.GetGlobalMousePosition());
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
