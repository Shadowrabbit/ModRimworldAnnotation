using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001003 RID: 4099
	public class ScenPart_SetNeedLevel : ScenPart_PawnModifier
	{
		// Token: 0x06006089 RID: 24713 RVA: 0x0020E1A0 File Offset: 0x0020C3A0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.need.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<NeedDef>(this.PossibleNeeds(), (NeedDef hd) => hd.LabelCap, (NeedDef n) => delegate()
				{
					this.need = n;
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.levelRange, 0f, 1f, "ConfigurableLevel", ToStringStyle.FloatTwo);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x0600608A RID: 24714 RVA: 0x0020E286 File Offset: 0x0020C486
		private IEnumerable<NeedDef> PossibleNeeds()
		{
			return from x in DefDatabase<NeedDef>.AllDefsListForReading
			where x.major
			select x;
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x0020E2B4 File Offset: 0x0020C4B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<FloatRange>(ref this.levelRange, "levelRange", default(FloatRange), false);
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x0020E2F4 File Offset: 0x0020C4F4
		public override string Summary(Scenario scen)
		{
			return "ScenPart_SetNeed".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.need.label, this.levelRange.min.ToStringPercent(), this.levelRange.max.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x0020E374 File Offset: 0x0020C574
		public override void Randomize()
		{
			base.Randomize();
			this.need = this.PossibleNeeds().RandomElement<NeedDef>();
			this.levelRange.max = Rand.Range(0f, 1f);
			this.levelRange.min = this.levelRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x0020E3D8 File Offset: 0x0020C5D8
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_SetNeedLevel scenPart_SetNeedLevel = other as ScenPart_SetNeedLevel;
			if (scenPart_SetNeedLevel != null && this.need == scenPart_SetNeedLevel.need)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_SetNeedLevel.chance);
				return true;
			}
			return false;
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x0020E418 File Offset: 0x0020C618
		protected override void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
			if (p.needs != null)
			{
				Need need = p.needs.TryGetNeed(this.need);
				if (need != null)
				{
					need.CurLevelPercentage = this.levelRange.RandomInRange;
				}
			}
		}

		// Token: 0x0400373B RID: 14139
		private NeedDef need;

		// Token: 0x0400373C RID: 14140
		private FloatRange levelRange;
	}
}
