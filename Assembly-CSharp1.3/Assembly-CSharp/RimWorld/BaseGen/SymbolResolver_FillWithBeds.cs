using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015DE RID: 5598
	public class SymbolResolver_FillWithBeds : SymbolResolver
	{
		// Token: 0x0600838B RID: 33675 RVA: 0x002EFC64 File Offset: 0x002EDE64
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.singleThingDef != null)
			{
				thingDef = rp.singleThingDef;
			}
			else if (rp.faction != null && rp.faction.def.techLevel >= TechLevel.Medieval)
			{
				thingDef = ThingDefOf.Bed;
			}
			else
			{
				thingDef = Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			}
			ThingDef singleThingStuff;
			if (rp.singleThingStuff != null && rp.singleThingStuff.stuffProps.CanMake(thingDef))
			{
				singleThingStuff = rp.singleThingStuff;
			}
			else
			{
				singleThingStuff = BaseGenUtility.CheapStuffFor(thingDef, rp.faction);
			}
			bool @bool = Rand.Bool;
			int num = rp.bedCount ?? int.MaxValue;
			int num2 = 0;
			foreach (IntVec3 intVec in rp.rect)
			{
				if (@bool)
				{
					if (intVec.x % 3 != 0)
					{
						continue;
					}
					if (intVec.z % 2 != 0)
					{
						continue;
					}
				}
				else if (intVec.x % 2 != 0 || intVec.z % 3 != 0)
				{
					continue;
				}
				Rot4 rot = @bool ? Rot4.West : Rot4.North;
				if (!GenSpawn.WouldWipeAnythingWith(intVec, rot, thingDef, map, (Thing x) => x.def.category == ThingCategory.Building) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(GenAdj.OccupiedRect(intVec, rot, thingDef.Size), map))
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = GenAdj.OccupiedRect(intVec, rot, thingDef.size);
					resolveParams.singleThingDef = thingDef;
					resolveParams.singleThingStuff = singleThingStuff;
					resolveParams.thingRot = new Rot4?(rot);
					BaseGen.symbolStack.Push("bed", resolveParams, null);
					num2++;
					if (num2 >= num)
					{
						break;
					}
				}
			}
		}
	}
}
