using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001662 RID: 5730
	public static class MinifyUtility
	{
		// Token: 0x06007CD8 RID: 31960 RVA: 0x00255164 File Offset: 0x00253364
		public static MinifiedThing MakeMinified(this Thing thing)
		{
			if (!thing.def.Minifiable)
			{
				Log.Warning("Tried to minify " + thing + " which is not minifiable.", false);
				return null;
			}
			if (thing.Spawned)
			{
				thing.DeSpawn(DestroyMode.Vanish);
			}
			if (thing.holdingOwner != null)
			{
				Log.Warning("Can't minify thing which is in a ThingOwner because we don't know how to handle it. Remove it from the container first. holder=" + thing.ParentHolder, false);
				return null;
			}
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(thing);
			MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(thing.def.minifiedDef, null);
			minifiedThing.InnerThing = thing;
			if (blueprint_Install != null)
			{
				blueprint_Install.SetThingToInstallFromMinified(minifiedThing);
			}
			if (minifiedThing.InnerThing.stackCount > 1)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to minify ",
					thing.LabelCap,
					" with stack count ",
					minifiedThing.InnerThing.stackCount,
					". Clamped stack count to 1."
				}), false);
				minifiedThing.InnerThing.stackCount = 1;
			}
			return minifiedThing;
		}

		// Token: 0x06007CD9 RID: 31961 RVA: 0x00053E2A File Offset: 0x0005202A
		public static Thing TryMakeMinified(this Thing thing)
		{
			if (thing.def.Minifiable)
			{
				return thing.MakeMinified();
			}
			return thing;
		}

		// Token: 0x06007CDA RID: 31962 RVA: 0x00255254 File Offset: 0x00253454
		public static Thing GetInnerIfMinified(this Thing outerThing)
		{
			MinifiedThing minifiedThing = outerThing as MinifiedThing;
			if (minifiedThing != null)
			{
				return minifiedThing.InnerThing;
			}
			return outerThing;
		}

		// Token: 0x06007CDB RID: 31963 RVA: 0x00255274 File Offset: 0x00253474
		public static MinifiedThing Uninstall(this Thing th)
		{
			if (!th.Spawned)
			{
				Log.Warning("Can't uninstall unspawned thing " + th, false);
				return null;
			}
			Map map = th.Map;
			MinifiedThing minifiedThing = th.MakeMinified();
			GenPlace.TryPlaceThing(minifiedThing, th.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
			SoundDefOf.ThingUninstalled.PlayOneShot(new TargetInfo(th.Position, map, false));
			return minifiedThing;
		}
	}
}
