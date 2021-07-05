using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200152D RID: 5421
	public class Pawn_TrainingTracker : IExposable
	{
		// Token: 0x0600756A RID: 30058 RVA: 0x0004F324 File Offset: 0x0004D524
		public Pawn_TrainingTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.countDecayFrom = Find.TickManager.TicksGame;
		}

		// Token: 0x0600756B RID: 30059 RVA: 0x0023C564 File Offset: 0x0023A764
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

		// Token: 0x0600756C RID: 30060 RVA: 0x0004F364 File Offset: 0x0004D564
		public bool GetWanted(TrainableDef td)
		{
			return this.wantedTrainables[td];
		}

		// Token: 0x0600756D RID: 30061 RVA: 0x0004F372 File Offset: 0x0004D572
		private void SetWanted(TrainableDef td, bool wanted)
		{
			this.wantedTrainables[td] = wanted;
		}

		// Token: 0x0600756E RID: 30062 RVA: 0x0004F381 File Offset: 0x0004D581
		internal int GetSteps(TrainableDef td)
		{
			return this.steps[td];
		}

		// Token: 0x0600756F RID: 30063 RVA: 0x0023C5E4 File Offset: 0x0023A7E4
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

		// Token: 0x06007570 RID: 30064 RVA: 0x0004F38F File Offset: 0x0004D58F
		public bool HasLearned(TrainableDef td)
		{
			return this.learned[td];
		}

		// Token: 0x06007571 RID: 30065 RVA: 0x0023C648 File Offset: 0x0023A848
		public AcceptanceReport CanAssignToTrain(TrainableDef td)
		{
			bool flag;
			return this.CanAssignToTrain(td, out flag);
		}

		// Token: 0x06007572 RID: 30066 RVA: 0x0023C660 File Offset: 0x0023A860
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

		// Token: 0x06007573 RID: 30067 RVA: 0x0023C824 File Offset: 0x0023AA24
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

		// Token: 0x06007574 RID: 30068 RVA: 0x0023C870 File Offset: 0x0023AA70
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

		// Token: 0x06007575 RID: 30069 RVA: 0x0023C90C File Offset: 0x0023AB0C
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

		// Token: 0x06007576 RID: 30070 RVA: 0x0023C9C8 File Offset: 0x0023ABC8
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

		// Token: 0x06007577 RID: 30071 RVA: 0x0004F39D File Offset: 0x0004D59D
		public void Debug_MakeDegradeHappenSoon()
		{
			this.countDecayFrom = Find.TickManager.TicksGame - TrainableUtility.DegradationPeriodTicks(this.pawn.def) - 500;
		}

		// Token: 0x04004D6B RID: 19819
		private Pawn pawn;

		// Token: 0x04004D6C RID: 19820
		private DefMap<TrainableDef, bool> wantedTrainables = new DefMap<TrainableDef, bool>();

		// Token: 0x04004D6D RID: 19821
		private DefMap<TrainableDef, int> steps = new DefMap<TrainableDef, int>();

		// Token: 0x04004D6E RID: 19822
		private DefMap<TrainableDef, bool> learned = new DefMap<TrainableDef, bool>();

		// Token: 0x04004D6F RID: 19823
		private int countDecayFrom;
	}
}
