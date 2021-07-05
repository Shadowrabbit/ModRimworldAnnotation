using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003BA RID: 954
	public static class DebugThingPlaceHelper
	{
		// Token: 0x06001D83 RID: 7555 RVA: 0x000B81E0 File Offset: 0x000B63E0
		public static bool IsDebugSpawnable(ThingDef def, bool allowPlayerBuildable = false)
		{
			return def.forceDebugSpawnable || (!(def.thingClass == typeof(Corpse)) && !def.IsBlueprint && !def.IsFrame && def != ThingDefOf.ActiveDropPod && !(def.thingClass == typeof(MinifiedThing)) && !(def.thingClass == typeof(UnfinishedThing)) && !def.thingClass.IsSubclassOf(typeof(SignalAction)) && !def.destroyOnDrop && (def.category == ThingCategory.Filth || def.category == ThingCategory.Item || def.category == ThingCategory.Plant || def.category == ThingCategory.Ethereal || (def.category == ThingCategory.Building && def.building.isNaturalRock) || (def.category == ThingCategory.Building && !def.BuildableByPlayer) || (def.category == ThingCategory.Building && def.BuildableByPlayer && allowPlayerBuildable)));
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000B82E0 File Offset: 0x000B64E0
		public static void DebugSpawn(ThingDef def, IntVec3 c, int stackCount = -1, bool direct = false, ThingStyleDef thingStyleDef = null)
		{
			if (stackCount <= 0)
			{
				stackCount = def.stackLimit;
			}
			ThingDef stuff = GenStuff.RandomStuffFor(def);
			Thing thing = ThingMaker.MakeThing(def, stuff);
			if (thingStyleDef != null)
			{
				thing.StyleDef = thingStyleDef;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), ArtGenerationContext.Colony);
			}
			if (thing.def.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			thing.stackCount = stackCount;
			if (direct)
			{
				GenPlace.TryPlaceThing(thing, c, Find.CurrentMap, ThingPlaceMode.Direct, null, null, default(Rot4));
				return;
			}
			GenPlace.TryPlaceThing(thing, c, Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000B837C File Offset: 0x000B657C
		public static List<DebugMenuOption> TryPlaceOptionsForStackCount(int stackCount, bool direct)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef3 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.stackLimit >= stackCount
			select def)
			{
				ThingDef localDef = localDef3;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null);
				}));
			}
			if (stackCount == 1)
			{
				foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.Minifiable
				select def)
				{
					ThingDef localDef = localDef2;
					list.Add(new DebugMenuOption(localDef.LabelCap + " (minified)", DebugMenuOptionMode.Tool, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null);
					}));
				}
			}
			return list;
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000B84E0 File Offset: 0x000B66E0
		public static List<DebugMenuOption> TryPlaceOptionsForBaseMarketValue(float marketValue, bool direct)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.stackLimit > 1
			select def)
			{
				ThingDef localDef = localDef2;
				int stackCount = (int)(marketValue / localDef.BaseMarketValue);
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null);
				}));
			}
			return list;
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000B85B4 File Offset: 0x000B67B4
		public static List<DebugMenuOption> SpawnOptions(WipeMode wipeMode)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, true)
			select def)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					Thing thing = ThingMaker.MakeThing(localDef, GenStuff.RandomStuffFor(localDef));
					CompQuality compQuality = thing.TryGetComp<CompQuality>();
					if (compQuality != null)
					{
						compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), ArtGenerationContext.Colony);
					}
					GenSpawn.Spawn(thing, UI.MouseCell(), Find.CurrentMap, wipeMode);
				}));
			}
			return list;
		}
	}
}
