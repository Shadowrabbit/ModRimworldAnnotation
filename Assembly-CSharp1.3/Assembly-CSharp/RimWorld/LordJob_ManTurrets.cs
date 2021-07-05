using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200087D RID: 2173
	public class LordJob_ManTurrets : LordJob
	{
		// Token: 0x06003965 RID: 14693 RVA: 0x0014144E File Offset: 0x0013F64E
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
