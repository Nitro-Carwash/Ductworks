extends Node3D


# Called when the node enters the scene tree for the first time.
func _ready():
	# Godot currently gives a nonsense error if you try to set the viewport material via the inspector, so we'll wait two frames and then create a new material and set it ourselves.
	# The error doesn't actually seem to impact anything? But why risk it
	# The side-effect is that now we have to make any edits to this material in code instead of the inspector...
	# Bug link: https://github.com/godotengine/godot/issues/66247
	
	# Let two frames pass to make sure the viewport is captured.
	await get_tree().process_frame
	await get_tree().process_frame
	
	# Create new material for the viewport
	var viewport_material : StandardMaterial3D = StandardMaterial3D.new()
	viewport_material.albedo_texture = $TabletViewport.get_texture()
	viewport_material.albedo_texture_force_srgb = true
	$TabletQuad.set_surface_override_material(0, viewport_material)
