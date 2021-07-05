using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200106A RID: 4202
	public class Building_ArchonexusCore : Building
	{
		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x0600638C RID: 25484 RVA: 0x0021A150 File Offset: 0x00218350
		public bool CanActivateNow
		{
			get
			{
				return !ArchonexusCountdown.CountdownActivated;
			}
		}

		// Token: 0x0600638D RID: 25485 RVA: 0x0021A15A File Offset: 0x0021835A
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					action = new Action(this.Activate),
					disabled = !this.CanActivateNow,
					defaultLabel = "Dev: Activate archonexus core"
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600638E RID: 25486 RVA: 0x0021A16A File Offset: 0x0021836A
		public void Activate()
		{
			if (!this.CanActivateNow)
			{
				return;
			}
			ArchonexusCountdown.InitiateCountdown(this);
		}

		// Token: 0x0600638F RID: 25487 RVA: 0x0021A17B File Offset: 0x0021837B
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			if (!this.CanActivateNow)
			{
				yield return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "AlreadyInvoked".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			this.tmpPawnsCanReach.Clear();
			for (int i = 0; i < selPawns.Count; i++)
			{
				if (selPawns[i].CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					this.tmpPawnsCanReach.Add(selPawns[i]);
				}
			}
			if (this.tmpPawnsCanReach.NullOrEmpty<Pawn>())
			{
				yield return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return new FloatMenuOption("Invoke".Translate("Power".Translate()), delegate()
			{
				this.tmpPawnsCanReach[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.ActivateArchonexusCore, this), new JobTag?(JobTag.Misc), false);
				for (int j = 1; j < this.tmpPawnsCanReach.Count; j++)
				{
					FloatMenuMakerMap.PawnGotoAction(base.Position, this.tmpPawnsCanReach[j], RCellFinder.BestOrderedGotoDestNear(this.InteractionCell, this.tmpPawnsCanReach[j], null));
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x0400385D RID: 14429
		private List<Pawn> tmpPawnsCanReach = new List<Pawn>();
	}
}
