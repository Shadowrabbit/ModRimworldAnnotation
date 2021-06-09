using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020006C2 RID: 1730
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugActionAttribute : Attribute
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002C9E RID: 11422 RVA: 0x0012FCA4 File Offset: 0x0012DEA4
		public bool IsAllowedInCurrentGameState
		{
			get
			{
				bool flag = (this.allowedGameStates & AllowedGameStates.Entry) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Entry;
				bool flag2 = (this.allowedGameStates & AllowedGameStates.Playing) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Playing;
				bool flag3 = (this.allowedGameStates & AllowedGameStates.WorldRenderedNow) == AllowedGameStates.Invalid || WorldRendererUtility.WorldRenderedNow;
				bool flag4 = (this.allowedGameStates & AllowedGameStates.IsCurrentlyOnMap) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null);
				bool flag5 = (this.allowedGameStates & AllowedGameStates.HasGameCondition) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldRenderedNow && Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ActiveConditions.Count > 0);
				return flag && flag2 && flag3 && flag4 && flag5;
			}
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x00023678 File Offset: 0x00021878
		public DebugActionAttribute(string category = null, string name = null)
		{
			this.name = name;
			if (!string.IsNullOrEmpty(category))
			{
				this.category = category;
			}
		}

		// Token: 0x04001E3C RID: 7740
		public string name;

		// Token: 0x04001E3D RID: 7741
		public string category = "General";

		// Token: 0x04001E3E RID: 7742
		public AllowedGameStates allowedGameStates = AllowedGameStates.Playing;

		// Token: 0x04001E3F RID: 7743
		public DebugActionType actionType;
	}
}
