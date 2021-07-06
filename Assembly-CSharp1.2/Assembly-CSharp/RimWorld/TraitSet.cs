using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001570 RID: 5488
	public class TraitSet : IExposable
	{
		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x0600771F RID: 30495 RVA: 0x002438F4 File Offset: 0x00241AF4
		public float HungerRateFactor
		{
			get
			{
				float num = 1f;
				foreach (Trait trait in this.allTraits)
				{
					num *= trait.CurrentData.hungerRateFactor;
				}
				return num;
			}
		}

		// Token: 0x06007720 RID: 30496 RVA: 0x00050604 File Offset: 0x0004E804
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06007721 RID: 30497 RVA: 0x00243958 File Offset: 0x00241B58
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allTraits.RemoveAll((Trait x) => x == null) != 0)
				{
					Log.Error("Some traits were null after loading.", false);
				}
				if (this.allTraits.RemoveAll((Trait x) => x.def == null) != 0)
				{
					Log.Error("Some traits had null def after loading.", false);
				}
				for (int i = 0; i < this.allTraits.Count; i++)
				{
					this.allTraits[i].pawn = this.pawn;
				}
			}
		}

		// Token: 0x06007722 RID: 30498 RVA: 0x00243A24 File Offset: 0x00241C24
		public void GainTrait(Trait trait)
		{
			if (this.HasTrait(trait.def))
			{
				Log.Warning(this.pawn + " already has trait " + trait.def, false);
				return;
			}
			this.allTraits.Add(trait);
			trait.pawn = this.pawn;
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.pawn.skills != null)
			{
				this.pawn.skills.Notify_SkillDisablesChanged();
			}
			if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06007723 RID: 30499 RVA: 0x00243AF8 File Offset: 0x00241CF8
		public bool HasTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06007724 RID: 30500 RVA: 0x0005061E File Offset: 0x0004E81E
		public IEnumerable<MentalBreakDef> TheOnlyAllowedMentalBreaks
		{
			get
			{
				int num;
				for (int i = 0; i < this.allTraits.Count; i = num + 1)
				{
					Trait trait = this.allTraits[i];
					if (trait.CurrentData.theOnlyAllowedMentalBreaks != null)
					{
						for (int j = 0; j < trait.CurrentData.theOnlyAllowedMentalBreaks.Count; j = num + 1)
						{
							yield return trait.CurrentData.theOnlyAllowedMentalBreaks[j];
							num = j;
						}
					}
					trait = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06007725 RID: 30501 RVA: 0x00243B34 File Offset: 0x00241D34
		public Trait GetTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		// Token: 0x06007726 RID: 30502 RVA: 0x00243B7C File Offset: 0x00241D7C
		public int DegreeOfTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef)
				{
					return this.allTraits[i].Degree;
				}
			}
			return 0;
		}

		// Token: 0x04004E7E RID: 20094
		protected Pawn pawn;

		// Token: 0x04004E7F RID: 20095
		public List<Trait> allTraits = new List<Trait>();
	}
}
