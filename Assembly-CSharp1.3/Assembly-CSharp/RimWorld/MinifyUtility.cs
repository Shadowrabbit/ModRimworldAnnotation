using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200103C RID: 4156
	public static class MinifyUtility
	{
		// Token: 0x06006235 RID: 25141 RVA: 0x002151B4 File Offset: 0x002133B4
		public static MinifiedThing MakeMinified(this Thing thing)
		{
			if (!thing.def.Minifiable)
			{
				Log.Warning("Tried to minify " + thing + " which is not minifiable.");
				return null;
			}
			if (thing.Spawned)
			{
				thing.DeSpawn(DestroyMode.Vanish);
			}
			if (thing.holdingOwner != null)
			{
				Log.Warning("Can't minify thing which is in a ThingOwner because we don't know how to handle it. Remove it from the container first. holder=" + thing.ParentHolder);
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
				}));
				minifiedThing.InnerThing.stackCount = 1;
			}
			return minifiedThing;
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x002152A1 File Offset: 0x002134A1
		public static Thing TryMakeMinified(this Thing thing)
		{
			if (thing.def.Minifiable)
			{
				return thing.MakeMinified();
			}
			return thing;
		}

		// Token: 0x06006237 RID: 25143 RVA: 0x002152B8 File Offset: 0x002134B8
		public static Thing GetInnerIfMinified(this Thing outerThing)
		{
			MinifiedThing minifiedThing = outerThing as MinifiedThing;
			if (minifiedThing != null)
			{
				return minifiedThing.InnerThing;
			}
			return outerThing;
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x002152D8 File Offset: 0x002134D8
		public static MinifiedThing Uninstall(this Thing th)
		{
			if (!th.Spawned)
			{
				Log.Warning("Can't uninstall unspawned thing " + th);
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
