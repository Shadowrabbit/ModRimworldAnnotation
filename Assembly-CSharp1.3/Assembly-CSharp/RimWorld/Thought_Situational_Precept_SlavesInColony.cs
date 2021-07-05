using System;

namespace RimWorld
{
	// Token: 0x02000958 RID: 2392
	public class Thought_Situational_Precept_SlavesInColony : Thought_Situational
	{
		// Token: 0x06003D0E RID: 15630 RVA: 0x00151074 File Offset: 0x0014F274
		public override float MoodOffset()
		{
			return this.BaseMoodOffset * (float)FactionUtility.GetSlavesInFactionCount(this.pawn.Faction);
		}
	}
}
