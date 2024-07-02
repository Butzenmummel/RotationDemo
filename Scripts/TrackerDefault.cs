using Godot;
using System;

public partial class TrackerDefault : Node3D
{
	DebugDraw3DScopeConfig currentConfig;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentConfig = DebugDraw3D.ScopedConfig();
        currentConfig.SetThickness(0.05f);
		currentConfig.SetNoDepthTest(true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DebugDraw3D.DrawGizmo(Transform, is_centered:true);

		if (Name == "TrackerDefault")
		{
			DebugDraw3D.DrawArrowRay(GlobalPosition, -GlobalTransform.Basis.Z, 2f, Colors.Yellow, 0.1f);
		}
		else
		{
			DebugDraw3D.DrawArrowRay(GlobalPosition, GlobalTransform.Basis.Y, 2f, Colors.Yellow, 0.1f);
		}
	}
}
