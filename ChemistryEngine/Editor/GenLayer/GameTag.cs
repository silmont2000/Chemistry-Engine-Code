namespace Games
{
	public partial class GameTag
	{
		#region Unity Default Lock
		public const string Untagged        = "Untagged";
		public const string Respawn         = "Respawn";
		public const string Finish          = "Finish";
		public const string EditorOnly      = "EditorOnly";
		public const string MainCamera      = "MainCamera";
		public const string Player          = "Player";
		public const string GameController  = "GameController";
		#endregion


		public const string UICamera  = "UICamera";
		public const string War_Terrain  = "War_Terrain";
		public const string War_Unit  = "War_Unit";


		public static int           customBeginIndex = 0;
		public static string[]      customTags = {"UICamera","War_Terrain","War_Unit"};
	}
}
