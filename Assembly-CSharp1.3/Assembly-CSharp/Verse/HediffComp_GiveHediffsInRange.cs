using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029D RID: 669
	public class HediffComp_GiveHediffsInRange : HediffComp
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x0006A7A1 File Offset: 0x000689A1
		public HediffCompProperties_GiveHediffsInRange Props
		{
			get
			{
				return (HediffCompProperties_GiveHediffsInRange)this.props;
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0006A7B0 File Offset: 0x000689B0
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.parent.pawn.Awake() || this.parent.pawn.health == null || this.parent.pawn.health.InPainShock)
			{
				return;
			}
			if (!this.Props.hideMoteWhenNotDrafted || this.parent.pawn.Drafted)
			{
				if (this.Props.mote != null && (this.mote == null || this.mote.Destroyed))
				{
					this.mote = MoteMaker.MakeAttachedOverlay(this.parent.pawn, this.Props.mote, Vector3.zero, 1f, -1f);
				}
				if (this.mote != null)
				{
					this.mote.Maintain();
				}
			}
			List<Pawn> list;
			if (this.Props.onlyPawnsInSameFaction && this.parent.pawn.Faction != null)
			{
				list = this.parent.pawn.Map.mapPawns.PawnsInFaction(this.parent.pawn.Faction);
			}
			else
			{
				list = this.parent.pawn.Map.mapPawns.AllPawns;
			}
			foreach (Pawn pawn in list)
			{
				if (pawn.RaceProps.Humanlike && !pawn.Dead && pawn.health != null && pawn != this.parent.pawn && pawn.Position.DistanceTo(this.parent.pawn.Position) <= this.Props.range && this.Props.targetingParameters.CanTarget(pawn, null))
				{
					Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
					if (hediff == null)
					{
						hediff = pawn.health.AddHediff(this.Props.hediff, pawn.health.hediffSet.GetBrain(), null, null);
						hediff.Severity = this.Props.initialSeverity;
						HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
						if (hediffComp_Link != null)
						{
							hediffComp_Link.drawConnection = true;
							hediffComp_Link.other = this.parent.pawn;
						}
					}
					HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears == null)
					{
						Log.Error("HediffComp_GiveHediffsInRange has a hediff in props which does not have a HediffComp_Disappears");
					}
					else
					{
						hediffComp_Disappears.ticksToDisappear = 5;
					}
				}
			}
		}

		// Token: 0x04000E03 RID: 3587
		private Mote mote;
	}
}
