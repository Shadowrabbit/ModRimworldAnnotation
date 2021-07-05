using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E5 RID: 4325
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x06006773 RID: 26483 RVA: 0x0022F793 File Offset: 0x0022D993
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks && x != ThingDefOf.RaisedRocks && !x.IsSmoothed
			select x);
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x0022F7D4 File Offset: 0x0022D9D4
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int randomInRange = (parms.countRange ?? ThingSetMaker_Meteorite.MineablesCountRange).RandomInRange;
			ThingDef def = this.FindRandomMineableDef();
			for (int i = 0; i < randomInRange; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(def, null);
				building.canChangeTerrainOnDestroyed = false;
				outThings.Add(building);
			}
		}

		// Token: 0x06006775 RID: 26485 RVA: 0x0022F840 File Offset: 0x0022DA40
		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			if (value < 0.4f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where !x.building.isResourceRock
				select x).RandomElement<ThingDef>();
			}
			if (value < 0.75f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f
				select x).RandomElement<ThingDef>();
			}
			return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
			where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f
			select x).RandomElement<ThingDef>();
		}

		// Token: 0x06006776 RID: 26486 RVA: 0x0022F8EF File Offset: 0x0022DAEF
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Token: 0x04003A58 RID: 14936
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x04003A59 RID: 14937
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x04003A5A RID: 14938
		private const float PreciousMineableMarketValue = 5f;
	}
}
