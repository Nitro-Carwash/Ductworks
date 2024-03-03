using Ductworks.PuzzleElement;
using Godot;

namespace Ductworks.Player.Tablet;

public partial class TabletButton : Area2D
{
	[Signal]
	public delegate void MouseEnteredTabletButtonEventHandler(TabletButton tabletButton);
	
	[Signal]
	public delegate void MouseExitedTabletButtonEventHandler(TabletButton tabletButton);
	
	public PuzzleElementBase PuzzleElement { get; private set; }
	
	private RotationButton rotateRight, rotateLeft;
	
	private bool isHovered;

	private TabletScreen tabletScreen;
	
	public override void _Ready()
	{
		this.MouseEntered += this.HandleMouseover;
		this.MouseExited += this.HandleMouseExit;
	}

	public void Initialize(PuzzleElementBase puzzleElement, Vector2 tabletTileDimensions, PackedScene rotateArrowScene, TabletScreen tabletScreen)
	{
		this.PuzzleElement = puzzleElement;
		this.tabletScreen = tabletScreen;
		
		var spriteScale = tabletTileDimensions / 80;
        
		var sprite = puzzleElement.OrientationSprites[puzzleElement.GetOrientation()];
		Sprite2D sprite2D = new Sprite2D();
		sprite2D.Texture = sprite;
		sprite2D.Scale = spriteScale;

		this.Position = tabletTileDimensions * puzzleElement.GridPosition + tabletTileDimensions / 2;
		this.AddSprite(sprite2D, tabletTileDimensions);

		if (puzzleElement.GetNumberOfOrientations > 1)
		{
			Node rotateArrowSceneRoot = rotateArrowScene.Instantiate();
			
			this.rotateRight = new RotationButton();
			this.rotateRight.Initialize(rotateArrowSceneRoot.GetChild(0), this);
			this.AddChild(this.rotateRight);
			
			this.rotateLeft = new RotationButton();
			this.rotateRight.Initialize(rotateArrowSceneRoot.GetChild(1), this);
			this.AddChild(this.rotateLeft);
		}

		this.InputPickable = false;
	}
	
	public void ToggleEnabled(bool shouldBeEnabled)
	{
		this.InputPickable = shouldBeEnabled;
		if (!shouldBeEnabled)
		{
			this.rotateRight?.OnParentHoverUpdated(false);
			this.rotateLeft?.OnParentHoverUpdated(false);
		}
	}
	
	public void HandleRotationButtonMouseExit()
	{
		// If nothing to do with this tablet is hovered, then emit the signal.
		if (!this.isHovered && (this.rotateRight == null || !this.rotateRight.IsHovered) && (this.rotateLeft == null || !this.rotateLeft.IsHovered))
		{
			this.EmitSignal(SignalName.MouseExitedTabletButton, this);
		}
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

	public void HandleMouseover()
	{
		this.isHovered = true;
		if (this.tabletScreen.GetAreMouseoversAllowedToBeDrawn(this))
		{
			this.rotateRight?.OnParentHoverUpdated(true);
			this.rotateLeft?.OnParentHoverUpdated(true);
		}
		this.EmitSignal(SignalName.MouseEnteredTabletButton, this);
	}
	
	private void HandleMouseExit()
	{
		this.isHovered = false;
		this.rotateRight?.OnParentHoverUpdated(false);
		this.rotateLeft?.OnParentHoverUpdated(false);
		this.HandleRotationButtonMouseExit();
	}
}
