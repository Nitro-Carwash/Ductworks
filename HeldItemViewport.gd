extends SubViewportContainer
@onready var player_camera = $"../Head/PlayerCamera"
@onready var viewport_camera = $SubViewport/HeldItemCamera

# Called when the node enters the scene tree for the first time.
func _ready():
	$SubViewport.size = Vector2i(ProjectSettings.get_setting("display/window/size/viewport_width"), ProjectSettings.get_setting("display/window/size/viewport_height"))
	
func _process(delta):
	viewport_camera.global_transform = player_camera.global_transform
