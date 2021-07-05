using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8B RID: 3723
	public class Pawn_RelationsTracker : IExposable
	{
		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06005738 RID: 22328 RVA: 0x001D9DDE File Offset: 0x001D7FDE
		public List<DirectPawnRelation> DirectRelations
		{
			get
			{
				return this.directRelations;
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06005739 RID: 22329 RVA: 0x001D9DE6 File Offset: 0x001D7FE6
		public IEnumerable<Pawn> Children
		{
			get
			{
				foreach (Pawn pawn in this.pawnsWithDirectRelationsWithMe)
				{
					List<DirectPawnRelation> list = pawn.relations.directRelations;
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].otherPawn == this.pawn && list[i].def == PawnRelationDefOf.Parent)
						{
							yield return pawn;
							break;
						}
					}
				}
				HashSet<Pawn>.Enumerator enumerator = default(HashSet<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x0600573A RID: 22330 RVA: 0x001D9DF6 File Offset: 0x001D7FF6
		public int ChildrenCount
		{
			get
			{
				return this.Children.Count<Pawn>();
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x0600573B RID: 22331 RVA: 0x001D9E03 File Offset: 0x001D8003
		public bool RelatedToAnyoneOrAnyoneRelatedToMe
		{
			get
			{
				return this.directRelations.Any<DirectPawnRelation>() || this.pawnsWithDirectRelationsWithMe.Any<Pawn>();
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x0600573C RID: 22332 RVA: 0x001D9E20 File Offset: 0x001D8020
		public IEnumerable<Pawn> FamilyByBlood
		{
			get
			{
				if (this.canCacheFamilyByBlood)
				{
					if (!this.familyByBloodIsCached)
					{
						this.cachedFamilyByBlood.Clear();
						foreach (Pawn item in this.FamilyByBlood_Internal)
						{
							this.cachedFamilyByBlood.Add(item);
						}
						this.familyByBloodIsCached = true;
					}
					return this.cachedFamilyByBlood;
				}
				return this.FamilyByBlood_Internal;
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x0600573D RID: 22333 RVA: 0x001D9EA4 File Offset: 0x001D80A4
		private IEnumerable<Pawn> FamilyByBlood_Internal
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> familyStack = null;
				List<Pawn> familyChildrenStack = null;
				HashSet<Pawn> familyVisited = null;
				try
				{
					familyStack = SimplePool<List<Pawn>>.Get();
					familyChildrenStack = SimplePool<List<Pawn>>.Get();
					familyVisited = SimplePool<HashSet<Pawn>>.Get();
					familyStack.Add(this.pawn);
					familyVisited.Add(this.pawn);
					while (familyStack.Any<Pawn>())
					{
						Pawn p = familyStack[familyStack.Count - 1];
						familyStack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						Pawn father = p.GetFather();
						if (father != null && !familyVisited.Contains(father))
						{
							familyStack.Add(father);
							familyVisited.Add(father);
						}
						Pawn mother = p.GetMother();
						if (mother != null && !familyVisited.Contains(mother))
						{
							familyStack.Add(mother);
							familyVisited.Add(mother);
						}
						familyChildrenStack.Clear();
						familyChildrenStack.Add(p);
						while (familyChildrenStack.Any<Pawn>())
						{
							Pawn child = familyChildrenStack[familyChildrenStack.Count - 1];
							familyChildrenStack.RemoveLast<Pawn>();
							if (child != p && child != this.pawn)
							{
								yield return child;
							}
							foreach (Pawn item in child.relations.Children)
							{
								if (!familyVisited.Contains(item))
								{
									familyChildrenStack.Add(item);
									familyVisited.Add(item);
								}
							}
							child = null;
						}
						p = null;
					}
				}
				finally
				{
					familyStack.Clear();
					SimplePool<List<Pawn>>.Return(familyStack);
					familyChildrenStack.Clear();
					SimplePool<List<Pawn>>.Return(familyChildrenStack);
					familyVisited.Clear();
					SimplePool<HashSet<Pawn>>.Return(familyVisited);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x0600573E RID: 22334 RVA: 0x001D9EB4 File Offset: 0x001D80B4
		public IEnumerable<Pawn> PotentiallyRelatedPawns
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> stack = null;
				HashSet<Pawn> visited = null;
				try
				{
					stack = SimplePool<List<Pawn>>.Get();
					visited = SimplePool<HashSet<Pawn>>.Get();
					stack.Add(this.pawn);
					visited.Add(this.pawn);
					while (stack.Any<Pawn>())
					{
						Pawn p = stack[stack.Count - 1];
						stack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						for (int i = 0; i < p.relations.directRelations.Count; i++)
						{
							Pawn otherPawn = p.relations.directRelations[i].otherPawn;
							if (!visited.Contains(otherPawn))
							{
								stack.Add(otherPawn);
								visited.Add(otherPawn);
							}
						}
						foreach (Pawn item in p.relations.pawnsWithDirectRelationsWithMe)
						{
							if (!visited.Contains(item))
							{
								stack.Add(item);
								visited.Add(item);
							}
						}
						p = null;
					}
				}
				finally
				{
					stack.Clear();
					SimplePool<List<Pawn>>.Return(stack);
					visited.Clear();
					SimplePool<HashSet<Pawn>>.Return(visited);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x0600573F RID: 22335 RVA: 0x001D9EC4 File Offset: 0x001D80C4
		public IEnumerable<Pawn> RelatedPawns
		{
			get
			{
				this.canCacheFamilyByBlood = true;
				this.familyByBloodIsCached = false;
				this.cachedFamilyByBlood.Clear();
				try
				{
					foreach (Pawn pawn in this.PotentiallyRelatedPawns)
					{
						if ((this.familyByBloodIsCached && this.cachedFamilyByBlood.Contains(pawn)) || this.pawn.GetRelations(pawn).Any<PawnRelationDef>())
						{
							yield return pawn;
						}
					}
					IEnumerator<Pawn> enumerator = null;
				}
				finally
				{
					this.canCacheFamilyByBlood = false;
					this.familyByBloodIsCached = false;
					this.cachedFamilyByBlood.Clear();
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x001D9ED4 File Offset: 0x001D80D4
		public Pawn_RelationsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x001D9F0C File Offset: 0x001D810C
		public void ExposeData()
		{
			Scribe_Collections.Look<DirectPawnRelation>(ref this.directRelations, "directRelations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].otherPawn == null)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Pawn ",
							this.pawn,
							" has relation \"",
							this.directRelations[i].def.defName,
							"\" with null pawn after loading. This means that we forgot to serialize pawns somewhere (e.g. pawns from passing trade ships)."
						}));
					}
				}
				this.directRelations.RemoveAll((DirectPawnRelation x) => x.otherPawn == null);
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					this.directRelations[j].otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
				}
			}
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", true, false);
			Scribe_Values.Look<bool>(ref this.canGetRescuedThought, "canGetRescuedThought", true, false);
			Scribe_Values.Look<MarriageNameChange>(ref this.nextMarriageNameChange, "nextMarriageNameChange", MarriageNameChange.NoChange, false);
			Scribe_References.Look<Pawn>(ref this.relativeInvolvedInRescueQuest, "relativeInvolvedInRescueQuest", false);
			Scribe_Values.Look<bool>(ref this.hidePawnRelations, "hidePawnRelations", false, false);
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x001DA06E File Offset: 0x001D826E
		public void RelationsTrackerTick()
		{
			if (this.pawn.Dead)
			{
				return;
			}
			this.Tick_CheckStartMarriageCeremony();
			this.Tick_CheckDevelopBondRelation();
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x001DA08C File Offset: 0x001D828C
		public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.");
				return null;
			}
			return this.directRelations.Find((DirectPawnRelation x) => x.def == def && x.otherPawn == otherPawn);
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x001DA0E8 File Offset: 0x001D82E8
		public Pawn GetFirstDirectRelationPawn(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.");
				return null;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x001DA154 File Offset: 0x001D8354
		public int GetDirectRelationsCount(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			int num = 0;
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.");
				return num;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x001DA1C0 File Offset: 0x001D83C0
		public bool DirectRelationExists(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.");
				return false;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x001DA220 File Offset: 0x001D8420
		public void AddDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to directly add implied pawn relation ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}));
				return;
			}
			if (otherPawn == this.pawn)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add pawn relation ",
					def,
					" with self, pawn=",
					this.pawn
				}));
				return;
			}
			if (this.DirectRelationExists(def, otherPawn))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add the same relation twice: ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}));
				return;
			}
			int startTicks = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
			def.Worker.OnRelationCreated(this.pawn, otherPawn);
			this.directRelations.Add(new DirectPawnRelation(def, otherPawn, startTicks));
			otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
			if (def.reflexive)
			{
				otherPawn.relations.directRelations.Add(new DirectPawnRelation(def, this.pawn, startTicks));
				this.pawnsWithDirectRelationsWithMe.Add(otherPawn);
			}
			this.GainedOrLostDirectRelation();
			otherPawn.relations.GainedOrLostDirectRelation();
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (!this.pawn.Dead && this.pawn.health != null)
				{
					for (int i = this.pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						this.pawn.health.hediffSet.hediffs[i].Notify_RelationAdded(otherPawn, def);
					}
				}
				if (!otherPawn.Dead && otherPawn.health != null)
				{
					for (int j = otherPawn.health.hediffSet.hediffs.Count - 1; j >= 0; j--)
					{
						otherPawn.health.hediffSet.hediffs[j].Notify_RelationAdded(this.pawn, def);
					}
				}
			}
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x001DA441 File Offset: 0x001D8641
		public void RemoveDirectRelation(DirectPawnRelation relation)
		{
			this.RemoveDirectRelation(relation.def, relation.otherPawn);
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x001DA458 File Offset: 0x001D8658
		public void RemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (!this.TryRemoveDirectRelation(def, otherPawn))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not remove relation ",
					def,
					" because it's not here. pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}));
			}
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x001DA4A8 File Offset: 0x001D86A8
		public bool TryRemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to remove implied pawn relation ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}));
				return false;
			}
			Predicate<DirectPawnRelation> <>9__1;
			Predicate<DirectPawnRelation> <>9__2;
			Predicate<DirectPawnRelation> <>9__0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == def && this.directRelations[i].otherPawn == otherPawn)
				{
					if (def.reflexive)
					{
						List<DirectPawnRelation> list = otherPawn.relations.directRelations;
						Predicate<DirectPawnRelation> match;
						if ((match = <>9__1) == null)
						{
							match = (<>9__1 = ((DirectPawnRelation x) => x.def == def && x.otherPawn == this.pawn));
						}
						DirectPawnRelation item = list.Find(match);
						list.Remove(item);
						Predicate<DirectPawnRelation> match2;
						if ((match2 = <>9__2) == null)
						{
							match2 = (<>9__2 = ((DirectPawnRelation x) => x.otherPawn == this.pawn));
						}
						if (list.Find(match2) == null)
						{
							this.pawnsWithDirectRelationsWithMe.Remove(otherPawn);
						}
					}
					this.directRelations.RemoveAt(i);
					List<DirectPawnRelation> list2 = this.directRelations;
					Predicate<DirectPawnRelation> match3;
					if ((match3 = <>9__0) == null)
					{
						match3 = (<>9__0 = ((DirectPawnRelation x) => x.otherPawn == otherPawn));
					}
					if (list2.Find(match3) == null)
					{
						otherPawn.relations.pawnsWithDirectRelationsWithMe.Remove(this.pawn);
					}
					this.GainedOrLostDirectRelation();
					otherPawn.relations.GainedOrLostDirectRelation();
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x001DA668 File Offset: 0x001D8868
		public int OpinionOf(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return 0;
			}
			if (this.pawn.Dead)
			{
				return 0;
			}
			int num = 0;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
			{
				num += pawnRelationDef.opinionOffset;
			}
			if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				num += this.pawn.needs.mood.thoughts.TotalOpinionOffset(other);
			}
			if (num != 0)
			{
				float num2 = 1f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].CurStage != null)
					{
						num2 *= hediffs[i].CurStage.opinionOfOthersFactor;
					}
				}
				num = Mathf.RoundToInt((float)num * num2);
			}
			if (num > 0 && this.pawn.HostileTo(other))
			{
				num = 0;
			}
			return Mathf.Clamp(num, -100, 100);
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001DA7AC File Offset: 0x001D89AC
		public string OpinionExplanation(Pawn other)
		{
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("OpinionOf".Translate(other.LabelShort, other).Colorize(ColoredText.TipSectionTitleColor) + ": " + this.OpinionOf(other).ToStringWithSign());
			string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(other, this.pawn);
			if (!pawnSituationLabel.NullOrEmpty())
			{
				stringBuilder.AppendLine(pawnSituationLabel);
			}
			stringBuilder.AppendLine("--------------");
			bool flag = false;
			if (this.pawn.Dead)
			{
				stringBuilder.AppendLine("IAmDead".Translate());
				flag = true;
			}
			else
			{
				foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
				{
					stringBuilder.AppendLine(pawnRelationDef.GetGenderSpecificLabelCap(other) + ": " + pawnRelationDef.opinionOffset.ToStringWithSign());
					flag = true;
				}
				if (this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
				{
					ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
					thoughts.GetDistinctSocialThoughtGroups(other, Pawn_RelationsTracker.tmpSocialThoughts);
					for (int i = 0; i < Pawn_RelationsTracker.tmpSocialThoughts.Count; i++)
					{
						ISocialThought socialThought = Pawn_RelationsTracker.tmpSocialThoughts[i];
						int num = 1;
						Thought thought = (Thought)socialThought;
						if (thought.def.IsMemory)
						{
							num = thoughts.memories.NumMemoriesInGroup((Thought_MemorySocial)socialThought);
						}
						stringBuilder.Append(thought.LabelCapSocial);
						if (num != 1)
						{
							stringBuilder.Append(" x" + num);
						}
						stringBuilder.AppendLine(": " + thoughts.OpinionOffsetOfGroup(socialThought, other).ToStringWithSign());
						flag = true;
					}
				}
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int j = 0; j < hediffs.Count; j++)
				{
					HediffStage curStage = hediffs[j].CurStage;
					if (curStage != null && curStage.opinionOfOthersFactor != 1f)
					{
						stringBuilder.Append(hediffs[j].LabelBaseCap);
						if (curStage.opinionOfOthersFactor != 0f)
						{
							stringBuilder.AppendLine(": x" + curStage.opinionOfOthersFactor.ToStringPercent());
						}
						else
						{
							stringBuilder.AppendLine();
						}
						flag = true;
					}
				}
				if (this.pawn.HostileTo(other))
				{
					stringBuilder.AppendLine("Hostile".Translate());
					flag = true;
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLine("NoneBrackets".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x001DAAB8 File Offset: 0x001D8CB8
		public float SecondaryLovinChanceFactor(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			if (this.pawn.story != null && this.pawn.story.traits != null)
			{
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					return 0f;
				}
				if (!this.pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
				{
					if (this.pawn.story.traits.HasTrait(TraitDefOf.Gay))
					{
						if (otherPawn.gender != this.pawn.gender)
						{
							return 0f;
						}
					}
					else if (otherPawn.gender == this.pawn.gender)
					{
						return 0f;
					}
				}
			}
			float ageBiologicalYearsFloat = this.pawn.ageTracker.AgeBiologicalYearsFloat;
			float ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
			if (ageBiologicalYearsFloat < 16f || ageBiologicalYearsFloat2 < 16f)
			{
				return 0f;
			}
			float num = 1f;
			if (this.pawn.gender == Gender.Male)
			{
				float min = ageBiologicalYearsFloat - 30f;
				float lower = ageBiologicalYearsFloat - 10f;
				float upper = ageBiologicalYearsFloat + 3f;
				float max = ageBiologicalYearsFloat + 10f;
				num = GenMath.FlatHill(0.2f, min, lower, upper, max, 0.2f, ageBiologicalYearsFloat2);
			}
			else if (this.pawn.gender == Gender.Female)
			{
				float min2 = ageBiologicalYearsFloat - 10f;
				float lower2 = ageBiologicalYearsFloat - 3f;
				float upper2 = ageBiologicalYearsFloat + 10f;
				float max2 = ageBiologicalYearsFloat + 30f;
				num = GenMath.FlatHill(0.2f, min2, lower2, upper2, max2, 0.2f, ageBiologicalYearsFloat2);
			}
			float num2 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat);
			float num3 = Mathf.InverseLerp(16f, 18f, ageBiologicalYearsFloat2);
			float num4 = 0f;
			if (otherPawn.RaceProps.Humanlike)
			{
				num4 = otherPawn.GetStatValue(StatDefOf.PawnBeauty, true);
			}
			float num5 = 1f;
			if (num4 < 0f)
			{
				num5 = 0.3f;
			}
			else if (num4 > 0f)
			{
				num5 = 2.3f;
			}
			return num * num2 * num3 * num5;
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x001DACE0 File Offset: 0x001D8EE0
		public float SecondaryRomanceChanceFactor(Pawn otherPawn)
		{
			float num = 1f;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(otherPawn))
			{
				num *= pawnRelationDef.romanceChanceFactor;
			}
			float num2 = 1f;
			HediffWithTarget hediffWithTarget = (HediffWithTarget)this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (hediffWithTarget != null && hediffWithTarget.target == otherPawn)
			{
				num2 = 10f;
			}
			return this.SecondaryLovinChanceFactor(otherPawn) * num * num2;
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x001DAD84 File Offset: 0x001D8F84
		public float CompatibilityWith(Pawn otherPawn)
		{
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				return 0f;
			}
			float x = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYearsFloat - otherPawn.ageTracker.AgeBiologicalYearsFloat);
			float num = Mathf.Clamp(GenMath.LerpDouble(0f, 20f, 0.45f, -0.45f, x), -0.45f, 0.45f);
			float num2 = this.ConstantPerPawnsPairCompatibilityOffset(otherPawn.thingIDNumber);
			return num + num2;
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x001DAE0D File Offset: 0x001D900D
		public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
		{
			Rand.PushState();
			Rand.Seed = (this.pawn.thingIDNumber ^ otherPawnID) * 37;
			float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
			Rand.PopState();
			return result;
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x001DAE44 File Offset: 0x001D9044
		public void ClearAllRelations()
		{
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveDirectRelation(list[i]);
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x001DAEF8 File Offset: 0x001D90F8
		public void ClearAllNonBloodRelations()
		{
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].def.familyByBloodRelation)
				{
					this.RemoveDirectRelation(list[i]);
				}
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn && !list3[j].def.familyByBloodRelation)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x001DAFD4 File Offset: 0x001D91D4
		internal void Notify_PawnKilled(DamageInfo? dinfo, Map mapBeforeDeath)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
			if (this.everSeenByPlayer && !PawnGenerator.IsBeingGenerated(this.pawn) && !this.pawn.RaceProps.Animal)
			{
				this.AffectBondedAnimalsOnMyDeath();
			}
			this.Notify_FailedRescueQuest();
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x001DB084 File Offset: 0x001D9284
		public void Notify_PassedToWorld()
		{
			if (!this.pawn.Dead)
			{
				this.relativeInvolvedInRescueQuest = null;
			}
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x001DB09A File Offset: 0x001D929A
		public void Notify_ExitedMap()
		{
			this.CheckRescued();
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x001DB0A2 File Offset: 0x001D92A2
		public void Notify_ChangedFaction()
		{
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				this.CheckRescued();
			}
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x001DB0BC File Offset: 0x001D92BC
		public void Notify_PawnSold(Pawn playerNegotiator)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(this.pawn);
					if (mostImportantRelation != null && mostImportantRelation.soldThoughts != null)
					{
						if (mostImportantRelation == PawnRelationDefOf.Bond)
						{
							this.pawn.relations.RemoveDirectRelation(mostImportantRelation, pawn);
						}
						foreach (ThoughtDef def in mostImportantRelation.soldThoughts)
						{
							pawn.needs.mood.thoughts.memories.TryGainMemory(def, playerNegotiator, null);
						}
					}
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x001DB1B8 File Offset: 0x001D93B8
		public void Notify_PawnKidnapped()
		{
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001DB1C0 File Offset: 0x001D93C0
		public void Notify_RescuedBy(Pawn rescuer)
		{
			if (rescuer.RaceProps.Humanlike && this.pawn.needs.mood != null && this.canGetRescuedThought)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer, null);
				this.canGetRescuedThought = false;
			}
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x001DB224 File Offset: 0x001D9424
		public void Notify_FailedRescueQuest()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageFailedToRescueRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, this.pawn, null);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x001DB2F8 File Offset: 0x001D94F8
		private void CheckRescued()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageRescuedRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort, this.pawn.Named("PAWN"), this.relativeInvolvedInRescueQuest.Named("RELATIVE")), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, this.pawn, null);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x001DB3CC File Offset: 0x001D95CC
		public float GetFriendDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, (float)opinion));
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x001DB3EE File Offset: 0x001D95EE
		public float GetRivalDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, (float)opinion));
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x001DB410 File Offset: 0x001D9610
		private void RemoveMySpouseMarriageRelatedThoughts()
		{
			foreach (Pawn pawn in this.pawn.GetSpouses(false))
			{
				if (pawn.needs.mood != null)
				{
					MemoryThoughtHandler memories = pawn.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
					memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
				}
			}
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x001DB49C File Offset: 0x001D969C
		public void CheckAppendBondedAnimalDiedInfo(ref TaggedString letter, ref TaggedString label)
		{
			if (!this.pawn.RaceProps.Animal || !this.everSeenByPlayer || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				return;
			}
			Predicate<Pawn> isAffected = (Pawn x) => !x.Dead && (!x.RaceProps.Humanlike || !x.story.traits.HasTrait(TraitDefOf.Psychopath));
			int num = 0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[i].otherPawn))
				{
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			TaggedString t;
			if (num == 1)
			{
				Pawn firstDirectRelationPawn = this.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => isAffected(x));
				t = "LetterPartBondedAnimalDied".Translate(this.pawn.LabelDefinite(), firstDirectRelationPawn.LabelShort, this.pawn.Named("ANIMAL"), firstDirectRelationPawn.Named("HUMAN")).CapitalizeFirst();
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					if (this.directRelations[j].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[j].otherPawn))
					{
						stringBuilder.AppendLine("  - " + this.directRelations[j].otherPawn.LabelShort);
					}
				}
				t = "LetterPartBondedAnimalDiedMulti".Translate(stringBuilder.ToString().TrimEndNewlines());
			}
			label += " (" + "LetterLabelSuffixBondedAnimalDied".Translate() + ")";
			if (!letter.NullOrEmpty())
			{
				letter += "\n\n";
			}
			letter += t;
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x001DB6C0 File Offset: 0x001D98C0
		private void AffectBondedAnimalsOnMyDeath()
		{
			int num = 0;
			Pawn pawn = null;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && this.directRelations[i].otherPawn.Spawned)
				{
					pawn = this.directRelations[i].otherPawn;
					num++;
					float value = Rand.Value;
					MentalStateDef stateDef;
					if (value < 0.25f)
					{
						stateDef = MentalStateDefOf.Wander_Sad;
					}
					else if (value < 0.5f)
					{
						stateDef = MentalStateDefOf.Wander_Psychotic;
					}
					else if (value < 0.75f)
					{
						stateDef = MentalStateDefOf.Berserk;
					}
					else
					{
						stateDef = MentalStateDefOf.Manhunter;
					}
					this.directRelations[i].otherPawn.mindState.mentalStateHandler.TryStartMentalState(stateDef, "MentalStateReason_BondedHumanDeath".Translate(this.pawn).Resolve(), true, false, null, false, false, false);
				}
			}
			if (num == 1)
			{
				TaggedString taggedString;
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					taggedString = "MessageNamedBondedAnimalMentalBreak".Translate(pawn.KindLabelIndefinite(), pawn.Name.ToStringShort, this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				else
				{
					taggedString = "MessageBondedAnimalMentalBreak".Translate(pawn.LabelIndefinite(), this.pawn.LabelShort, pawn.Named("ANIMAL"), this.pawn.Named("HUMAN"));
				}
				Messages.Message(taggedString.CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall, true);
				return;
			}
			if (num > 1)
			{
				Messages.Message("MessageBondedAnimalsMentalBreak".Translate(num, this.pawn.LabelShort, this.pawn.Named("HUMAN")), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
		}

		// Token: 0x06005761 RID: 22369 RVA: 0x001DB8DC File Offset: 0x001D9ADC
		private void Tick_CheckStartMarriageCeremony()
		{
			if (!this.pawn.Spawned || this.pawn.RaceProps.Animal)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(1017))
			{
				int ticksGame = Find.TickManager.TicksGame;
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					float num = (float)(ticksGame - this.directRelations[i].startTicks) / 60000f;
					if (this.directRelations[i].def == PawnRelationDefOf.Fiance && this.pawn.thingIDNumber < this.directRelations[i].otherPawn.thingIDNumber && num > 10f && Rand.MTBEventOccurs(2f, 60000f, 1017f) && this.pawn.Map == this.directRelations[i].otherPawn.Map && this.pawn.Map.IsPlayerHome && MarriageCeremonyUtility.AcceptableGameConditionsToStartCeremony(this.pawn.Map) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.pawn, this.directRelations[i].otherPawn) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.directRelations[i].otherPawn, this.pawn))
					{
						this.pawn.Map.lordsStarter.TryStartMarriageCeremony(this.pawn, this.directRelations[i].otherPawn);
					}
				}
			}
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x001DBA78 File Offset: 0x001D9C78
		private void Tick_CheckDevelopBondRelation()
		{
			if (!this.pawn.Spawned || !this.pawn.RaceProps.Animal || this.pawn.Faction != Faction.OfPlayer || this.pawn.playerSettings.RespectedMaster == null)
			{
				return;
			}
			Pawn respectedMaster = this.pawn.playerSettings.RespectedMaster;
			if (this.pawn.IsHashIntervalTick(2500) && this.pawn.Map == respectedMaster.Map && this.pawn.Position.InHorDistOf(respectedMaster.Position, 12f) && GenSight.LineOfSight(this.pawn.Position, respectedMaster.Position, this.pawn.Map, false, null, 0, 0))
			{
				RelationsUtility.TryDevelopBondRelation(respectedMaster, this.pawn, 0.001f);
			}
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x001DBB58 File Offset: 0x001D9D58
		private void GainedOrLostDirectRelation()
		{
			if (Current.ProgramState == ProgramState.Playing && !this.pawn.Dead && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x0400339D RID: 13213
		private Pawn pawn;

		// Token: 0x0400339E RID: 13214
		private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

		// Token: 0x0400339F RID: 13215
		public bool everSeenByPlayer;

		// Token: 0x040033A0 RID: 13216
		public bool canGetRescuedThought = true;

		// Token: 0x040033A1 RID: 13217
		public Pawn relativeInvolvedInRescueQuest;

		// Token: 0x040033A2 RID: 13218
		public MarriageNameChange nextMarriageNameChange;

		// Token: 0x040033A3 RID: 13219
		public bool hidePawnRelations;

		// Token: 0x040033A4 RID: 13220
		private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

		// Token: 0x040033A5 RID: 13221
		private HashSet<Pawn> cachedFamilyByBlood = new HashSet<Pawn>();

		// Token: 0x040033A6 RID: 13222
		private bool familyByBloodIsCached;

		// Token: 0x040033A7 RID: 13223
		private bool canCacheFamilyByBlood;

		// Token: 0x040033A8 RID: 13224
		private const int CheckDevelopBondRelationIntervalTicks = 2500;

		// Token: 0x040033A9 RID: 13225
		private const float MaxBondRelationCheckDist = 12f;

		// Token: 0x040033AA RID: 13226
		private const float BondRelationPerIntervalChance = 0.001f;

		// Token: 0x040033AB RID: 13227
		public const int FriendOpinionThreshold = 20;

		// Token: 0x040033AC RID: 13228
		public const int RivalOpinionThreshold = -20;

		// Token: 0x040033AD RID: 13229
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();
	}
}
