using Godot;

namespace Ductworks.Player.Tablet;

public partial class RotationButton : Area2D
{
	public override void _Ready()
	{
		this.MouseEntered += () => GD.Print("ArrowHovered");
	}

	public void Initialize(Node arrowRoot)
	{
		foreach (Node child in arrowRoot.GetChildren())
		{
			child.Reparent(this, false);
		}
		
		this.ToggleVisibility(false);
	}

	public void ToggleVisibility(bool shouldBeVisible)
	{
		this.InputPickable = shouldBeVisible;
		this.Visible = shouldBeVisible;
	}
}
