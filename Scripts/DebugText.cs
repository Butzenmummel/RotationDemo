using Godot;
using System;

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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var rotationDefault = trackerDefault.RotationDegrees;
		var rotationUltimate = trackerUltimate.RotationDegrees;


		labelDefaultX.Text = $"X: {Mathf.Round(rotationDefault.X)} ";
		labelDefaultY.Text = $"Y: {Mathf.Round(rotationDefault.Y)} ";
		labelDefaultZ.Text = $"Z: {Mathf.Round(rotationDefault.Z)} ";

		labelUltimateX.Text = $"X: {Mathf.Round(rotationUltimate.X)} ";
		labelUltimateY.Text = $"Y: {Mathf.Round(rotationUltimate.Y)} ";
		labelUltimateZ.Text = $"Z: {Mathf.Round(rotationUltimate.Z)} ";

	}
}
