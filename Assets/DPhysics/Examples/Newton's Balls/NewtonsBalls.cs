using UnityEngine;
using System.Collections;
using DPhysics;

/// <summary>
/// Here's a neat example of what you can do with DPhysics that other physics engine can't.
/// Because DPhysics is deterministic, every replay and every calculation will be consistent.
/// This means you can create a perfect Newton's Cradle to demonstrate transfer of momentum!
/// </summary>
/// 
public class NewtonsBalls : MonoBehaviour
{
	public GameObject Ball;
	public GameObject Wall;

	// Spawns balls in rows and columns, and applies an initial force to a ball each row.
	void Starter (int rows, int columns, double speed)
	{
		//Initializing the simulation
		Time.fixedDeltaTime = .1f; //Setting our fixed update rate
		DPhysicsManager.SimulationDelta = FInt.Create (.1d); //Syncing DPhysics's simulation rate with Unity's fixed update
		DPhysicsManager.Restitution = FInt.OneF; //Restitution of 1 results in complete conservation of momentum
		DPhysicsManager.CollisionDamp = FInt.ZeroF; //No reason for collision offsets since objects aren't clumped
		DPhysicsManager.Drag = FInt.OneF; //Velocity is completely conserved

		//The defualt spacing
		FInt Spacing = FInt.Create (3.5d);

		//Instantiating our walls first
		//Please take note of how an object is instantiated and initialized
		Body WallLeft = Instantiate (Wall).GetComponent<Body> (); 
		WallLeft.Initialize (new Vector2d (Spacing * (-columns / 2 - 3), FInt.Create (0)));
		Body WallRight = Instantiate (Wall).GetComponent<Body> ();
		WallRight.Initialize (new Vector2d (Spacing * (columns / 2 + 2), FInt.Create (0)));

		//Instantiating Newton's Balls!
		for (int j = 0; j < rows; j++) {
			Vector2d SpawnPosition = new Vector2d ((Spacing * (-columns / 2)), Spacing * (j - rows / 2));
			Vector2d StartVelocity = new Vector2d (FInt.Create (1d * speed),FInt.Create (0));
			Body ball;
			for (int i = 0; i < columns; i++) {
				ball = Instantiate (Ball).GetComponent<Body> ();
				ball.Initialize (SpawnPosition); //Always remember to initialize
				SpawnPosition.x += Spacing; //Increment the spawn position for the next spawned ball
				if (i == j % columns) //This adds the 'diagnal effect'
					ball.ApplyVelocity (ref StartVelocity);
			}
		}
	}

	void FixedUpdate ()
	{
		//Remember to simulate DPhysics with this call
		DPhysicsManager.Simulate ();
	}

	void Update ()
	{
		//This call will communicate with Unity's rendering and transform components
		DPhysicsManager.Visualize ();
	}

	int Rows = 1;
	int Columns = 20;
	bool Started = false;
	float camHeight = 42;
	double Speed = 4;
	void OnGUI ()
	{
		//GUI stuff :P
		if (!Started) {
			GUILayout.Label ("Rows: ");
			int tempRows;
			if (int.TryParse (GUILayout.TextField (Rows.ToString ()), out tempRows)) {
				Rows = tempRows;
			}

			GUILayout.Label ("Columns: ");
			int tempColumns;
			if (int.TryParse (GUILayout.TextField (Columns.ToString ()), out tempColumns)) {
				Columns = tempColumns;
			}
			GUILayout.Label ("Speed: ");
			double tempSpeed;
			if (double.TryParse (GUILayout.TextField (Speed.ToString ()), out tempSpeed))
			{
				Speed = tempSpeed;
			}
			if (GUILayout.Button ("Start!")) {
				Started = true;
				Starter (Rows, Columns, Speed);
			}
		}
		else{
			if (GUILayout.Button("Clear"))
			{
				//This dessimilate and destroys all existing physics objects.
				//While dessimilation is optional, it significantly improves performance when there are many objects.
				for (int i = 0; i < DPhysicsManager.SimObjects.Length; i++)
				{
					Body body = DPhysicsManager.SimObjects[i];
					if (body == null) continue;
					DPhysicsManager.Dessimilate(body);
					Destroy(body.gameObject);
				}
				Started = false;
			}
		}
		//Camera stuff
		GUILayout.Label ("Camera Height");
		camHeight = GUILayout.VerticalSlider(camHeight,100,10);
		Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,camHeight,Camera.main.transform.position.z);
		
	}
}
