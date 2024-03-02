extends Node3D

signal tablet_is_receiving_input(new_toggle)

@export var is_input_enabled = false
@export var is_mouse_inside: bool

@onready var node_screen_mesh = $TabletScreenMesh
@onready var node_screen_mesh_area = $TabletScreenMesh/Area3D
@onready var node_viewport = $TabletViewport

var last_event_pos2D = null
var last_event_time: float = -1.0

func _ready():
	#node_viewport.set_clear_mode(SubViewport.CLEAR_MODE_ALWAYS)
	
	# Godot currently gives a nonsense error if you try to set the viewport material via the inspector, so we'll wait two frames and then create a new material and set it ourselves.
	# The error doesn't actually seem to impact anything? But why risk it
	# The side-effect is that now we have to make any edits to this material in code instead of the inspector...
	# Tracker link: https://github.com/godotengine/godot/issues/66247
	
	# Assign texture here in order to make it shut up.
	# Note if I continue doing this I'll have to do it for the emissive texture too I guess
	#node_screen_mesh.get_surface_override_material(0).albedo_texture = node_viewport.get_texture()
	#node_screen_mesh.get_surface_override_material(0).albedo_texture_force_srgb = true
	
	node_screen_mesh_area.mouse_entered.connect(self.on_mouse_entered_area)
	node_screen_mesh_area.mouse_exited.connect(self.on_mouse_exited_area)
	node_screen_mesh_area.input_event.connect(self.handle_mouse_input_event)
	
	
func set_input_enabled(input_enabled: bool):
	self.is_input_enabled = input_enabled
	if !input_enabled:
		tablet_is_receiving_input.emit(false)
	

func on_mouse_entered_area():
	self.is_mouse_inside = true

func on_mouse_exited_area():
	self.is_mouse_inside = false

func handle_mouse_input_event(_camera: Camera3D, event: InputEvent, event_position: Vector3, _normal: Vector3, _shape_idx: int):
	# Taken from godot sample scene: https://github.com/godotengine/godot-demo-projects/tree/a69b2f7e215b1d5432959091ec90eb2b0044610c/viewport/gui_in_3d
	
	# When the tablet is enabled, the screen scene seems to immediately get a mouseenter event at (0,0) no matter what.
	# So use this signal to disable mouse events explicitly until the tablet has gotten its first real input event
	tablet_is_receiving_input.emit(true)
	
	var quad_mesh_size = node_screen_mesh.mesh.size
	var event_pos3D = event_position

	var now: float = Time.get_ticks_msec() / 1000.0

	# Convert position to a coordinate space relative to the Area3D node.
	# NOTE: affine_inverse accounts for the Area3D node's scale, rotation, and position in the scene!
	event_pos3D = node_screen_mesh.global_transform.affine_inverse() * event_pos3D

	var event_pos2D: Vector2 = Vector2()

	if is_mouse_inside:
		# Convert the relative event position from 3D to 2D.
		event_pos2D = Vector2(event_pos3D.x, -event_pos3D.y)

		# Right now the event position's range is the following: (-quad_size/2) -> (quad_size/2)
		# We need to convert it into the following range: -0.5 -> 0.5
		event_pos2D.x = event_pos2D.x / quad_mesh_size.x
		event_pos2D.y = event_pos2D.y / quad_mesh_size.y
		# Then we need to convert it into the following range: 0 -> 1
		event_pos2D.x += 0.5
		event_pos2D.y += 0.5

		# Finally, we convert the position to the following range: 0 -> viewport.size
		event_pos2D.x *= node_viewport.size.x
		event_pos2D.y *= node_viewport.size.y
		# We need to do these conversions so the event's position is in the viewport's coordinate system.
	elif last_event_pos2D != null:
		# Fall back to the last known event position.
		event_pos2D = last_event_pos2D

	# Set the event's position and global position.
	event.position = event_pos2D
	if event is InputEventMouse:
		event.global_position = event_pos2D

	# Calculate the relative event distance.
	if event is InputEventMouseMotion or event is InputEventScreenDrag:
		# If there is not a stored previous position, then we'll assume there is no relative motion.
		if last_event_pos2D == null:
			event.relative = Vector2(0, 0)
		# If there is a stored previous position, then we'll calculate the relative position by subtracting
		# the previous position from the new position. This will give us the distance the event traveled from prev_pos.
		else:
			event.relative = event_pos2D - last_event_pos2D
			event.velocity = event.relative / (now - last_event_time)

	# Update last_event_pos2D with the position we just calculated.
	last_event_pos2D = event_pos2D

	# Update last_event_time to current time.
	last_event_time = now

	# Finally, send the processed input event to the viewport.
	node_viewport.push_input(event)
