using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F2 RID: 4594
	public abstract class CompTargetable : CompUseEffect
	{
		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06006E8F RID: 28303 RVA: 0x0025071E File Offset: 0x0024E91E
		public CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06006E90 RID: 28304
		protected abstract bool PlayerChoosesTarget { get; }

		// Token: 0x06006E91 RID: 28305 RVA: 0x0025072B File Offset: 0x0024E92B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x06006E92 RID: 28306 RVA: 0x00250744 File Offset: 0x0024E944
		public override bool SelectedUseOption(Pawn p)
		{
			if (this.PlayerChoosesTarget)
			{
				Find.Targeter.BeginTargeting(this.GetTargetingParameters(), delegate(LocalTargetInfo t)
				{
					this.target = t.Thing;
					this.parent.GetComp<CompUsable>().TryStartUseJob(p, this.target);
				}, p, null, null);
				return true;
			}
			this.target = null;
			return false;
		}

		// Token: 0x06006E93 RID: 28307 RVA: 0x0025079C File Offset: 0x0024E99C
		public override void DoEffect(Pawn usedBy)
		{
			if (this.PlayerChoosesTarget && this.target == null)
			{
				return;
			}
			if (this.target != null && !this.GetTargetingParameters().CanTarget(this.target, null))
			{
				return;
			}
			base.DoEffect(usedBy);
			foreach (Thing thing in this.GetTargets(this.target))
			{
				foreach (CompTargetEffect compTargetEffect in this.parent.GetComps<CompTargetEffect>())
				{
					compTargetEffect.DoEffectOn(usedBy, thing);
				}
				if (this.Props.moteOnTarget != null)
				{
					MoteMaker.MakeAttachedOverlay(thing, this.Props.moteOnTarget, Vector3.zero, 1f, -1f);
				}
				if (this.Props.fleckOnTarget != null)
				{
					FleckMaker.AttachedOverlay(thing, this.Props.fleckOnTarget, Vector3.zero, 1f, -1f);
				}
				if (this.Props.moteConnecting != null)
				{
					MoteMaker.MakeConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.moteConnecting, usedBy.Map, 1f);
				}
				if (this.Props.fleckConnecting != null)
				{
					FleckMaker.ConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.fleckConnecting, usedBy.Map, 1f);
				}
			}
			this.target = null;
		}

		// Token: 0x06006E94 RID: 28308
		protected abstract TargetingParameters GetTargetingParameters();

		// Token: 0x06006E95 RID: 28309
		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		// Token: 0x06006E96 RID: 28310 RVA: 0x0025094C File Offset: 0x0024EB4C
		public bool BaseTargetValidator(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (this.Props.psychicSensitiveTargetsOnly && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) <= 0f)
				{
					return false;
				}
				if (this.Props.ignoreQuestLodgerPawns && pawn.IsQuestLodger())
				{
					return false;
				}
				if (this.Props.ignorePlayerFactionPawns && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
			}
			if (this.Props.fleshCorpsesOnly)
			{
				Corpse corpse = t as Corpse;
				if (corpse != null && !corpse.InnerPawn.RaceProps.IsFlesh)
				{
					return false;
				}
			}
			if (this.Props.nonDessicatedCorpsesOnly)
			{
				Corpse corpse2 = t as Corpse;
				if (corpse2 != null && corpse2.GetRotStage() == RotStage.Dessicated)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003D45 RID: 15685
		private Thing target;
	}
}
