extends Area2D

func _ready():
	self.mouse_entered.connect(self.mouse_entered_inner)
	
func mouse_entered_inner():
	print("hovered")
