using Godot;
using System;
using System.Diagnostics;

public partial class DebugText : Control
{
	Label labelDefaultX;
	Label labelDefaultY;
	Label labelDefaultZ;
	Label labelUltimateX;
	Label labelUltimateY;
	Label labelUltimateZ;

	Node3D trackerDefault;
	Node3D trackerUltimate;

	Transform3D transformDefaultZero;
	Transform3D transformUltimateZero;

	Quaternion initialRotationDefault;
	Quaternion initialRotationUltimate;

	Vector3 directionForwardDefault = new Vector3(0, 0, -1);
	Vector3 directionForwardUltimate = new Vector3(0, 1, 0);

	Node3D Button => GetNode<Node3D>("%Button");

	DebugDraw3DScopeConfig currentConfig;

	Plane planeButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		labelDefaultX = GetNode<Label>("VBoxContainer/HBoxDefault/LabelX");
		labelDefaultY = GetNode<Label>("VBoxContainer/HBoxDefault/LabelY");
		labelDefaultZ = GetNode<Label>("VBoxContainer/HBoxDefault/LabelZ");

		labelUltimateX = GetNode<Label>("VBoxContainer/HBoxUltimate/LabelX");
		labelUltimateY = GetNode<Label>("VBoxContainer/HBoxUltimate/LabelY");
		labelUltimateZ = GetNode<Label>("VBoxContainer/HBoxUltimate/LabelZ");

		trackerDefault = GetNode<Node3D>("%TrackerDefault");
		trackerUltimate = GetNode<Node3D>("%TrackerUltimate");

		currentConfig = DebugDraw3D.ScopedConfig();
        currentConfig.SetPlaneSize(1f);
		currentConfig.SetNoDepthTest(true);

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var rotationDefault = trackerDefault.RotationDegrees;
		var rotationUltimate = trackerUltimate.RotationDegrees;

		Plane planeUltimate = new Plane(trackerUltimate.GlobalTransform.Basis.Y, trackerUltimate.GlobalTransform.Origin);
		 // Assuming you want to track rotation around the local Y axis (0, 1, 0)
		Vector3 localYAxis = new Vector3(0, 1, 0);

		// Calculate the rotation around the local Y axis
		//float angleAroundYAxis = CalculateRotationAroundAxis(trackerUltimate, localYAxis);

		planeButton = new Plane((trackerUltimate.GlobalTransform.Origin - Button.GlobalTransform.Origin).Normalized(), Button.GlobalTransform.Origin);
		ProjectPoints();


		labelDefaultX.Text = $"X: {Mathf.Round(rotationDefault.X)} ";
		labelDefaultY.Text = $"Y: {Mathf.Round(rotationDefault.Y)} ";
		labelDefaultZ.Text = $"Z: {Mathf.Round(rotationDefault.Z)} ";

		labelUltimateX.Text  = "";
		labelUltimateY.Text = "";

		DebugDraw3D.DrawGizmo(transformDefaultZero, is_centered:true);
		DebugDraw3D.DrawGizmo(transformUltimateZero, is_centered:true);

		DebugDraw3D.DrawGrid(Button.GlobalTransform.Origin, Button.GlobalTransform.Basis.Y, Button.GlobalTransform.Basis.Z, new Vector2I(3,3), Colors.Yellow);
		DebugDraw3D.DrawPlane(planeButton, Colors.Red, Button.GlobalTransform.Origin);
	}

	private void ProjectPoints()
	{
		Vector3 origin = trackerUltimate.GlobalTransform.Origin;
		Vector3 up = trackerUltimate.GlobalTransform.Basis.Z;
		Vector3 upNegative = -trackerUltimate.GlobalTransform.Basis.Z;
		Vector3 right = trackerUltimate.GlobalTransform.Basis.X;
		Vector3 rightNegative = -trackerUltimate.GlobalTransform.Basis.X;

		Vector3 forwardGlobal = trackerUltimate.GlobalTransform.Basis.Y + trackerUltimate.GlobalTransform.Origin;
		Vector3 backwardGlobal = -trackerUltimate.GlobalTransform.Basis.Y + trackerUltimate.GlobalTransform.Origin;



		if (forwardGlobal.DistanceTo(Button.GlobalTransform.Origin) < backwardGlobal.DistanceTo(Button.GlobalTransform.Origin))
		{
			up = upNegative;
		}
	
		if (right.Y < rightNegative.Y)
		{
			right = rightNegative;
		}

		planeButton = new Plane((trackerUltimate.GlobalTransform.Origin - Button.GlobalTransform.Origin).Normalized(), Button.GlobalTransform.Origin);

		Vector3 originProjected = planeButton.Project(origin);
		Vector3 upProjected = planeButton.Project(origin + up);
		Vector3 rightProjected = planeButton.Project(origin + right);
		
		DebugDraw3D.DrawLine(origin, originProjected, Colors.Gray);
		DebugDraw3D.DrawLine(origin + up, upProjected, Colors.Gray);
		//DebugDraw3D.DrawLine(origin + right, rightProjected, Colors.Gray);

		DebugDraw3D.DrawLine(originProjected, upProjected, Colors.Cyan);
		DebugDraw3D.DrawLine(Button.GlobalTransform.Origin, Button.GlobalTransform.Origin + Vector3.Up, Colors.Magenta);


		DebugDraw3D.DrawPoints(new Vector3[] {originProjected, upProjected}, DebugDraw3D.PointType.TypeSphere, 0.1f ,color:Colors.White);

		Vector3 trackerUp = upProjected - originProjected;
		Vector3 buttonUp = Button.GlobalTransform.Origin + Vector3.Up - Button.GlobalTransform.Origin;

		float angle = trackerUp.AngleTo(buttonUp);

		var angleNormals = (Button.GlobalTransform.Origin - trackerUltimate.GlobalTransform.Origin).AngleTo(trackerUltimate.GlobalTransform.Basis.Y);
		if (Mathf.RadToDeg(angleNormals) > 50)
		{
			angle = 0;
		}

		labelUltimateZ.Text = $"Angle: {Mathf.RadToDeg(angle)} ";
	}

	public float CalculateRotationAroundAxis(Node3D vrController, Vector3 axis)
	{
		// Get the current rotation quaternion
		Quaternion currentRotation = vrController.GlobalTransform.Basis.GetRotationQuaternion();

		// Calculate the relative rotation quaternion
		Quaternion relativeRotation = initialRotationUltimate.Inverse() * currentRotation;

		// Isolate the rotation around the specified axis
		Vector3 axisNormalized = axis.Normalized();
		float angleAroundAxis = relativeRotation.GetAxis().Dot(axisNormalized) * relativeRotation.GetAngle();

		return angleAroundAxis;
	}

	private void OnButtonPressed()
	{
		directionForwardDefault = trackerDefault.GlobalTransform.Basis.Z;
		transformDefaultZero = trackerDefault.GlobalTransform;
		Debug.Print($"TrackerDefault zeroed at {transformDefaultZero}");

		directionForwardUltimate = trackerUltimate.GlobalTransform.Basis.Y;
		transformUltimateZero = trackerUltimate.GlobalTransform;
		Debug.Print($"TrackerUltimate zeroed at {transformUltimateZero}");

		initialRotationDefault = trackerDefault.GlobalTransform.Basis.GetRotationQuaternion();
		initialRotationUltimate = trackerUltimate.GlobalTransform.Basis.GetRotationQuaternion();
		
	}
}
