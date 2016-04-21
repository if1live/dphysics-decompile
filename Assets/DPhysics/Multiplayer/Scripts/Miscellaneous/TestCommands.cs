using UnityEngine;
using System.Collections;
using DPhysics;
public class TestCommands : MonoBehaviour {


	// Update is called once per frame
	void OnGUI () {
		//Don't want the user doing anything when the game hasn't started yet
		if (!DarkRift.DarkRiftAPI.isConnected || Client.StepCount == 0) return;

		if (GUILayout.Button ("Move Up"))
		{
			Command com = new Command(GameOp.MoveUp);
			Client.SendCommand (com);
		}
		if (GUILayout.Button ("Move Down"))
		{
			Command com = new Command(GameOp.MoveDown);
			Client.SendCommand (com);
		}
		if (GUILayout.Button ("Move Left"))
		{
			Command com = new Command(GameOp.MoveLeft);
			Client.SendCommand (com);
		}
		if (GUILayout.Button ("Move Right"))
		{
			Command com = new Command(GameOp.MoveRight);
			Client.SendCommand (com);
		}


		if (GUILayout.Button ("Move To"))
		{
			Command com = new Command(GameOp.MoveTo);
			com.SerialPosition = new Vector2d (
				FInt.Create (xPos),
				FInt.Create (yPos)
				);
			Client.SendCommand (com);
		}
		//The position to move to
		double.TryParse(GUILayout.TextField (xPos.ToString()), out xPos);
		double.TryParse(GUILayout.TextField (yPos.ToString ()), out yPos);
	}
	double xPos;
	double yPos;
	public static void Execute (Command com)
	{
		//Do something based on the Command's designated operation
		const int speed = 5;
		switch (com.Operation)
		{
		case GameOp.MoveUp:
			Vector2d UpForce = new Vector2d(0,speed);
			foreach (Body body in DPhysicsManager.SimObjects)
			{
				if (body != null)
				body.ApplyVelocity (ref UpForce);
			}
			break;
		case GameOp.MoveDown:
			Vector2d DownForce = new Vector2d (0,-speed);
			foreach (Body body in DPhysicsManager.SimObjects)
			{
				if (body != null)
					body.ApplyVelocity(ref DownForce);
			}
			break;
		case GameOp.MoveLeft:
			Vector2d LeftForce = new Vector2d(-speed,0);
			foreach (Body body in DPhysicsManager.SimObjects)
			{
				if (body != null)
				body.ApplyVelocity(ref LeftForce);
			}
			break;
		case GameOp.MoveRight:
			Vector2d RightForce = new Vector2d (speed,0);
			foreach (Body body in DPhysicsManager.SimObjects)
			{
				if (body != null)
					body.ApplyVelocity(ref RightForce);
			}
			break;
		case GameOp.MoveTo:
			if (!com.HasPosition) break; //If there is no position contained in the command, don't do anything
			Vector2d Target = com.SerialPosition;
			foreach (Body body in DPhysicsManager.SimObjects)
			{
				if (body != null)
				{
					Vector2d direction = Target - body.Position;
					direction.Normalize();
					direction *= speed;
					body.ApplyVelocity (ref direction);
				}
			}
			break;
		}
	}
}
/// <summary>
/// GameOps are used for similar purposes of RPCs. They can be interpreted by clients to generate certain actions.
/// These are serialized and sent in the Command container.
/// </summary>
public enum GameOp : byte {
	MoveUp,
	MoveDown,
	MoveLeft,
	MoveRight,
	MoveTo
}