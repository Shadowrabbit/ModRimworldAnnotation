using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B9 RID: 6329
	public abstract class CompTargetable : CompUseEffect
	{
		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x06008C73 RID: 35955 RVA: 0x0005E273 File Offset: 0x0005C473
		public CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x06008C74 RID: 35956
		protected abstract bool PlayerChoosesTarget { get; }

		// Token: 0x06008C75 RID: 35957 RVA: 0x0005E280 File Offset: 0x0005C480
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x06008C76 RID: 35958 RVA: 0x0028D250 File Offset: 0x0028B450
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

		// Token: 0x06008C77 RID: 35959 RVA: 0x0028D2A8 File Offset: 0x0028B4A8
		public override void DoEffect(Pawn usedBy)
		{
			if (this.PlayerChoosesTarget && this.target == null)
			{
				return;
			}
			if (this.target != null && !this.GetTargetingParameters().CanTarget(this.target))
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
				if (this.Props.moteConnecting != null)
				{
					MoteMaker.MakeConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.moteConnecting, usedBy.Map, 1f);
				}
			}
			this.target = null;
		}

		// Token: 0x06008C78 RID: 35960
		protected abstract TargetingParameters GetTargetingParameters();

		// Token: 0x06008C79 RID: 35961
		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		// Token: 0x06008C7A RID: 35962 RVA: 0x0028D3E0 File Offset: 0x0028B5E0
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

		// Token: 0x040059E7 RID: 23015
		private Thing target;
	}
}
