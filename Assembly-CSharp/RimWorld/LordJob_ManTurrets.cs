using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DC5 RID: 3525
	public class LordJob_ManTurrets : LordJob
	{
		// Token: 0x06005062 RID: 20578 RVA: 0x000386E5 File Offset: 0x000368E5
		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
