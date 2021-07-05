using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC9 RID: 3785
	public class KidnappedPawnsTracker : IExposable
	{
		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x0600594A RID: 22858 RVA: 0x001E726B File Offset: 0x001E546B
		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x001E7273 File Offset: 0x001E5473
		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x001E7290 File Offset: 0x001E5490
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x001E72E8 File Offset: 0x001E54E8
		public void Kidnap(Pawn pawn, Pawn kidnapper)
		{
			if (this.kidnappedPawns.Contains(pawn))
			{
				Log.Error("Tried to kidnap already kidnapped pawn " + pawn);
				return;
			}
			if (pawn.Faction == this.faction)
			{
				Log.Error("Tried to kidnap pawn with the same faction: " + pawn);
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
					Log.Error("WorldPawns discarded kidnapped pawn.");
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

		// Token: 0x0600594E RID: 22862 RVA: 0x001E7432 File Offset: 0x001E5632
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
				Log.Warning("Tried to remove kidnapped pawn " + pawn + " but he's not here.");
			}
		}

		// Token: 0x0600594F RID: 22863 RVA: 0x001E746C File Offset: 0x001E566C
		public void LogKidnappedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.faction.Name + ":");
			for (int i = 0; i < this.kidnappedPawns.Count; i++)
			{
				stringBuilder.AppendLine(this.kidnappedPawns[i].Name.ToStringFull);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x001E74DC File Offset: 0x001E56DC
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

		// Token: 0x04003462 RID: 13410
		private Faction faction;

		// Token: 0x04003463 RID: 13411
		private List<Pawn> kidnappedPawns = new List<Pawn>();

		// Token: 0x04003464 RID: 13412
		private const int TryRecruitInterval = 15051;

		// Token: 0x04003465 RID: 13413
		private const float RecruitMTBDays = 30f;
	}
}
