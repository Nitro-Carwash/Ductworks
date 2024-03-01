extends CharacterBody3D

@export var vertical_angle_limit := 90.0
@export var mouse_sensitivity := 2.0
@export var speed := 10
@export var acceleration := 8
@export var deceleration := 10

func _physics_process(delta):
	var movement_axis = Input.get_vector("move_forward", "move_backward", "move_left", "move_right")
	
	# Get direction we're moving in based on facing direction
	var aim := self.get_global_transform().basis
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
	
func _input(event):
	if event is InputEventMouseMotion:
		var rotation_y = self.rotation.y - event.relative.x * (mouse_sensitivity/1000)
		var rotation_x = clamp(self.rotation.x - event.relative.y * (mouse_sensitivity/1000), -vertical_angle_limit, vertical_angle_limit)
		self.rotation.y = rotation_y
		self.rotation.x = rotation_x
