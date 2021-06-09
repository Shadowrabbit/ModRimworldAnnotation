using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A0 RID: 5024
	public class CompAbilityEffect_Waterskip : CompAbilityEffect
	{
		// Token: 0x06006CF3 RID: 27891 RVA: 0x00216CF0 File Offset: 0x00214EF0
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			foreach (IntVec3 c in this.AffectedCells(target, map))
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					if (thingList[i] is Fire)
					{
						thingList[i].Destroy(DestroyMode.Vanish);
					}
				}
				if (!c.Filled(map))
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Water, 1, FilthSourceFlags.None);
				}
				Mote mote = MoteMaker.MakeStaticMote(c.ToVector3Shifted(), map, ThingDefOf.Mote_WaterskipSplashParticles, 1f);
				if (mote != null)
				{
					mote.rotationRate = Rand.Range(-30f, 30f);
					mote.exactRotation = (float)(90 * Rand.RangeInclusive(0, 3));
				}
			}
		}

		// Token: 0x06006CF4 RID: 27892 RVA: 0x0004A1DC File Offset: 0x000483DC
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				yield break;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, this.parent.def.EffectRadius, true))
			{
				if (intVec.InBounds(map) && GenSight.LineOfSightToEdges(target.Cell, intVec, map, true, null))
				{
					yield return intVec;
				}
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006CF5 RID: 27893 RVA: 0x0004A1FA File Offset: 0x000483FA
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red);
		}

		// Token: 0x06006CF6 RID: 27894 RVA: 0x00216DF0 File Offset: 0x00214FF0
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
