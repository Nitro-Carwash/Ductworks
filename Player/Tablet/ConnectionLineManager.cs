﻿using System.Collections.Generic;
using Godot;

namespace Ductworks.Player.Tablet;

public class ConnectionLineManager
{
	public Node2D Owner;

	private List<ConnectionLine> connections = new List<ConnectionLine>();

	private ConnectionLine currentLine;
	
	public ConnectionLineManager()
	{
		
	}

	public void HandleClickOnNothing()
	{
		
	}

	public void HandleClickOnButton(TabletButton target)
	{
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
		}
		else if (target.PuzzleElement.CanReceivePowerConnection)
		{
			if (this.currentLine != null && this.currentLine.enabled)
			{
				this.currentLine.FinishLine(target.Position);
				this.connections.Add(this.currentLine);
				this.currentLine = null;
			}
		}
	}
}