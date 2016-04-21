using UnityEngine;
using System.Collections;

/// <summary>
/// Frame holds the data to simulate for a frame. Simulation cannot advance without a frame for the simulation's current frame.
/// </summary>
public struct Frame {
	public byte[] _Data;
	public Frame (byte[] data){
		_Data = data;
	}
}
