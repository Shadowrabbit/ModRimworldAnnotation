using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A8 RID: 5544
	public class KidnappedPawnsTracker : IExposable
	{
		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06007865 RID: 30821 RVA: 0x00051149 File Offset: 0x0004F349
		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		// Token: 0x06007866 RID: 30822 RVA: 0x00051151 File Offset: 0x0004F351
		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x0024A02C File Offset: 0x0024822C
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06007868 RID: 30824 RVA: 0x0024A084 File Offset: 0x00248284
		public void Kidnap(Pawn pawn, Pawn kidnapper)
		{
			if (this.kidnappedPawns.Contains(pawn))
			{
				Log.Error("Tried to kidnap already kidnapped pawn " + pawn, false);
				return;
			}
			if (pawn.Faction == this.faction)
			{
				Log.Error("Tried to kidnap pawn with the same faction: " + pawn, false);
				return;
			}
			pawn.PreKidnapped(kidnapper);
			if (pawn.Spawned)
			{
				pawn.DeSpawn(DestroyMode.Vanish);
			}
			this.kidnappedPawns.Add(pawn);
			if (!Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				if (!Find.WorldPawns.Contains(pawn))
				{
					Log.Error("WorldPawns discarded kidnapped pawn.", false);
					this.kidnappedPawns.Remove(pawn);
				}
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, PawnDiedOrDownedThoughtsKind.Lost);
				BillUtility.Notify_ColonistUnavailable(pawn);
				if (kidnapper != null)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelPawnsKidnapped".Translate(pawn.Named("PAWN")), "LetterPawnsKidnapped".Translate(pawn.Named("PAWN"), kidnapper.Faction.Named("FACTION")), LetterDefOf.NegativeEvent, null);
				}
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "Kidnapped", pawn.Named("SUBJECT"), kidnapper.Named("KIDNAPPER"));
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x06007869 RID: 30825 RVA: 0x0005116B File Offset: 0x0004F36B
		public void RemoveKidnappedPawn(Pawn pawn)
		{
			if (this.kidnappedPawns.Remove(pawn))
			{
				if (pawn.Faction == Faction.OfPlayer)
				{
					PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(pawn);
					return;
				}
			}
			else
			{
				Log.Warning("Tried to remove kidnapped pawn " + pawn + " but he's not here.", false);
			}
		}

		// Token: 0x0600786A RID: 30826 RVA: 0x0024A1DC File Offset: 0x002483DC
		public void LogKidnappedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.faction.Name + ":");
			for (int i = 0; i < this.kidnappedPawns.Count; i++)
			{
				stringBuilder.AppendLine(this.kidnappedPawns[i].Name.ToStringFull);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600786B RID: 30827 RVA: 0x0024A24C File Offset: 0x0024844C
		public void KidnappedPawnsTrackerTick()
		{
			for (int i = this.kidnappedPawns.Count - 1; i >= 0; i--)
			{
				if (this.kidnappedPawns[i].DestroyedOrNull())
				{
					this.kidnappedPawns.RemoveAt(i);
				}
			}
			if (Find.TickManager.TicksGame % 15051 == 0)
			{
				for (int j = this.kidnappedPawns.Count - 1; j >= 0; j--)
				{
					if (Rand.MTBEventOccurs(30f, 60000f, 15051f))
					{
						this.kidnappedPawns[j].SetFaction(this.faction, null);
						this.kidnappedPawns.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x04004F57 RID: 20311
		private Faction faction;

		// Token: 0x04004F58 RID: 20312
		private List<Pawn> kidnappedPawns = new List<Pawn>();

		// Token: 0x04004F59 RID: 20313
		private const int TryRecruitInterval = 15051;

		// Token: 0x04004F5A RID: 20314
		private const float RecruitMTBDays = 30f;
	}
}
