using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFC RID: 4092
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x0600605A RID: 24666 RVA: 0x0020D393 File Offset: 0x0020B593
		private float MaxSeverity
		{
			get
			{
				if (this.hediff.lethalSeverity <= 0f)
				{
					return 1f;
				}
				return this.hediff.lethalSeverity * 0.99f;
			}
		}

		// Token: 0x0600605B RID: 24667 RVA: 0x0020D3C0 File Offset: 0x0020B5C0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.hediff.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<HediffDef>(this.PossibleHediffs(), (HediffDef hd) => hd.LabelCap, (HediffDef hd) => delegate()
				{
					this.hediff = hd;
					if (this.severityRange.max > this.MaxSeverity)
					{
						this.severityRange.max = this.MaxSeverity;
					}
					if (this.severityRange.min > this.MaxSeverity)
					{
						this.severityRange.min = this.MaxSeverity;
					}
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.severityRange, 0f, this.MaxSeverity, "ConfigurableSeverity", ToStringStyle.FloatTwo);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x0600605C RID: 24668 RVA: 0x0020D4A7 File Offset: 0x0020B6A7
		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0020D4D4 File Offset: 0x0020B6D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x0020D514 File Offset: 0x0020B714
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.hediff.label).CapitalizeFirst();
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x0020D568 File Offset: 0x0020B768
		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x0020D5DC File Offset: 0x0020B7DC
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ForcedHediff scenPart_ForcedHediff = other as ScenPart_ForcedHediff;
			if (scenPart_ForcedHediff != null && this.hediff == scenPart_ForcedHediff.hediff)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_ForcedHediff.chance);
				return true;
			}
			return false;
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x0020D61C File Offset: 0x0020B81C
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			if (!base.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
			{
				return false;
			}
			if (this.hideOffMap)
			{
				if (!req.AllowDead && pawn.health.WouldDieAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
				if (!req.AllowDowned && pawn.health.WouldBeDownedAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x0020D693 File Offset: 0x0020B893
		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x0020D693 File Offset: 0x0020B893
		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x06006064 RID: 24676 RVA: 0x0020D69C File Offset: 0x0020B89C
		private void AddHediff(Pawn p)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x0400372C RID: 14124
		private HediffDef hediff;

		// Token: 0x0400372D RID: 14125
		private FloatRange severityRange;
	}
}
