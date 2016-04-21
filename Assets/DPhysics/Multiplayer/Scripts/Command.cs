using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
public class Command {
	#region Constructor
	public Command (GameOp operation)
	{
		Operation = operation;
	}
	#endregion

	#region Encapsulated Data: The data which will be serialized and used to reconstruct a Command.
	/// <summary>
	/// The operation this command causes.
	/// </summary>
	public GameOp Operation;

	/// <summary>
	/// Gets or seta the a serialized position sent over the network.
	/// </summary>
	public Vector2d SerialPosition{
		get{
			return _position;
		}
		set{
			HasPosition = true;
			_position = value;
		}
	}
	private Vector2d _position;
	public bool HasPosition;
	#endregion

	#region Serialization/Deserialization
	//DataType is used to define which data type to serialize or deserialize
	private enum DataType : byte{
		Position

	}
	/// <summary>
	/// Returns a list of bytes that can be used to reconstruct the command.
	/// </summary>
	public List<byte> Serialized ()
	{
		//The serialization process
		using (MemoryStream m = new MemoryStream())
		{
			using (BinaryWriter writer = new BinaryWriter(m))
			{
				//First, serialize our operation
				writer.Write ((byte)Operation);
				//If we have a position, serialize it.
				if (HasPosition)
				{
					//First, serialize the position's data type to tell us what to construct later
					writer.Write ((byte)DataType.Position);
					//Then serialize the position's values
					writer.Write (_position.x.RawValue);
					writer.Write (_position.y.RawValue);
				}

				//Tie our command off with a neat little bow in the beginning
				//That tells how many bytes the command has.
				//This will allow you to send and receive multiple commands in the same frame.
			}

			List<byte> ret = new List<byte>( m.ToArray());
			ret.Insert (0,(byte)(ret.Count + 1));
			return ret;
		}
	}

	/// <summary>
	/// Deserializes a byte array into a list of Commands.
	/// </summary>
	/// <param name="Source">Source.</param>
	public static  List<Command> Deserialize (byte[] Source)
	{
		List<Command> DeserializedCommands = new List<Command>();

		//The deserialization process
		using (MemoryStream m = new MemoryStream(Source))
		{
			using (BinaryReader reader = new BinaryReader (m))
			{
				//As long as there are still bytes to read, there are still Commands to deserialize
				while (reader.BaseStream.Position < reader.BaseStream.Length)
				{

					//First, let's get the amount of bytes in this Command's package, a byte that we serializd.
					byte DataLength = reader.ReadByte ();
					//Then create an int that represents how many bytes of the Command's package we've read.
					int curRead = 1;

					//Create the Command from the deserialized operation
					GameOp operation = (GameOp) reader.ReadByte();
					Command com = new Command(operation);
					//Don't forget to increase curRead
					curRead++;

					//As long as we haven't read more than the amount in the Command's package, there are more variables.
					while (curRead < DataLength)
					{
						//Get the type of data to deserialize
						DataType type = (DataType)reader.ReadByte();
						curRead++;

						//Plug it into the magical switch of deserialization; deserialization is hard-coded here.
						switch (type)
						{
						case DataType.Position:
							com.SerialPosition = new Vector2d(
								FInt.Create ((long)reader.ReadUInt64()),
								FInt.Create ((long)reader.ReadUInt64())
								);
							//Make sure to increase curRead by the amount of bytes read 
							curRead += 16;
							break;
						}
					}
					//Now that we've deserialized the Command, let's add it into the list
					DeserializedCommands.Add (com);
				}
			}
		}

		//Now let's return the list of commands and be on our way with the simulation!
		return DeserializedCommands;
	}
	#endregion
}
