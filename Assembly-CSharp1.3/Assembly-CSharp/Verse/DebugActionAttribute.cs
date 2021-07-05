using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020003C0 RID: 960
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugActionAttribute : Attribute
	{
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x000B8CB0 File Offset: 0x000B6EB0
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

		// Token: 0x06001D92 RID: 7570 RVA: 0x000B8D5A File Offset: 0x000B6F5A
		public DebugActionAttribute(string category = null, string name = null)
		{
			this.name = name;
			if (!string.IsNullOrEmpty(category))
			{
				this.category = category;
			}
		}

		// Token: 0x040011C7 RID: 4551
		public string name;

		// Token: 0x040011C8 RID: 4552
		public string category = "General";

		// Token: 0x040011C9 RID: 4553
		public AllowedGameStates allowedGameStates = AllowedGameStates.Playing;

		// Token: 0x040011CA RID: 4554
		public DebugActionType actionType;
	}
}
