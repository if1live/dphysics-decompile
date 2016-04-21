using UnityEngine;
using System.Collections;
using DarkRift;
using System.Collections.Generic;
using DPhysics;

public class Client : MonoBehaviour
{
	#region Initialization
	public GameObject TestFab;
	public GameObject CustomTestObject;

	private void Start ()
	{
		Time.fixedDeltaTime = TargetSimulationRate;
		DarkRiftAPI.onData += HandleonData;
	}
	#endregion

	#region Communication
	public GUIStyle Style;
	private bool isStarted;
	private string _IP = "127.0.0.1";
	//GUI Stuff for connecting
	private void OnGUI ()
	{
		GUILayout.BeginArea (new Rect(Screen.width / 2 - 100, 0, 200,150));
		//If the player hasn't connected yet, allow the player to connect
		if (!DarkRiftAPI.isConnected ) {
			_IP = GUILayout.TextField (_IP);
			if (GUILayout.Button ("Connect")) {
				DarkRiftAPI.Connect (_IP);
				//Connect to the DarkRift server hosted on the machine with _IP IP Address
			}
		}
		if (!DoVisualize) {
			Texture2D tex = new Texture2D (1, 1);
			tex.SetPixel (1, 1, new Color32 (255, 255, 255, 255));
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), tex);
			GUILayout.Label ("Catching up", Style);
		}
		GUILayout.Label ("Step Count: " + StepCount.ToString (), Style);
		GUILayout.EndArea();
	}

	/// <summary>
	/// Handles data received from server.
	/// </summary>
	void HandleonData (byte tag, ushort frameCount, object data)
	{
		switch ((NetworkTag)tag) {
		//Meta data includes connection, room initialization, and subject.
		//In the current implementation, the server automatically puts a player in a room upon connecting
		//So there is no need for NetworkTag.Meta out of the box.
		case NetworkTag.Meta:

			break;

		//Game data includes sending input to the server and receiving frames from the server
		case NetworkTag.Game:
			//Checking if we already have the frame to prevent duplicate frames
			if (Frames.ContainsKey (frameCount))
				break;

			//Checks if this frame is the one we're expecting
			if (frameCount == NextStepCount) {
				Frames.Add (frameCount, new Frame ((byte[])data));
				NextStepCount++;
				ForeSight++;
				//Check how many frames in advance we have already stored
				for (ushort i = NextStepCount; i < Frames.Count; i++) {
					if (Frames.ContainsKey (i)) {
						NextStepCount++;
						ForeSight++;
					} else {
						break;
					}
				}
			} else if (frameCount > NextStepCount) {
				//If it's a future frame that we won't immediately use, store it for later
				Frames.Add (frameCount, new Frame ((byte[])data));
			}
			break;
		}
	}
	/// <summary>
	/// Sends a command to the server, to be distributed to everyone in the player's room.
	/// </summary>
	public static void SendCommand (Command com)
	{
		//Be sure to convert the list to an array since DarkRift can't send lists
		DarkRiftAPI.SendMessageToServer ((byte)NetworkTag.Game,(ushort)0,com.Serialized().ToArray ());
	}
	//Needed to prevent some nasty bugs
	private void OnApplicationQuit ()
	{
		DarkRiftAPI.Disconnect ();
	}
	#endregion

	#region Simulation
	/// <summary>
	/// The target simulation rate. In ideal networking conditions, simulation will run at this rate. Make sure you change this value on the server if you change it on the client.
	/// </summary>
	public const float TargetSimulationRate = .1f;
	/// <summary>
	/// Frames are stored here. When saving a game, this is what you would serialize and replay.
	/// </summary>
	public Dictionary<ushort,Frame> Frames = new Dictionary<ushort, Frame> ();
	/// <summary>
	/// Represents how many simulation frames have passed; given the same input and step, all clients will have the same game state.
	/// </summary>
	[HideInInspector]
	public static ushort StepCount{
		get{
			return _StepCount;
		}
	}
	private static ushort _StepCount = 0;
	private ushort NextStepCount = 0;
	private int ForeSight = 0;
	private bool DoVisualize = true;

	private void FixedUpdate ()
	{
		DarkRiftAPI.Recieve ();
		//Adjust the simulation rate based on how many frames in advance we have
		//This prevents lag stutters and promotes responsiveness
		//If we have 0 frames ahead, we cannot go ahead (hence the if statement)
		if (ForeSight > 0) {
			float NewSimRate = (TargetSimulationRate * 1.1f) - (ForeSight * TargetSimulationRate * .1f);
			if (NewSimRate < TargetSimulationRate / 2) {
				NewSimRate = TargetSimulationRate / 2;
				DoVisualize = false;
			} else {
				DoVisualize = true;
			}
			Time.fixedDeltaTime = NewSimRate;
			ForeSight--;

			//Get our current frame
			Frame CurrentFrame = Frames [_StepCount];
			//Get the commands of our current frame and execute them
			List<Command> CurrentCommands = Command.Deserialize (CurrentFrame._Data);
			foreach (Command com in CurrentCommands)
			{
				//Executes the command...
				//Explore to TestCommands.cs to find the game logic behind commands
				TestCommands.Execute(com);
			}

			//Spawns a few objects in the beginning
			if (_StepCount == 0) {
				TestSpawn ();
			}

			//Simulate the physics
			DPhysicsManager.Simulate ();

			//Increase our step and advance forward in time
			_StepCount++;
		}
	}
	/// <summary>
	/// Spawns and initializes 200 objects
	/// </summary>
	public void TestSpawn ()
	{
		const int Iterations = 200;
		FInt spacing = FInt.Create(1.5d);
		int Width = (int)Mathd.IntSqrt ((long)Iterations);
		int Height = Iterations / Width;
		for (int i = 0; i < Width; i++) {
			for (int j = 0; j < Height; j++) {
				Vector2d spawnVec = new Vector2d (spacing * i - Width / 2, spacing * j - Width / 2);
				Body body = Instantiate (TestFab).GetComponent<Body> ();
				body.Initialize (spawnVec);
				Vector2d randomVelocity = DPhysicsManager.RandomDirection * 4;
				body.ApplyVelocity (ref randomVelocity);

				#region Spawn your custom physics object here

				#endregion
			}
		}
	}
	#endregion

	#region Visualization: Communicating with Unity's rendering
	private void Update ()
	{
		if (DoVisualize)
			DPhysicsManager.Visualize ();
	}
	#endregion
}
