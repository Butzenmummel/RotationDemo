using Godot;
using System;

public partial class CoordinateSystemOverlay : Node3D
{
	DebugDraw3DScopeConfig currentConfig;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentConfig = DebugDraw3D.ScopedConfig();
        currentConfig.SetThickness(0.03f);
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		currentConfig.SetThickness(0.03f);
		DebugDraw3D.DrawArrowRay(Vector3.Zero, Vector3.Right, 6f, new Color(1,0,0), 0.05f);
		DebugDraw3D.DrawArrowRay(Vector3.Zero, Vector3.Up, 6f, new Color(0,1,0), 0.05f);
		DebugDraw3D.DrawArrowRay(Vector3.Zero, Vector3.Forward, 6f, new Color(0,0,1), 0.05f);

		currentConfig.SetThickness(0.01f);
		currentConfig.SetNoDepthTest(false);
		//DebugDraw3D.DrawGrid(Vector3.Zero, Vector3.Up * 10, Vector3.Right * 10, new Vector2I(10, 10), new Color(0, 0, 1, 0.2f));
		//DebugDraw3D.DrawGrid(Vector3.Zero, Vector3.Up * 10, Vector3.Forward * 10, new Vector2I(10, 10), new Color(1, 0, 0, 0.2f));
		DebugDraw3D.DrawGrid(Vector3.Zero, Vector3.Right * 100, Vector3.Forward * 100, new Vector2I(100, 100), new Color(0.1f, 0.1f, 0.1f, 0.2f));
	}
}
