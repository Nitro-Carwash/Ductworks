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

	private readonly List<TabletButton> buttons = new List<TabletButton>();

	private bool tabletIsEnabled = false;

	public void AddPuzzleElementToScreen(PuzzleElementBase puzzleElement, int tileCountX, int tileCountZ)
	{
		int maxX = this.screenViewport.Size.X;
		int maxZ = this.screenViewport.Size.Y;
		Vector2 tabletTileDimensions = new Vector2((float)maxX / tileCountX, (float)maxZ / tileCountZ);

		TabletButton button = new TabletButton();
		button.Initialize(puzzleElement, tabletTileDimensions, this.rotateArrowTexture, this.rotateArrowOffset, this.rotateArrowScale);
		this.AddChild(button);
		this.buttons.Add(button);
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
}