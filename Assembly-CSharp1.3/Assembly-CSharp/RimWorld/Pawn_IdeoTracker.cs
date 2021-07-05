using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E6A RID: 3690
	public class Pawn_IdeoTracker : IExposable
	{
		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x060055A4 RID: 21924 RVA: 0x001D02C1 File Offset: 0x001CE4C1
		public Ideo Ideo
		{
			get
			{
				return this.ideo;
			}
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x060055A5 RID: 21925 RVA: 0x001D02C9 File Offset: 0x001CE4C9
		public List<Ideo> PreviousIdeos
		{
			get
			{
				return this.previousIdeos;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x060055A6 RID: 21926 RVA: 0x001D02D1 File Offset: 0x001CE4D1
		public float Certainty
		{
			get
			{
				return this.certainty;
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x060055A7 RID: 21927 RVA: 0x001D02D9 File Offset: 0x001CE4D9
		public float CertaintyChangePerDay
		{
			get
			{
				return ConversionTuning.CertaintyPerDayByMoodCurve.Evaluate(this.pawn.needs.mood.CurLevelPercentage);
			}
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x001D02FA File Offset: 0x001CE4FA
		public Pawn_IdeoTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.previousIdeos = new List<Ideo>();
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x000033AC File Offset: 0x000015AC
		public Pawn_IdeoTracker()
		{
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x001D0314 File Offset: 0x001CE514
		public void IdeoTrackerTick()
		{
			if (!this.pawn.Destroyed && !this.pawn.InMentalState)
			{
				this.certainty += this.CertaintyChangePerDay / 60000f;
				this.certainty = Mathf.Clamp01(this.certainty);
			}
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x001D0368 File Offset: 0x001CE568
		public void SetIdeo(Ideo ideo)
		{
			if (this.ideo == ideo)
			{
				return;
			}
			if (this.ideo != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ChangedIdeo, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (this.previousIdeos.Contains(ideo))
			{
				this.previousIdeos.Remove(ideo);
			}
			if (this.ideo != null)
			{
				this.previousIdeos.Add(this.ideo);
			}
			Ideo ideo2 = this.ideo;
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				Ideo ideo3 = this.ideo;
				if (ideo3 != null)
				{
					ideo3.Notify_MemberLost(this.pawn, this.pawn.Map);
				}
				Ideo ideo4 = this.ideo;
				if (ideo4 != null)
				{
					ideo4.RecacheColonistBelieverCount();
				}
			}
			this.ideo = ideo;
			this.certainty = Mathf.Clamp01(ConversionTuning.InitialCertaintyRange.RandomInRange);
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				this.pawn.Faction.ideos.Notify_ColonistChangedIdeo();
				if (ideo2 != null)
				{
					ideo2.RecacheColonistBelieverCount();
				}
				Ideo ideo5 = this.ideo;
				if (ideo5 != null)
				{
					ideo5.RecacheColonistBelieverCount();
				}
			}
			if (this.pawn.ownership.OwnedBed != null && this.pawn.ownership.OwnedBed.CompAssignableToPawn.IdeoligionForbids(this.pawn))
			{
				this.pawn.ownership.UnclaimBed();
			}
			SpouseRelationUtility.RemoveSpousesAsForbiddenByIdeo(this.pawn);
			if (ideo != null && !ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.Bonded)))
			{
				List<Pawn> list = new List<Pawn>();
				List<DirectPawnRelation> directRelations = this.pawn.relations.DirectRelations;
				for (int i = directRelations.Count - 1; i >= 0; i--)
				{
					DirectPawnRelation directPawnRelation = directRelations[i];
					if (directPawnRelation.def == PawnRelationDefOf.Bond)
					{
						list.Add(directPawnRelation.otherPawn);
						this.pawn.relations.RemoveDirectRelation(directPawnRelation);
					}
				}
				if (list.Count > 0)
				{
					Find.LetterStack.ReceiveLetter("LetterBondRemoved".Translate(), "LetterBondRemovedDesc".Translate(ideo.Named("IDEO"), this.pawn.Named("PAWN"), (from b in list
					select b.LabelCap).ToLineList(null, false).Named("BONDS")), LetterDefOf.NeutralEvent, new LookTargets(list.Concat(new Pawn[]
					{
						this.pawn
					})), null, null, null, null);
				}
			}
			this.joinTick = Find.TickManager.TicksGame;
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			Pawn_NeedsTracker needs2 = this.pawn.needs;
			if (needs2 != null)
			{
				needs2.AddOrRemoveNeedsAsAppropriate();
			}
			Pawn_ApparelTracker apparel = this.pawn.apparel;
			if (apparel != null)
			{
				apparel.Notify_IdeoChanged();
			}
			Pawn_AbilityTracker abilities = this.pawn.abilities;
			if (abilities != null)
			{
				abilities.Notify_TemporaryAbilitiesChanged();
			}
			Pawn_AgeTracker ageTracker = this.pawn.ageTracker;
			if (ageTracker == null)
			{
				return;
			}
			ageTracker.Notify_IdeoChanged();
		}

		// Token: 0x060055AC RID: 21932 RVA: 0x001D06AC File Offset: 0x001CE8AC
		public bool IdeoConversionAttempt(float certaintyReduction, Ideo initiatorIdeo)
		{
			if (!ModLister.CheckIdeology("Ideoligion conversion"))
			{
				return false;
			}
			float f = Mathf.Clamp01(this.certainty - certaintyReduction);
			if (this.pawn.Spawned)
			{
				string text = "Certainty".Translate() + "\n" + this.certainty.ToStringPercent() + " -> " + f.ToStringPercent();
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, text, 8f);
			}
			this.certainty = f;
			if (this.certainty <= 0f)
			{
				this.SetIdeo(initiatorIdeo);
				this.certainty = 0.5f;
				return true;
			}
			return false;
		}

		// Token: 0x060055AD RID: 21933 RVA: 0x001D076B File Offset: 0x001CE96B
		public void Reassure(float certaintyGain)
		{
			this.OffsetCertainty(certaintyGain);
		}

		// Token: 0x060055AE RID: 21934 RVA: 0x001D0774 File Offset: 0x001CE974
		public void OffsetCertainty(float offset)
		{
			if (!ModLister.CheckIdeology("Ideoligion certainty"))
			{
				return;
			}
			float f = Mathf.Clamp01(this.certainty + offset);
			if (this.pawn.Spawned)
			{
				string text = "Certainty".Translate() + "\n" + this.certainty.ToStringPercent() + " -> " + f.ToStringPercent();
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, text, 8f);
			}
			this.certainty = f;
		}

		// Token: 0x060055AF RID: 21935 RVA: 0x001D0810 File Offset: 0x001CEA10
		public void Notify_IdeoRemoved(Ideo removedIdeo)
		{
			if (this.Ideo == removedIdeo)
			{
				if (this.pawn.Faction != null && this.pawn.Faction.ideos.PrimaryIdeo != removedIdeo)
				{
					this.SetIdeo(this.pawn.Faction.ideos.PrimaryIdeo);
				}
				else
				{
					this.SetIdeo(Find.IdeoManager.IdeosListForReading.RandomElement<Ideo>());
				}
			}
			if (this.previousIdeos.Contains(removedIdeo))
			{
				this.previousIdeos.Remove(removedIdeo);
			}
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x001D0898 File Offset: 0x001CEA98
		public void Debug_ReduceCertainty(float amt)
		{
			this.certainty -= amt;
			this.certainty = Mathf.Clamp01(this.certainty);
		}

		// Token: 0x060055B1 RID: 21937 RVA: 0x001D08BC File Offset: 0x001CEABC
		public void ExposeData()
		{
			Scribe_References.Look<Ideo>(ref this.ideo, "ideo", false);
			Scribe_Collections.Look<Ideo>(ref this.previousIdeos, "previousIdeos", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.certainty, "certainty", 0f, false);
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.previousIdeos.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Removed null ideos");
				}
				if (this.certainty <= 0f)
				{
					this.certainty = Mathf.Clamp01(ConversionTuning.InitialCertaintyRange.RandomInRange);
				}
			}
		}

		// Token: 0x040032A5 RID: 12965
		private Pawn pawn;

		// Token: 0x040032A6 RID: 12966
		public int joinTick;

		// Token: 0x040032A7 RID: 12967
		private Ideo ideo;

		// Token: 0x040032A8 RID: 12968
		private List<Ideo> previousIdeos;

		// Token: 0x040032A9 RID: 12969
		private float certainty;
	}
}
