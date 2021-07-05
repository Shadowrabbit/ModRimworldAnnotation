using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001758 RID: 5976
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x060083CF RID: 33743 RVA: 0x00058694 File Offset: 0x00056894
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks && x != ThingDefOf.RaisedRocks && !x.IsSmoothed
			select x);
		}

		// Token: 0x060083D0 RID: 33744 RVA: 0x00270D1C File Offset: 0x0026EF1C
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

		// Token: 0x060083D1 RID: 33745 RVA: 0x00270D88 File Offset: 0x0026EF88
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

		// Token: 0x060083D2 RID: 33746 RVA: 0x000586D3 File Offset: 0x000568D3
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Token: 0x0400556D RID: 21869
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x0400556E RID: 21870
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x0400556F RID: 21871
		private const float PreciousMineableMarketValue = 5f;
	}
}
