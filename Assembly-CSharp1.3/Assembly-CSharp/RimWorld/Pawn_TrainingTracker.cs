using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7C RID: 3708
	public class Pawn_TrainingTracker : IExposable
	{
		// Token: 0x060056CA RID: 22218 RVA: 0x001D72BA File Offset: 0x001D54BA
		public Pawn_TrainingTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.countDecayFrom = Find.TickManager.TicksGame;
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x001D72FC File Offset: 0x001D54FC
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.wantedTrainables, "wantedTrainables", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TrainableDef, int>>(ref this.steps, "steps", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.learned, "learned", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.countDecayFrom, "countDecayFrom", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.PawnTrainingTrackerPostLoadInit(this, ref this.wantedTrainables, ref this.steps, ref this.learned);
			}
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x001D737A File Offset: 0x001D557A
		public bool GetWanted(TrainableDef td)
		{
			return this.wantedTrainables[td];
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x001D7388 File Offset: 0x001D5588
		private void SetWanted(TrainableDef td, bool wanted)
		{
			this.wantedTrainables[td] = wanted;
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x001D7397 File Offset: 0x001D5597
		internal int GetSteps(TrainableDef td)
		{
			return this.steps[td];
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x001D73A8 File Offset: 0x001D55A8
		public bool CanBeTrained(TrainableDef td)
		{
			if (this.steps[td] >= td.steps)
			{
				return false;
			}
			List<TrainableDef> prerequisites = td.prerequisites;
			if (!prerequisites.NullOrEmpty<TrainableDef>())
			{
				for (int i = 0; i < prerequisites.Count; i++)
				{
					if (!this.HasLearned(prerequisites[i]) || this.CanBeTrained(prerequisites[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x001D740C File Offset: 0x001D560C
		public bool HasLearned(TrainableDef td)
		{
			return this.learned[td];
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x001D741C File Offset: 0x001D561C
		public AcceptanceReport CanAssignToTrain(TrainableDef td)
		{
			bool flag;
			return this.CanAssignToTrain(td, out flag);
		}

		// Token: 0x060056D2 RID: 22226 RVA: 0x001D7434 File Offset: 0x001D5634
		public AcceptanceReport CanAssignToTrain(TrainableDef td, out bool visible)
		{
			if (this.pawn.RaceProps.untrainableTags != null)
			{
				for (int i = 0; i < this.pawn.RaceProps.untrainableTags.Count; i++)
				{
					if (td.MatchesTag(this.pawn.RaceProps.untrainableTags[i]))
					{
						visible = false;
						return false;
					}
				}
			}
			if (this.pawn.RaceProps.trainableTags != null)
			{
				int j = 0;
				while (j < this.pawn.RaceProps.trainableTags.Count)
				{
					if (td.MatchesTag(this.pawn.RaceProps.trainableTags[j]))
					{
						if (this.pawn.BodySize < td.minBodySize)
						{
							visible = true;
							return new AcceptanceReport("CannotTrainTooSmall".Translate(this.pawn.LabelCapNoCount, this.pawn).Resolve());
						}
						visible = true;
						return true;
					}
					else
					{
						j++;
					}
				}
			}
			if (!td.defaultTrainable)
			{
				visible = false;
				return false;
			}
			if (this.pawn.BodySize < td.minBodySize)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainTooSmall".Translate(this.pawn.LabelCapNoCount, this.pawn).Resolve());
			}
			if (this.pawn.RaceProps.trainability.intelligenceOrder < td.requiredTrainability.intelligenceOrder)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainNotSmartEnough".Translate(td.requiredTrainability.label).Resolve());
			}
			visible = true;
			return true;
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x001D75F8 File Offset: 0x001D57F8
		public TrainableDef NextTrainableToTrain()
		{
			List<TrainableDef> trainableDefsInListOrder = TrainableUtility.TrainableDefsInListOrder;
			for (int i = 0; i < trainableDefsInListOrder.Count; i++)
			{
				if (this.GetWanted(trainableDefsInListOrder[i]) && this.CanBeTrained(trainableDefsInListOrder[i]))
				{
					return trainableDefsInListOrder[i];
				}
			}
			return null;
		}

		// Token: 0x060056D4 RID: 22228 RVA: 0x001D7644 File Offset: 0x001D5844
		public void Train(TrainableDef td, Pawn trainer, bool complete = false)
		{
			if (complete)
			{
				this.steps[td] = td.steps;
			}
			else
			{
				DefMap<TrainableDef, int> defMap = this.steps;
				int num = defMap[td];
				defMap[td] = num + 1;
			}
			if (this.steps[td] >= td.steps)
			{
				this.learned[td] = true;
				if (td == TrainableDefOf.Obedience && trainer != null && this.pawn.playerSettings != null && this.pawn.playerSettings.Master == null)
				{
					this.pawn.playerSettings.Master = trainer;
				}
			}
		}

		// Token: 0x060056D5 RID: 22229 RVA: 0x001D76E0 File Offset: 0x001D58E0
		public void SetWantedRecursive(TrainableDef td, bool checkOn)
		{
			this.SetWanted(td, checkOn);
			if (checkOn)
			{
				if (td.prerequisites != null)
				{
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						this.SetWantedRecursive(td.prerequisites[i], true);
					}
					return;
				}
			}
			else
			{
				foreach (TrainableDef td2 in from t in DefDatabase<TrainableDef>.AllDefsListForReading
				where t.prerequisites != null && t.prerequisites.Contains(td)
				select t)
				{
					this.SetWantedRecursive(td2, false);
				}
			}
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x001D779C File Offset: 0x001D599C
		public void TrainingTrackerTickRare()
		{
			if (this.pawn.Suspended)
			{
				this.countDecayFrom += 250;
				return;
			}
			if (!this.pawn.Spawned)
			{
				this.countDecayFrom += 250;
				return;
			}
			if (this.steps[TrainableDefOf.Tameness] == 0)
			{
				this.countDecayFrom = Find.TickManager.TicksGame;
				return;
			}
			if (Find.TickManager.TicksGame < this.countDecayFrom + TrainableUtility.DegradationPeriodTicks(this.pawn.def))
			{
				return;
			}
			if (this.pawn.RaceProps.animalType == AnimalType.Dryad)
			{
				return;
			}
			TrainableDef trainableDef = (from kvp in this.steps
			where kvp.Value > 0
			select kvp.Key).Except((from kvp in this.steps
			where kvp.Value > 0 && kvp.Key.prerequisites != null
			select kvp).SelectMany((KeyValuePair<TrainableDef, int> kvp) => kvp.Key.prerequisites)).RandomElement<TrainableDef>();
			if (trainableDef == TrainableDefOf.Tameness && !TrainableUtility.TamenessCanDecay(this.pawn.def))
			{
				this.countDecayFrom = Find.TickManager.TicksGame;
				return;
			}
			this.countDecayFrom = Find.TickManager.TicksGame;
			DefMap<TrainableDef, int> defMap = this.steps;
			TrainableDef def = trainableDef;
			int value = defMap[def] - 1;
			defMap[def] = value;
			if (this.steps[trainableDef] <= 0 && this.learned[trainableDef])
			{
				this.learned[trainableDef] = false;
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					if (trainableDef == TrainableDefOf.Tameness)
					{
						this.pawn.SetFaction(null, null);
						Messages.Message("MessageAnimalReturnedWild".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
						return;
					}
					Messages.Message("MessageAnimalLostSkill".Translate(this.pawn.LabelShort, trainableDef.LabelCap, this.pawn.Named("ANIMAL")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x001D7A2A File Offset: 0x001D5C2A
		public void Debug_MakeDegradeHappenSoon()
		{
			this.countDecayFrom = Find.TickManager.TicksGame - TrainableUtility.DegradationPeriodTicks(this.pawn.def) - 500;
		}

		// Token: 0x04003335 RID: 13109
		public Pawn pawn;

		// Token: 0x04003336 RID: 13110
		private DefMap<TrainableDef, bool> wantedTrainables = new DefMap<TrainableDef, bool>();

		// Token: 0x04003337 RID: 13111
		private DefMap<TrainableDef, int> steps = new DefMap<TrainableDef, int>();

		// Token: 0x04003338 RID: 13112
		private DefMap<TrainableDef, bool> learned = new DefMap<TrainableDef, bool>();

		// Token: 0x04003339 RID: 13113
		private int countDecayFrom;
	}
}
