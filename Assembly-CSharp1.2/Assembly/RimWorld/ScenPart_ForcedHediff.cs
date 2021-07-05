using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D9 RID: 5593
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		// Token: 0x170012C7 RID: 4807
		// (get) Token: 0x06007995 RID: 31125 RVA: 0x00051D56 File Offset: 0x0004FF56
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

		// Token: 0x06007996 RID: 31126 RVA: 0x0024D2BC File Offset: 0x0024B4BC
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

		// Token: 0x06007997 RID: 31127 RVA: 0x00051D81 File Offset: 0x0004FF81
		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		// Token: 0x06007998 RID: 31128 RVA: 0x0024D3A4 File Offset: 0x0024B5A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		// Token: 0x06007999 RID: 31129 RVA: 0x0024D3E4 File Offset: 0x0024B5E4
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.hediff.label).CapitalizeFirst();
		}

		// Token: 0x0600799A RID: 31130 RVA: 0x0024D438 File Offset: 0x0024B638
		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x0600799B RID: 31131 RVA: 0x0024D4AC File Offset: 0x0024B6AC
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

		// Token: 0x0600799C RID: 31132 RVA: 0x0024D4EC File Offset: 0x0024B6EC
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

		// Token: 0x0600799D RID: 31133 RVA: 0x00051DAC File Offset: 0x0004FFAC
		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600799E RID: 31134 RVA: 0x00051DAC File Offset: 0x0004FFAC
		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600799F RID: 31135 RVA: 0x0024D564 File Offset: 0x0024B764
		private void AddHediff(Pawn p)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x04004FED RID: 20461
		private HediffDef hediff;

		// Token: 0x04004FEE RID: 20462
		private FloatRange severityRange;
	}
}
