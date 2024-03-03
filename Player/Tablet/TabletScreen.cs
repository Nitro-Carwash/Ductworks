using System.Collections.Generic;
using Ductworks.PuzzleElement;
using Godot;

namespace Ductworks.Player.Tablet;

public partial class TabletScreen : Node2D
{
	[Export]
	private SubViewport screenViewport;

	[Export]
	private Texture2D rotateArrowTexture;

	[Export]
	private Vector2 rotateArrowOffset;
	
	[Export]
	private Vector2 rotateArrowScale;

	[Export]
	private PackedScene rotateArrowScene;

	private readonly List<TabletButton> buttons = new List<TabletButton>();
	
	private readonly ConnectionLineManager connectionLineManager = new ConnectionLineManager();

	private bool tabletIsEnabled = false;

	private TabletButton currentlyHoveredTabletButton;

	public override void _Ready()
	{
		this.connectionLineManager.Owner = this;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			var mouseButtonEvent = (InputEventMouseButton)@event;
			if (mouseButtonEvent.IsActionPressed("tablet_interact", allowEcho: false, exactMatch: false))
			{
				if (this.currentlyHoveredTabletButton != null)
				{
					this.connectionLineManager.HandleClickOnButton(this.currentlyHoveredTabletButton);
				}
			}
			else if (mouseButtonEvent.IsActionReleased("tablet_interact", exactMatch: false))
			{
				if (this.currentlyHoveredTabletButton != null)
				{
					this.connectionLineManager.HandleClickOnButton(this.currentlyHoveredTabletButton);
				}
			}
		}
	}

	public void AddPuzzleElementToScreen(PuzzleElementBase puzzleElement, int tileCountX, int tileCountZ)
	{
		int maxX = this.screenViewport.Size.X;
		int maxZ = this.screenViewport.Size.Y;
		Vector2 tabletTileDimensions = new Vector2((float)maxX / tileCountX, (float)maxZ / tileCountZ);

		TabletButton button = new TabletButton();
		button.Initialize(puzzleElement, tabletTileDimensions, this.rotateArrowScene);
		this.AddChild(button);
		this.buttons.Add(button);
		button.MouseEnteredTabletButton += this.HandlePuzzleButtonMouseEnter;
		button.MouseExitedTabletButton += this.HandlePuzzleButtonMouseExit;
	}

	// When the tablet is enabled, the screen scene seems to immediately get a mouseenter event at (0,0) no matter what.
	// So use this function to disable mouse events explicitly until the tablet has gotten its first real input event.
	public void ToggleScreenEnabled(bool shouldBeEnabled)
	{
		// Ignore redundant signals 
		if (shouldBeEnabled == this.tabletIsEnabled)
		{
			return;
		}

		foreach (TabletButton button in this.buttons)
		{
			button.ToggleEnabled(shouldBeEnabled);
		}
		
		this.tabletIsEnabled = shouldBeEnabled;
	}

	private void HandlePuzzleButtonMouseEnter(TabletButton instigator)
	{
		// Disable picking/input on all buttons but the currently hovered.
		this.currentlyHoveredTabletButton = instigator;
		foreach (var button in this.buttons)
		{
			if (button == instigator)
			{
				continue;
			}
			button.InputPickable = false;
		}
	}
	
	private void HandlePuzzleButtonMouseExit(TabletButton instigator)
	{
		this.currentlyHoveredTabletButton = null;
		foreach (var button in this.buttons)
		{
			button.InputPickable = true;
		}
	}
}