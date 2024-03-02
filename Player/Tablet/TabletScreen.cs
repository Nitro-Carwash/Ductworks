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

	public void AddPuzzleElementToScreen(PuzzleElementBase puzzleElement, int tileCountX, int tileCountZ)
	{
		int maxX = this.screenViewport.Size.X;
		int maxZ = this.screenViewport.Size.Y;
		Vector2 tabletTileDimensions = new Vector2((float)maxX / tileCountX, (float)maxZ / tileCountZ);

		TabletButton button = new TabletButton();
		button.Initialize(puzzleElement, tabletTileDimensions, rotateArrowTexture, rotateArrowOffset, rotateArrowScale);
		this.AddChild(button);
	}
}