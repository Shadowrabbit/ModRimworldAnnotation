using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001507 RID: 5383
	public static class ThoughtUtility
	{
		// Token: 0x06008044 RID: 32836 RVA: 0x002D6E04 File Offset: 0x002D5004
		public static void Reset()
		{
			ThoughtUtility.situationalSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && x.IsSocial
			select x).ToList<ThoughtDef>();
			ThoughtUtility.situationalNonSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && !x.IsSocial
			select x).ToList<ThoughtDef>();
		}

		// Token: 0x06008045 RID: 32837 RVA: 0x002D6E78 File Offset: 0x002D5078
		public static void GiveThoughtsForPawnExecuted(Pawn victim, Pawn executioner, PawnExecutionKind kind)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			int num = 0;
			if (victim.guilt.IsGuilty)
			{
				num = 0;
			}
			else
			{
				switch (kind)
				{
				case PawnExecutionKind.GenericBrutal:
					num = 2;
					break;
				case PawnExecutionKind.GenericHumane:
					num = 1;
					break;
				case PawnExecutionKind.OrganHarvesting:
					num = 3;
					break;
				}
			}
			if (victim.IsPrisoner)
			{
				if (executioner.Faction != null)
				{
					executioner.Faction.lastExecutionTick = Find.TickManager.TicksGame;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedPrisoner, executioner.Named(HistoryEventArgsNames.Doer), num.Named(HistoryEventArgsNames.ExecutionThoughtStage)), true);
				if (victim.guilt.IsGuilty)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerGuilty, executioner.Named(HistoryEventArgsNames.Doer)), true);
					return;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedPrisonerInnocent, executioner.Named(HistoryEventArgsNames.Doer)), true);
				return;
			}
			else
			{
				if (victim.HostFaction != null)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedGuest, executioner.Named(HistoryEventArgsNames.Doer), num.Named(HistoryEventArgsNames.ExecutionThoughtStage)), true);
					return;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ExecutedColonist, executioner.Named(HistoryEventArgsNames.Doer), num.Named(HistoryEventArgsNames.ExecutionThoughtStage)), true);
				return;
			}
		}

		// Token: 0x06008046 RID: 32838 RVA: 0x002D6FD4 File Offset: 0x002D51D4
		public static void GiveThoughtsForPawnOrganHarvested(Pawn victim, Pawn billDoer)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			if (victim.needs.mood != null)
			{
				victim.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested, null, null);
			}
			if (ModsConfig.IdeologyActive)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrgan, billDoer.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (billDoer.needs.mood != null)
			{
				Pawn_StoryTracker story = billDoer.story;
				if (((story != null) ? story.traits : null) != null && billDoer.story.traits.HasTrait(TraitDefOf.Bloodlust))
				{
					billDoer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarvestedOrgan_Bloodlust, null, null);
				}
			}
			if (victim.IsColonist)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrganFromColonist, billDoer.Named(HistoryEventArgsNames.Doer)), true);
				return;
			}
			if (victim.HostFaction == Faction.OfPlayer)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.HarvestedOrganFromGuest, billDoer.Named(HistoryEventArgsNames.Doer)), true);
			}
		}

		// Token: 0x06008047 RID: 32839 RVA: 0x002D70F4 File Offset: 0x002D52F4
		public static Hediff NullifyingHediff(ThoughtDef def, Pawn pawn)
		{
			if (def.IsMemory)
			{
				return null;
			}
			float num = 0f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			Hediff result = null;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.pctConditionalThoughtsNullified > num)
				{
					num = curStage.pctConditionalThoughtsNullified;
					result = hediffs[i];
				}
			}
			if (num == 0f)
			{
				return null;
			}
			Rand.PushState();
			Rand.Seed = pawn.thingIDNumber * 31 + (int)(def.index * 139);
			bool flag = Rand.Value < num;
			Rand.PopState();
			if (!flag)
			{
				return null;
			}
			return result;
		}

		// Token: 0x06008048 RID: 32840 RVA: 0x002D71A0 File Offset: 0x002D53A0
		public static bool NeverNullified(ThoughtDef def, Pawn pawn)
		{
			if (!def.neverNullifyIfAnyTrait.NullOrEmpty<TraitDef>())
			{
				for (int i = 0; i < def.neverNullifyIfAnyTrait.Count; i++)
				{
					if (pawn.story.traits.GetTrait(def.neverNullifyIfAnyTrait[i]) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008049 RID: 32841 RVA: 0x002D71F4 File Offset: 0x002D53F4
		public static Trait NullifyingTrait(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingTraits != null)
			{
				for (int i = 0; i < def.nullifyingTraits.Count; i++)
				{
					Trait trait = pawn.story.traits.GetTrait(def.nullifyingTraits[i]);
					if (trait != null)
					{
						return trait;
					}
				}
			}
			if (def.nullifyingTraitDegrees != null)
			{
				for (int j = 0; j < def.nullifyingTraitDegrees.Count; j++)
				{
					Trait trait2 = def.nullifyingTraitDegrees[j].GetTrait(pawn);
					if (trait2 != null)
					{
						return trait2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600804A RID: 32842 RVA: 0x002D7278 File Offset: 0x002D5478
		public static TaleDef NullifyingTale(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingOwnTales != null)
			{
				for (int i = 0; i < def.nullifyingOwnTales.Count; i++)
				{
					if (Find.TaleManager.GetLatestTale(def.nullifyingOwnTales[i], pawn) != null)
					{
						return def.nullifyingOwnTales[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600804B RID: 32843 RVA: 0x002D72CC File Offset: 0x002D54CC
		public static PreceptDef NullifyingPrecept(ThoughtDef def, Pawn pawn)
		{
			if (def.nullifyingPrecepts != null)
			{
				for (int i = 0; i < def.nullifyingPrecepts.Count; i++)
				{
					if (pawn.Ideo != null && pawn.Ideo.HasPrecept(def.nullifyingPrecepts[i]))
					{
						return def.nullifyingPrecepts[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600804C RID: 32844 RVA: 0x002D7328 File Offset: 0x002D5528
		public static void RemovePositiveBedroomThoughts(Pawn pawn)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBedroom, (Thought_Memory thought) => thought.MoodOffset() > 0f);
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBarracks, (Thought_Memory thought) => thought.MoodOffset() > 0f);
		}

		// Token: 0x0600804D RID: 32845 RVA: 0x002D73C0 File Offset: 0x002D55C0
		public static bool CanGetThought(Pawn pawn, ThoughtDef def, bool checkIfNullified = false)
		{
			try
			{
				if (!def.validWhileDespawned && !pawn.Spawned && !def.IsMemory)
				{
					return false;
				}
				if (pawn.story.traits.IsThoughtDisallowed(def))
				{
					return false;
				}
				if (!def.requiredTraits.NullOrEmpty<TraitDef>())
				{
					bool flag = false;
					for (int i = 0; i < def.requiredTraits.Count; i++)
					{
						if (pawn.story.traits.HasTrait(def.requiredTraits[i]) && (!def.RequiresSpecificTraitsDegree || def.requiredTraitsDegree == pawn.story.traits.DegreeOfTrait(def.requiredTraits[i])))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (def.nullifiedIfNotColonist && !pawn.IsColonist)
				{
					return false;
				}
				if (checkIfNullified && ThoughtUtility.ThoughtNullified(pawn, def))
				{
					return false;
				}
			}
			finally
			{
			}
			return true;
		}

		// Token: 0x0600804E RID: 32846 RVA: 0x002D74BC File Offset: 0x002D56BC
		public static bool ThoughtNullified(Pawn pawn, ThoughtDef def)
		{
			return !ThoughtUtility.NeverNullified(def, pawn) && (ThoughtUtility.NullifyingTrait(def, pawn) != null || ThoughtUtility.NullifyingHediff(def, pawn) != null || ThoughtUtility.NullifyingTale(def, pawn) != null || ThoughtUtility.NullifyingPrecept(def, pawn) != null);
		}

		// Token: 0x0600804F RID: 32847 RVA: 0x002D74F8 File Offset: 0x002D56F8
		public static string ThoughtNullifiedMessage(Pawn pawn, ThoughtDef def)
		{
			if (ThoughtUtility.NeverNullified(def, pawn))
			{
				return "";
			}
			Trait trait = ThoughtUtility.NullifyingTrait(def, pawn);
			if (trait != null)
			{
				return "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": " + trait.LabelCap;
			}
			Hediff hediff = ThoughtUtility.NullifyingHediff(def, pawn);
			if (hediff != null)
			{
				return "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": " + hediff.def.LabelCap;
			}
			TaleDef taleDef = ThoughtUtility.NullifyingTale(def, pawn);
			if (taleDef != null)
			{
				return "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": " + taleDef.LabelCap;
			}
			PreceptDef preceptDef = ThoughtUtility.NullifyingPrecept(def, pawn);
			if (preceptDef != null)
			{
				return "DisabledByPrecept".Translate(preceptDef.issue.LabelCap) + ": " + preceptDef.LabelCap;
			}
			return "";
		}

		// Token: 0x06008050 RID: 32848 RVA: 0x002D760C File Offset: 0x002D580C
		public static bool Witnessed(Pawn p, Pawn victim)
		{
			if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			if (victim.IsCaravanMember())
			{
				return victim.GetCaravan() == p.GetCaravan();
			}
			return victim.Spawned && p.Spawned && p.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, p.Position, victim.Map, false, null, 0, 0);
		}

		// Token: 0x06008051 RID: 32849 RVA: 0x002D76A0 File Offset: 0x002D58A0
		public static IEnumerable<TraitRequirement> GetNullifyingTraits(ThoughtDef thoughtDef)
		{
			if (thoughtDef.nullifyingTraits != null)
			{
				int num;
				for (int i = 0; i < thoughtDef.nullifyingTraits.Count; i = num + 1)
				{
					yield return new TraitRequirement
					{
						def = thoughtDef.nullifyingTraits[i]
					};
					num = i;
				}
			}
			if (thoughtDef.nullifyingTraitDegrees != null)
			{
				int num;
				for (int i = 0; i < thoughtDef.nullifyingTraitDegrees.Count; i = num + 1)
				{
					yield return thoughtDef.nullifyingTraitDegrees[i];
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04004FE5 RID: 20453
		public static List<ThoughtDef> situationalSocialThoughtDefs;

		// Token: 0x04004FE6 RID: 20454
		public static List<ThoughtDef> situationalNonSocialThoughtDefs;
	}
}
