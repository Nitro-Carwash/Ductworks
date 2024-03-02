using Godot;
using Ductworks.PuzzleElement;

public partial class TabletScreen : Node2D
{
	[Export]
	private SubViewport screenViewport;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddPuzzleElementToScreen(PuzzleElement puzzleElement, int tileCountX, int tileCountZ)
	{
		int maxX = screenViewport.Size.X;
		int maxZ = this.screenViewport.Size.Y;

		Vector2 tabletTileDimensions = new Vector2((float)maxX / tileCountX, (float)maxZ / tileCountZ);

		var spriteScale = tabletTileDimensions / 80;
        
		var sprite = puzzleElement.OrientationSprites[puzzleElement.GetOrientation()];
		Sprite2D sprite2D = new Sprite2D();
		sprite2D.Texture = sprite;
		sprite2D.Scale = spriteScale;
		sprite2D.Position = tabletTileDimensions * puzzleElement.GridPosition + tabletTileDimensions / 2;
		this.AddChild(sprite2D);
	}
}
