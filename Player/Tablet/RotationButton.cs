using Godot;

namespace Ductworks.Player.Tablet;

public partial class RotationButton : Area2D
{
	public bool IsHovered { get; private set; }
	private bool isParentHovered;

	private TabletButton owner;
	
	public override void _Ready()
	{
		this.MouseEntered += this.HandleMouseEnter; 
		this.MouseExited += this.HandleMouseExit;
	}

	public void Initialize(Node arrowRoot, TabletButton owner)
	{
		foreach (Node child in arrowRoot.GetChildren())
		{
			child.Reparent(this, false);
		}

		this.owner = owner;
		this.ToggleVisibility(false);
	}

	public void OnParentHoverUpdated(bool newIsParentHovered)
	{
		this.isParentHovered = newIsParentHovered;
		this.ToggleVisibility(this.isParentHovered || this.IsHovered);
	}
	
	private void ToggleVisibility(bool shouldBeVisible)
	{
		this.InputPickable = shouldBeVisible;
		this.Visible = shouldBeVisible;
	}

	private void HandleMouseEnter()
	{
		this.IsHovered = true;
	}
	
	private void HandleMouseExit()
	{
		this.IsHovered = false;
		if (!this.isParentHovered)
		{
			this.ToggleVisibility(false);
			this.owner.HandleRotationButtonMouseExit();
		}
	}
}
