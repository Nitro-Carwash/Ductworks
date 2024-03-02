using Ductworks.PuzzleElement;
using Godot;

namespace Ductworks.Player.Tablet;

public partial class TabletButton : Area2D
{
	public override void _Ready()
	{
		this.MouseEntered += () => { GD.Print("hover"); };
	}

	private void AddSprite(Sprite2D sprite, Vector2 size)
	{
		var collision = new CollisionShape2D();
		var shape = new RectangleShape2D();
		shape.Size = size;
		collision.Shape = shape;
		
		this.AddChild(collision);
		this.AddChild(sprite);
		
	}

	public void Initialize(PuzzleElementBase puzzleElementBase, Vector2 tabletTileDimensions)
	{
		var spriteScale = tabletTileDimensions / 80;
        
		var sprite = puzzleElementBase.OrientationSprites[puzzleElementBase.GetOrientation()];
		Sprite2D sprite2D = new Sprite2D();
		sprite2D.Texture = sprite;
		sprite2D.Scale = spriteScale;

		this.Position = tabletTileDimensions * puzzleElementBase.GridPosition + tabletTileDimensions / 2;
		this.AddSprite(sprite2D, tabletTileDimensions);
	}
}
