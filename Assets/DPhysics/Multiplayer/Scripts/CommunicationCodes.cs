/// <summary>
/// Use these elements for interpreting and sending tags..
/// </summary>
public enum NetworkTag : byte {
	/// <summary>
	/// Used for logging in, matchmaking, etc.. Note: Nothing implemented for Meta.
	/// </summary>
	Meta = 0,
	/// <summary>
	/// Used for in-game communication; defines data that should be distributed to everyone in the player's room.
	/// </summary>
	Game = 1,
}
