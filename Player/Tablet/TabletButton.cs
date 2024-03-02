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

	public void Initialize(PuzzleElementBase puzzleElement, Vector2 tabletTileDimensions, Texture2D rotateArrowTexture, Vector2 rotateArrowOffset, Vector2 rotateArrowScale)
	{
		var spriteScale = tabletTileDimensions / 80;
        
		var sprite = puzzleElement.OrientationSprites[puzzleElement.GetOrientation()];
		Sprite2D sprite2D = new Sprite2D();
		sprite2D.Texture = sprite;
		sprite2D.Scale = spriteScale;

		this.Position = tabletTileDimensions * puzzleElement.GridPosition + tabletTileDimensions / 2;
		this.AddSprite(sprite2D, tabletTileDimensions);

		if (puzzleElement.GetNumberOfOrientations > 0)
		{
			Sprite2D rightArrow = new Sprite2D();
			rightArrow.Texture = rotateArrowTexture;
			rightArrow.Scale = spriteScale * rotateArrowScale;
			rightArrow.Rotation = Mathf.DegToRad(90f);
			this.AddChild(rightArrow);
			rightArrow.Position += rotateArrowOffset;
			
			
			Sprite2D leftArrow = new Sprite2D();
			leftArrow.Texture = rotateArrowTexture;
			leftArrow.Scale = spriteScale * rotateArrowScale;
			leftArrow.Rotation = Mathf.DegToRad(-90f);
			this.AddChild(leftArrow);
			leftArrow.Position -= rotateArrowOffset;
		}
	}
}
