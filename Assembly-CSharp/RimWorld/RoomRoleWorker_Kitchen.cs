using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001306 RID: 4870
	public class RoomRoleWorker_Kitchen : RoomRoleWorker
	{
		// Token: 0x060069A1 RID: 27041 RVA: 0x00208748 File Offset: 0x00206948
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.designationCategory == DesignationCategoryDefOf.Production)
				{
					for (int j = 0; j < thing.def.AllRecipes.Count; j++)
					{
						RecipeDef recipeDef = thing.def.AllRecipes[j];
						for (int k = 0; k < recipeDef.products.Count; k++)
						{
							ThingDef thingDef = recipeDef.products[k].thingDef;
							if (thingDef.IsNutritionGivingIngestible && thingDef.ingestible.HumanEdible)
							{
								num++;
								goto IL_AD;
							}
						}
					}
				}
				IL_AD:;
			}
			return (float)num * 14f;
		}
	}
}
