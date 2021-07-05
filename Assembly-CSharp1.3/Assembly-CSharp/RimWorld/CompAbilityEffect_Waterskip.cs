using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D7C RID: 3452
	public class CompAbilityEffect_Waterskip : CompAbilityEffect
	{
		// Token: 0x06004FF9 RID: 20473 RVA: 0x001ABFF8 File Offset: 0x001AA1F8
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
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(c.ToVector3Shifted(), map, FleckDefOf.WaterskipSplashParticles, 1f);
				dataStatic.rotationRate = (float)Rand.Range(-30, 30);
				dataStatic.rotation = (float)(90 * Rand.RangeInclusive(0, 3));
				map.flecks.CreateFleck(dataStatic);
			}
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x001AC0FC File Offset: 0x001AA2FC
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

		// Token: 0x06004FFB RID: 20475 RVA: 0x001AC11C File Offset: 0x001AA31C
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red, null);
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x001AC16C File Offset: 0x001AA36C
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
