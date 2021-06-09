using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000834 RID: 2100
	public static class GenDrop
	{
		// Token: 0x060034BA RID: 13498 RVA: 0x0002930E File Offset: 0x0002750E
		[Obsolete("Only used for mod compatibility")]
		public static bool TryDropSpawn(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			return GenDrop.TryDropSpawn_NewTmp(thing, dropCell, map, mode, out resultingThing, placedAction, nearPlaceValidator, true);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x001544C8 File Offset: 0x001526C8
		public static bool TryDropSpawn_NewTmp(Thing thing, IntVec3 dropCell, Map map, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (map == null)
			{
				Log.Error("Dropped " + thing + " in a null map.", false);
				resultingThing = null;
				return false;
			}
			if (!dropCell.InBounds(map))
			{
				Log.Error(string.Concat(new object[]
				{
					"Dropped ",
					thing,
					" out of bounds at ",
					dropCell
				}), false);
				resultingThing = null;
				return false;
			}
			if (thing.def.destroyOnDrop)
			{
				thing.Destroy(DestroyMode.Vanish);
				resultingThing = null;
				return true;
			}
			if (playDropSound && thing.def.soundDrop != null)
			{
				thing.def.soundDrop.PlayOneShot(new TargetInfo(dropCell, map, false));
			}
			return GenPlace.TryPlaceThing(thing, dropCell, map, mode, out resultingThing, placedAction, nearPlaceValidator, default(Rot4));
		}
	}
}
