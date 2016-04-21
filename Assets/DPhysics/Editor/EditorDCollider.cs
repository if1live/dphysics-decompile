namespace DPhysics{
	using System.Collections;
	using UnityEngine;
	using System;
	using UnityEditor;
	using System.Collections.Generic;
	
	[CustomEditor(typeof(DCollider))]
	public class EditorCollider : Editor
	{
		
		// Use this for initialization
		DCollider dCollider;
		
		void OnEnable ()
		{
			dCollider = (DCollider)target;
		}
		void Update()
		{
			dCollider.transform.position = Vector3.zero;
			
			dCollider.transform.rotation = Quaternion.identity;
			
		}
		void OnSceneGUI ()
		{
			
			if(Application.isPlaying)
				return;
			Update ();
			const float Snap = .5f;
			Vector3 storePos = Vector3.zero;
			Vector3 laststorePos;
			int MaxCount = dCollider.Vertices.Length - 1;
			
			if (dCollider.IsCircle){
				storePos = (Vector3)dCollider.Center;
				storePos.y = (float)(0);
				Vector3 tempPos = new Vector3(storePos.x + (float)dCollider.Radius,storePos.y,storePos.z );
				Color lecolor = Color.yellow;
				if (!Application.isPlaying)
				{
					dCollider.Radius = Handles.FreeMoveHandle (
						tempPos,
						Quaternion.identity,
						.6f,
						new Vector3(Snap,0,Snap),
						Handles.SphereCap).x;
				}
				else{
					lecolor = Color.gray;
					Handles.color = lecolor;
					Handles.SphereCap(7,tempPos,Quaternion.identity,.6f);
				}
				Handles.DrawLine (tempPos,storePos);
				
				
				Handles.color = Color.blue;
				
				Handles.CircleCap (1,storePos,Quaternion.Euler (new Vector3(90,0,0)),(float)dCollider.Radius);
				
				Handles.color = lecolor;
				storePos.x = tempPos.x;
				Handles.DrawLine (storePos,tempPos);
			}
			else if (MaxCount > 0) {
				for (int i = 0; i < MaxCount + 1; i ++) {
					if (i == 0) {
						Vector2 tempvec = dCollider.Vertices [MaxCount];
						laststorePos = new Vector3(tempvec.x, 0, tempvec.y);
					} else {
						laststorePos = storePos;
					}
					Vector2 tempvec2 = dCollider.Vertices [i];
					storePos = new Vector3(tempvec2.x, 0, tempvec2.y);
					if (!Application.isPlaying) {
						Handles.color = Color.yellow;
						storePos = Handles.FreeMoveHandle (storePos, Quaternion.identity, .4f, new Vector3(Snap,0,Snap), Handles.SphereCap);
					} else {
						Handles.color = Color.grey;
						Handles.CircleCap (i, storePos, Quaternion.identity, .1f);
					}
					dCollider.Vertices [i] = ((Vector2d)storePos).ToSinglePrecision();
					Handles.Label (storePos, "p" + i.ToString ());
					Handles.color = Color.blue;
					Handles.DrawLine (storePos, laststorePos);
					Vector3 thisLastStorePos = storePos;
					
					Handles.DrawLine (storePos, laststorePos);
					Handles.DrawLine (thisLastStorePos, storePos);
				}
			}
		}
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			//Serializing and caching fint creation
			if (dCollider.backupPoints.Length != dCollider.Vertices.Length)
				dCollider.backupPoints = new Vector2d[dCollider.Vertices.Length];
			Vector2 vert;
			for (int i = 0; i < dCollider.Vertices.Length; i++)
			{
				vert = dCollider.Vertices[i];
				dCollider.backupPoints[i] = new Vector2d(FInt.Create (vert.x), FInt.Create (vert.y));
			}
			dCollider.radius = FInt.Create (dCollider.Radius);
		}
	}
}