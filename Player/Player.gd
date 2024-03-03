extends CharacterBody3D

signal player_moved(new_position: Vector3, new_rotation: Vector3)

@export var vertical_angle_limit := 90.0
@export var mouse_sensitivity := 2.0
@export var speed := 10
@export var acceleration := 8
@export var deceleration := 10
@export var is_tablet_toggled = false

@onready var head_node: Node3D = $Head
@onready var tablet_node = $Head/PlayerCamera/Tablet
var is_toggle_blocked = false

func _physics_process(delta):
	var movement_axis = Input.get_vector("move_forward", "move_backward", "move_left", "move_right")
	
	# Get direction we're moving in based on facing direction
	var aim : Basis = head_node.get_global_transform().basis
	var move_direction := Vector3()
	if abs(movement_axis.x) >= 0.5:
		move_direction += sign(movement_axis.x) * aim.z
	if abs(movement_axis.y) >= 0.5:
		move_direction += sign(movement_axis.y) * aim.x
	move_direction = move_direction.normalized()
	
	# Move
	var target_velocity = move_direction * speed
	var target_acceleration := acceleration if move_direction.dot(velocity) > 0 else deceleration
	velocity = velocity.lerp(target_velocity, target_acceleration * delta)
	velocity.y = 0
	move_and_slide()
	
	player_moved.emit(self.position, self.head_node.rotation)
	
func _input(event):
	if event is InputEventMouseMotion:
		if Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
			var rotation_y = head_node.rotation.y - event.relative.x * (mouse_sensitivity/1000)
			var rotation_x = clamp(head_node.rotation.x - event.relative.y * (mouse_sensitivity/1000), -deg_to_rad(vertical_angle_limit), deg_to_rad(vertical_angle_limit))
			head_node.rotation.y = rotation_y
			head_node.rotation.x = rotation_x
	if event.is_action_pressed("toggle_tablet"):
		if !is_toggle_blocked:
			is_tablet_toggled = !is_tablet_toggled
			if is_tablet_toggled:
				Input.set_mouse_mode(Input.MOUSE_MODE_CONFINED)
			else:
				Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
			tablet_node.set_input_enabled(is_tablet_toggled)
			is_toggle_blocked = true

func toggle_animation_complete_callback():
	is_toggle_blocked = false
