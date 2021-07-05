using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200093F RID: 2367
	public class ThoughtWorker_Precept_HasAutomatedTurrets : ThoughtWorker_Precept
	{
		// Token: 0x06003CD1 RID: 15569 RVA: 0x00150528 File Offset: 0x0014E728
		public static void ResetStaticData()
		{
			ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.IsTurret && !allDefsListForReading[i].HasComp(typeof(CompMannable)))
				{
					ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Add(allDefsListForReading[i]);
				}
			}
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x001505A0 File Offset: 0x0014E7A0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction == null || p.IsSlave)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				for (int j = 0; j < ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Count; j++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs[j]);
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].Faction == p.Faction)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040020BE RID: 8382
		private static List<ThingDef> automatedTurretDefs = new List<ThingDef>();
	}
}
