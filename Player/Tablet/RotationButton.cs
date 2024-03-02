using Godot;

namespace Ductworks.Player.Tablet;

public partial class RotationButton : Area2D
{
	public override void _Ready()
	{
		this.MouseEntered += () => GD.Print("ArrowHovered");
	}

	public void Initialize(Texture2D texture, Vector2 scale, float rotation)
	{
		Sprite2D buttonSprite = new Sprite2D();
		buttonSprite.Texture = texture;
		buttonSprite.Scale = scale;
		buttonSprite.Rotation = rotation;
		this.AddChild(buttonSprite);

		var buttonDimensions = buttonSprite.Texture.GetSize() * scale;
		var collision = new CollisionShape2D();
		var shape = new RectangleShape2D();
		shape.Size = buttonDimensions;
		collision.Shape = shape;
		this.AddChild(collision);
		
		this.ToggleVisibility(false);
	}

	public void ToggleVisibility(bool shouldBeVisible)
	{
		this.InputPickable = shouldBeVisible;
		this.Visible = shouldBeVisible;
	}
}
