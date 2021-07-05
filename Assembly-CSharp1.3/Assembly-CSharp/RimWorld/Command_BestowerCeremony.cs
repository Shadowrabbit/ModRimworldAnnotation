using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000874 RID: 2164
	public class Command_BestowerCeremony : Command
	{
		// Token: 0x06003924 RID: 14628 RVA: 0x00140090 File Offset: 0x0013E290
		public Command_BestowerCeremony(LordJob_BestowingCeremony job, Pawn bestower, Pawn forPawn, Action<List<Pawn>> action)
		{
			this.bestower = bestower;
			this.forPawn = forPawn;
			this.action = action;
			this.job = job;
			this.defaultLabel = "BeginCeremony".Translate(this.forPawn);
			this.icon = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/BestowCeremony", true);
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x001400F4 File Offset: 0x0013E2F4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!JobDriver_BestowingCeremony.AnalyzeThroneRoom(this.bestower, this.forPawn))
			{
				this.disabledReason = "BestowingCeremonyThroneroomRequirementsNotSatisfiedShort".Translate(this.forPawn.Named("PAWN"), this.forPawn.royalty.GetTitleAwardedWhenUpdating(this.bestower.Faction, this.forPawn.royalty.GetFavor(this.bestower.Faction)).label.Named("TITLE"));
				this.disabled = true;
			}
			if (!this.job.GetSpot().IsValid)
			{
				this.disabledReason = "MessageBestowerUnreachable".Translate();
				this.disabled = true;
			}
			Lord lord = this.forPawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_Ritual)
			{
				this.disabledReason = "CantStartRitualTargetIsAlreadyInRitual".Translate(this.forPawn.LabelShort);
				this.disabled = true;
			}
			return base.GizmoOnGUI(topLeft, maxWidth, parms);
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x00140208 File Offset: 0x0013E408
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			WindowStack windowStack = Find.WindowStack;
			string header = "ChooseParticipantsBestow".Translate();
			string ritualLabel = "RitualBestowingCeremony".Translate();
			Precept_Ritual ritual = null;
			TargetInfo target = this.job.targetSpot.ToTargetInfo(this.bestower.Map);
			Map map = this.bestower.Map;
			Dialog_BeginRitual.ActionCallback actionCallback = delegate(RitualRoleAssignments assignments)
			{
				this.action((from p in assignments.Participants
				where p != this.bestower
				select p).ToList<Pawn>());
				return true;
			};
			Pawn organizer = this.bestower;
			RitualObligation obligation = null;
			Func<Pawn, bool, bool, bool> filter = delegate(Pawn pawn, bool voluntary, bool allowOtherIdeos)
			{
				Lord lord = pawn.GetLord();
				return (lord == null || !(lord.LordJob is LordJob_Ritual)) && !pawn.IsPrisonerOfColony && !pawn.RaceProps.Animal;
			};
			string confirmText = "Begin".Translate();
			List<Pawn> list = new List<Pawn>();
			list.Add(this.bestower);
			list.Add(this.forPawn);
			Dictionary<string, Pawn> forcedForRole = null;
			RitualOutcomeEffectDef bestowingCeremony = RitualOutcomeEffectDefOf.BestowingCeremony;
			windowStack.Add(new Dialog_BeginRitual(header, ritualLabel, ritual, target, map, actionCallback, organizer, obligation, filter, confirmText, list, forcedForRole, "RitualBestowingCeremony".Translate(), bestowingCeremony, null, null));
		}

		// Token: 0x04001F67 RID: 8039
		private Pawn bestower;

		// Token: 0x04001F68 RID: 8040
		private Pawn forPawn;

		// Token: 0x04001F69 RID: 8041
		private Action<List<Pawn>> action;

		// Token: 0x04001F6A RID: 8042
		private LordJob_BestowingCeremony job;
	}
}
