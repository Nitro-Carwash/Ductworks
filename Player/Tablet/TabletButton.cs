﻿using Ductworks.PuzzleElement;
using Godot;

namespace Ductworks.Player.Tablet;

public partial class TabletButton : Area2D
{
	private RotationButton rotateRight, rotateLeft;
	
	public override void _Ready()
	{
		this.MouseEntered += this.HandleMouseover;
		this.MouseExited += this.HandleMouseExit;
	}

	public void Initialize(PuzzleElementBase puzzleElement, Vector2 tabletTileDimensions, PackedScene rotateArrowScene)
	{
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
			this.rotateRight.Initialize(rotateArrowSceneRoot.GetChild(0));
			this.AddChild(this.rotateRight);
			
			this.rotateLeft = new RotationButton();
			this.rotateRight.Initialize(rotateArrowSceneRoot.GetChild(1));
			this.AddChild(this.rotateLeft);
		}

		this.InputPickable = false;
	}
	
	public void ToggleEnabled(bool shouldBeEnabled)
	{
		this.InputPickable = shouldBeEnabled;
		if (!shouldBeEnabled)
		{
			this.rotateRight?.ToggleVisibility(false);
			this.rotateLeft?.ToggleVisibility(false);
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

	private void HandleMouseover()
	{
		GD.Print("Hovered");
		this.rotateRight?.ToggleVisibility(true);
		this.rotateLeft?.ToggleVisibility(true);
	}
	
	private void HandleMouseExit()
	{
		GD.Print("Unhovered");
		this.rotateRight?.ToggleVisibility(false);
		this.rotateLeft?.ToggleVisibility(false);
	}
}
