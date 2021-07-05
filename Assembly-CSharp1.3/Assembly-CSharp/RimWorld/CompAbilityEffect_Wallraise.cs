using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D7A RID: 3450
	public class CompAbilityEffect_Wallraise : CompAbilityEffect
	{
		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06004FEE RID: 20462 RVA: 0x001ABBFC File Offset: 0x001A9DFC
		public new CompProperties_AbilityWallraise Props
		{
			get
			{
				return (CompProperties_AbilityWallraise)this.props;
			}
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001ABC0C File Offset: 0x001A9E0C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			List<Thing> list = new List<Thing>();
			list.AddRange(this.AffectedCells(target, map).SelectMany((IntVec3 c) => from t in c.GetThingList(map)
			where t.def.category == ThingCategory.Item
			select t));
			foreach (Thing thing in list)
			{
				thing.DeSpawn(DestroyMode.Vanish);
			}
			foreach (IntVec3 loc in this.AffectedCells(target, map))
			{
				GenSpawn.Spawn(ThingDefOf.RaisedRocks, loc, map, WipeMode.Vanish);
				FleckMaker.ThrowDustPuffThick(loc.ToVector3Shifted(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Wallraise.DustColor);
			}
			foreach (Thing thing2 in list)
			{
				IntVec3 intVec = IntVec3.Invalid;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 intVec2 = thing2.Position + GenRadial.RadialPattern[i];
					if (intVec2.InBounds(map) && intVec2.Walkable(map) && map.thingGrid.ThingsListAtFast(intVec2).Count <= 0)
					{
						intVec = intVec2;
						break;
					}
				}
				if (intVec != IntVec3.Invalid)
				{
					GenSpawn.Spawn(thing2, intVec, map, WipeMode.Vanish);
				}
				else
				{
					GenPlace.TryPlaceThing(thing2, thing2.Position, map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x001ABE10 File Offset: 0x001AA010
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, true);
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001ABE1C File Offset: 0x001AA01C
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red, null);
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001ABE69 File Offset: 0x001AA069
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			foreach (IntVec2 intVec in this.Props.pattern)
			{
				IntVec3 intVec2 = target.Cell + new IntVec3(intVec.x, 0, intVec.z);
				if (intVec2.InBounds(map))
				{
					yield return intVec2;
				}
			}
			List<IntVec2>.Enumerator enumerator = default(List<IntVec2>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x001ABE88 File Offset: 0x001AA088
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => c.Filled(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.AffectedCells(target, this.parent.pawn.Map).Any((IntVec3 c) => !c.Standable(this.parent.pawn.Map)))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityUnwalkable".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04002FD1 RID: 12241
		public static Color DustColor = new Color(0.55f, 0.55f, 0.55f, 4f);
	}
}
