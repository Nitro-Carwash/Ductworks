extends Node3D

func _ready():
	# Godot currently gives a nonsense error if you try to set the viewport material via the inspector, so we'll wait two frames and then create a new material and set it ourselves.
	# The error doesn't actually seem to impact anything? But why risk it
	# The side-effect is that now we have to make any edits to this material in code instead of the inspector...
	# Tracker link: https://github.com/godotengine/godot/issues/66247
	
	# Let two frames pass to make sure the viewport is captured.
	await get_tree().process_frame
	await get_tree().process_frame
	
	# Assign texture here in order to make it shut up.
	# Note if I continue doing this I'll have to do it for the emissive texture too I guess
	$TabletScreenMesh.get_surface_override_material(0).albedo_texture = $TabletViewport.get_texture()
	$TabletScreenMesh.get_surface_override_material(0).albedo_texture_force_srgb = true
		
