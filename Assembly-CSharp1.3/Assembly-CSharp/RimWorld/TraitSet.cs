using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA6 RID: 3750
	public class TraitSet : IExposable
	{
		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x001DF3EE File Offset: 0x001DD5EE
		public bool AnyTraitHasIngestibleOverrides
		{
			get
			{
				return this.anyTraitHasIngestibleOverrides;
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x0600582E RID: 22574 RVA: 0x001DF3F8 File Offset: 0x001DD5F8
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

		// Token: 0x0600582F RID: 22575 RVA: 0x001DF45C File Offset: 0x001DD65C
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x001DF478 File Offset: 0x001DD678
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allTraits.RemoveAll((Trait x) => x == null) != 0)
				{
					Log.Error("Some traits were null after loading.");
				}
				if (this.allTraits.RemoveAll((Trait x) => x.def == null) != 0)
				{
					Log.Error("Some traits had null def after loading.");
				}
				for (int i = 0; i < this.allTraits.Count; i++)
				{
					this.allTraits[i].pawn = this.pawn;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CacheAnyTraitHasIngestibleOverrides();
			}
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x001DF550 File Offset: 0x001DD750
		public bool DisableHostilityFrom(Pawn p)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (p.Faction != null && this.allTraits[i].def.disableHostilityFromFaction == p.Faction.def)
				{
					return true;
				}
				if (this.allTraits[i].def.disableHostilityFromAnimalType != null)
				{
					AnimalType? disableHostilityFromAnimalType = this.allTraits[i].def.disableHostilityFromAnimalType;
					AnimalType animalType = p.RaceProps.animalType;
					if (disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x001DF600 File Offset: 0x001DD800
		public bool IsThoughtDisallowed(ThoughtDef thought)
		{
			if (this.pawn.story == null || thought == null)
			{
				return false;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				TraitDegreeData currentData = this.allTraits[i].CurrentData;
				if (currentData.disallowedThoughts != null)
				{
					for (int j = 0; j < currentData.disallowedThoughts.Count; j++)
					{
						if (currentData.disallowedThoughts[j] == thought)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x001DF678 File Offset: 0x001DD878
		public bool IsThoughtFromIngestionDisallowed(ThoughtDef thought, ThingDef ingestible, MeatSourceCategory meatSourceCategory)
		{
			if (thought == null || ingestible == null)
			{
				return false;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				TraitDegreeData currentData = this.allTraits[i].CurrentData;
				if (currentData.disallowedThoughtsFromIngestion != null)
				{
					for (int j = 0; j < currentData.disallowedThoughtsFromIngestion.Count; j++)
					{
						TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.disallowedThoughtsFromIngestion[j];
						if (!traitIngestionThoughtsOverride.thoughts.NullOrEmpty<ThoughtDef>())
						{
							if (traitIngestionThoughtsOverride.thing != null)
							{
								if (traitIngestionThoughtsOverride.thing != ingestible)
								{
									goto IL_92;
								}
							}
							else if (meatSourceCategory == MeatSourceCategory.NotMeat || traitIngestionThoughtsOverride.meatSource != meatSourceCategory)
							{
								goto IL_92;
							}
							for (int k = 0; k < traitIngestionThoughtsOverride.thoughts.Count; k++)
							{
								if (traitIngestionThoughtsOverride.thoughts[k] == thought)
								{
									return true;
								}
							}
						}
						IL_92:;
					}
				}
			}
			return false;
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x001DF740 File Offset: 0x001DD940
		public void GetExtraThoughtsFromIngestion(List<ThoughtDef> buffer, ThingDef ingestible, MeatSourceCategory meatSourceCategory, bool direct)
		{
			if (ingestible == null || buffer == null)
			{
				return;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				TraitDegreeData currentData = this.allTraits[i].CurrentData;
				if (currentData.extraThoughtsFromIngestion != null)
				{
					int j = 0;
					while (j < currentData.extraThoughtsFromIngestion.Count)
					{
						TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.extraThoughtsFromIngestion[j];
						if (traitIngestionThoughtsOverride.thing != null)
						{
							if (traitIngestionThoughtsOverride.thing == ingestible)
							{
								goto IL_5E;
							}
						}
						else if (meatSourceCategory != MeatSourceCategory.NotMeat && traitIngestionThoughtsOverride.meatSource == meatSourceCategory)
						{
							goto IL_5E;
						}
						IL_AF:
						j++;
						continue;
						IL_5E:
						if (!traitIngestionThoughtsOverride.thoughts.NullOrEmpty<ThoughtDef>())
						{
							buffer.AddRange(traitIngestionThoughtsOverride.thoughts);
						}
						if (direct)
						{
							if (!traitIngestionThoughtsOverride.thoughtsDirect.NullOrEmpty<ThoughtDef>())
							{
								buffer.AddRange(traitIngestionThoughtsOverride.thoughtsDirect);
								goto IL_AF;
							}
							goto IL_AF;
						}
						else
						{
							if (!traitIngestionThoughtsOverride.thoughtsAsIngredient.NullOrEmpty<ThoughtDef>())
							{
								buffer.AddRange(traitIngestionThoughtsOverride.thoughtsAsIngredient);
								goto IL_AF;
							}
							goto IL_AF;
						}
					}
				}
			}
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x001DF828 File Offset: 0x001DDA28
		public void GainTrait(Trait trait)
		{
			if (this.HasTrait(trait.def))
			{
				Log.Warning(this.pawn + " already has trait " + trait.def);
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
			List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
			if (!abilities.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < abilities.Count; i++)
				{
					this.pawn.abilities.GainAbility(abilities[i]);
				}
			}
			if (trait.def.disableHostilityFromAnimalType != null)
			{
				AnimalType? disableHostilityFromAnimalType = trait.def.disableHostilityFromAnimalType;
				AnimalType animalType = AnimalType.None;
				if (!(disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null))
				{
					goto IL_142;
				}
			}
			if (trait.def.disableHostilityFromFaction == null)
			{
				goto IL_16A;
			}
			IL_142:
			if (this.pawn.Map != null)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			IL_16A:
			this.CacheAnyTraitHasIngestibleOverrides();
			if (trait.CurrentData.needs != null && trait.CurrentData.needs.Count > 0)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x001DF9E0 File Offset: 0x001DDBE0
		public void RemoveTrait(Trait trait)
		{
			if (!this.HasTrait(trait.def))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Trying to remove ",
					trait.Label,
					" but ",
					this.pawn,
					" doesn't have it."
				}));
				return;
			}
			List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
			if (!abilities.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < abilities.Count; i++)
				{
					this.pawn.abilities.RemoveAbility(abilities[i]);
				}
			}
			this.allTraits.Remove(trait);
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.pawn.skills != null)
			{
				this.pawn.skills.Notify_SkillDisablesChanged();
			}
			if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (trait.def.disableHostilityFromAnimalType != null)
			{
				AnimalType? disableHostilityFromAnimalType = trait.def.disableHostilityFromAnimalType;
				AnimalType animalType = AnimalType.None;
				if (!(disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null))
				{
					goto IL_156;
				}
			}
			if (trait.def.disableHostilityFromFaction == null)
			{
				goto IL_17E;
			}
			IL_156:
			if (this.pawn.Map != null)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			IL_17E:
			this.CacheAnyTraitHasIngestibleOverrides();
			if (trait.CurrentData.needs != null && trait.CurrentData.needs.Count > 0)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x001DFBAC File Offset: 0x001DDDAC
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

		// Token: 0x06005838 RID: 22584 RVA: 0x001DFBE8 File Offset: 0x001DDDE8
		public bool HasTrait(TraitDef tDef, int degree)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && this.allTraits[i].Degree == degree)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06005839 RID: 22585 RVA: 0x001DFC36 File Offset: 0x001DDE36
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

		// Token: 0x0600583A RID: 22586 RVA: 0x001DFC48 File Offset: 0x001DDE48
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

		// Token: 0x0600583B RID: 22587 RVA: 0x001DFC90 File Offset: 0x001DDE90
		public Trait GetTrait(TraitDef tDef, int degree)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && this.allTraits[i].Degree == degree)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x001DFCEC File Offset: 0x001DDEEC
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

		// Token: 0x0600583D RID: 22589 RVA: 0x001DFD38 File Offset: 0x001DDF38
		private void CacheAnyTraitHasIngestibleOverrides()
		{
			this.anyTraitHasIngestibleOverrides = false;
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].CurrentData.ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
				{
					this.anyTraitHasIngestibleOverrides = true;
					return;
				}
			}
		}

		// Token: 0x040033E3 RID: 13283
		protected Pawn pawn;

		// Token: 0x040033E4 RID: 13284
		public List<Trait> allTraits = new List<Trait>();

		// Token: 0x040033E5 RID: 13285
		public bool anyTraitHasIngestibleOverrides;
	}
}
