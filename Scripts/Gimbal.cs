using Godot;
using System;
using System.Diagnostics;

public partial class Gimbal : Node3D
{
	Area3D gimbalX;
	Area3D gimbalY;
	Area3D gimbalZ;

	[Export]
	StandardMaterial3D redMaterial;
	[Export]
	StandardMaterial3D greenMaterial;
	[Export]
	StandardMaterial3D blueMaterial;

	Vector3 initialClickDirection;
	Vector3 currentClickDirection;

	Vector2 initialMousePosition2D;
	Vector2 currentMousePosition2D;

	bool isXHovered = false;
	bool isYHovered = false;
	bool isZHovered = false;

	bool isXClicked = false;
	bool isYClicked = false;
	bool isZClicked = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gimbalX = GetNode<Area3D>("GimbalX");
		gimbalY = GetNode<Area3D>("GimbalY");
		gimbalZ = GetNode<Area3D>("GimbalZ");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isXHovered)
		{
			gimbalX.GetNode<MeshInstance3D>("MeshInstance3D").SetSurfaceOverrideMaterial(0, redMaterial);
		}
		else if (!isXHovered && !isXClicked)
		{
			gimbalX.GetNode<MeshInstance3D>("MeshInstance3D").SetSurfaceOverrideMaterial(0, null);
		}

		if (isYHovered)
		{
			gimbalY.GetNode<MeshInstance3D>("MeshInstance3D").SetSurfaceOverrideMaterial(0, greenMaterial);
		}
		else if (!isYHovered && !isYClicked)
		{
			gimbalY.GetNode<MeshInstance3D>("MeshInstance3D").SetSurfaceOverrideMaterial(0, null);
		}

		if (isZHovered)
		{
			gimbalZ.GetNode<MeshInstance3D>("MeshInstance3D").SetSurfaceOverrideMaterial(0, blueMaterial);
		}
		else if (!isZHovered && !isZClicked)
		{
			gimbalZ.GetNode<MeshInstance3D>("MeshInstance3D").SetSurfaceOverrideMaterial(0, null);
		}

		if (isXClicked)
		{
			currentMousePosition2D = GetViewport().GetMousePosition() - GetObjectScreenPosition();
			var angle = initialMousePosition2D.AngleTo(currentMousePosition2D);
			GetParent<Node3D>().RotateObjectLocal(Vector3.Right, -angle);
			initialMousePosition2D = currentMousePosition2D;
		}


		if (isYClicked)
		{
			currentMousePosition2D = GetViewport().GetMousePosition() - GetObjectScreenPosition();
			var angle = initialMousePosition2D.AngleTo(currentMousePosition2D);
			GetParent<Node3D>().RotateObjectLocal(Vector3.Up, -angle);
			initialMousePosition2D = currentMousePosition2D;
		}

		if (isZClicked)
		{
			currentMousePosition2D = GetViewport().GetMousePosition() - GetObjectScreenPosition();
			var angle = initialMousePosition2D.AngleTo(currentMousePosition2D);
			GetParent<Node3D>().RotateObjectLocal(Vector3.Back, -angle);
			initialMousePosition2D = currentMousePosition2D;
		}

	}


	public void GimbalXInputEvent(Node camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, int shapeIdx)
	{
		initialClickDirection = clickPosition - GlobalTransform.Origin;

		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.Pressed)
			{
				initialMousePosition2D = mouseButton.Position - GetObjectScreenPosition();
				currentClickDirection = clickPosition - GlobalTransform.Origin;
				isXClicked = true;
			}
			else
			{
				currentClickDirection = Vector3.Zero; // or any other default value
				isXClicked = false;
				return;
			}
		}
	}

	public void GimbalYInputEvent(Node camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, int shapeIdx)
	{
		initialClickDirection = clickPosition - GlobalTransform.Origin;

		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.Pressed)
			{
				initialMousePosition2D = mouseButton.Position - GetObjectScreenPosition();
				currentClickDirection = clickPosition - GlobalTransform.Origin;
				isYClicked = true;
			}
			else
			{
				currentClickDirection = Vector3.Zero; // or any other default value
				isYClicked = false;
				return;
			}
		}
	}

	public void GimbalZInputEvent(Node camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, int shapeIdx)
	{
		initialClickDirection = clickPosition - GlobalTransform.Origin;

		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.Pressed)
			{
				initialMousePosition2D = mouseButton.Position - GetObjectScreenPosition();
				currentClickDirection = clickPosition - GlobalTransform.Origin;
				isZClicked = true;
			}
			else
			{
				currentClickDirection = Vector3.Zero; // or any other default value
				isZClicked = false;
				return;
			}
		}
	}



	public Vector2 GetObjectScreenPosition()
	{
		Vector3 objectCenter = GlobalTransform.Origin;

		var screenPos = GetViewport().GetCamera3D().UnprojectPosition(objectCenter);

		return screenPos;
	}

#region Mouse Detection
	public void GimbalXMouseEntered()
	{
		isXHovered = true;
	}

	public void GimbalXMouseExited()
	{
		isXHovered = false;
	}

	public void GimbalYMouseEntered()
	{
		isYHovered = true;
	}

	public void GimbalYMouseExited()
	{
		isYHovered = false;
	}
	public void GimbalZMouseEntered()
	{
		isZHovered = true;
	}

	public void GimbalZMouseExited()
	{
		isZHovered = false;
	}
#endregion
}
