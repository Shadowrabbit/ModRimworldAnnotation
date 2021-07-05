using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D77 RID: 7543
	public static class ThoughtUtility
	{
		// Token: 0x0600A3FB RID: 41979 RVA: 0x002FC0A4 File Offset: 0x002FA2A4
		public static void Reset()
		{
			ThoughtUtility.situationalSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && x.IsSocial
			select x).ToList<ThoughtDef>();
			ThoughtUtility.situationalNonSocialThoughtDefs = (from x in DefDatabase<ThoughtDef>.AllDefs
			where x.IsSituational && !x.IsSocial
			select x).ToList<ThoughtDef>();
		}

		// Token: 0x0600A3FC RID: 41980 RVA: 0x002FC118 File Offset: 0x002FA318
		public static void GiveThoughtsForPawnExecuted(Pawn victim, PawnExecutionKind kind)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			int forcedStage = 1;
			if (victim.guilt.IsGuilty)
			{
				forcedStage = 0;
			}
			else
			{
				switch (kind)
				{
				case PawnExecutionKind.GenericBrutal:
					forcedStage = 2;
					break;
				case PawnExecutionKind.GenericHumane:
					forcedStage = 1;
					break;
				case PawnExecutionKind.OrganHarvesting:
					forcedStage = 3;
					break;
				}
			}
			ThoughtDef def;
			if (victim.IsColonist)
			{
				def = ThoughtDefOf.KnowColonistExecuted;
			}
			else
			{
				def = ThoughtDefOf.KnowGuestExecuted;
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
			{
				if (pawn.IsColonist && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(def, forcedStage), null);
				}
			}
		}

		// Token: 0x0600A3FD RID: 41981 RVA: 0x002FC1F0 File Offset: 0x002FA3F0
		public static void GiveThoughtsForPawnOrganHarvested(Pawn victim)
		{
			if (!victim.RaceProps.Humanlike)
			{
				return;
			}
			ThoughtDef thoughtDef = null;
			if (victim.IsColonist)
			{
				thoughtDef = ThoughtDefOf.KnowColonistOrganHarvested;
			}
			else if (victim.HostFaction == Faction.OfPlayer)
			{
				thoughtDef = ThoughtDefOf.KnowGuestOrganHarvested;
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
			{
				if (pawn.needs.mood != null)
				{
					if (pawn == victim)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested, null);
					}
					else if (thoughtDef != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, null);
					}
				}
			}
		}

		// Token: 0x0600A3FE RID: 41982 RVA: 0x002FC2C0 File Offset: 0x002FA4C0
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

		// Token: 0x0600A3FF RID: 41983 RVA: 0x002FC36C File Offset: 0x002FA56C
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
			return null;
		}

		// Token: 0x0600A400 RID: 41984 RVA: 0x002FC3BC File Offset: 0x002FA5BC
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

		// Token: 0x0600A401 RID: 41985 RVA: 0x002FC410 File Offset: 0x002FA610
		public static void RemovePositiveBedroomThoughts(Pawn pawn)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBedroom, (Thought_Memory thought) => thought.MoodOffset() > 0f);
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefIf(ThoughtDefOf.SleptInBarracks, (Thought_Memory thought) => thought.MoodOffset() > 0f);
		}

		// Token: 0x0600A402 RID: 41986 RVA: 0x0006CC31 File Offset: 0x0006AE31
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static bool CanGetThought(Pawn pawn, ThoughtDef def)
		{
			return ThoughtUtility.CanGetThought_NewTemp(pawn, def, false);
		}

		// Token: 0x0600A403 RID: 41987 RVA: 0x002FC4A8 File Offset: 0x002FA6A8
		public static bool CanGetThought_NewTemp(Pawn pawn, ThoughtDef def, bool checkIfNullified = false)
		{
			try
			{
				if (!def.validWhileDespawned && !pawn.Spawned && !def.IsMemory)
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

		// Token: 0x0600A404 RID: 41988 RVA: 0x0006CC3B File Offset: 0x0006AE3B
		public static bool ThoughtNullified(Pawn pawn, ThoughtDef def)
		{
			return ThoughtUtility.NullifyingTrait(def, pawn) != null || ThoughtUtility.NullifyingHediff(def, pawn) != null || ThoughtUtility.NullifyingTale(def, pawn) != null;
		}

		// Token: 0x0600A405 RID: 41989 RVA: 0x002FC588 File Offset: 0x002FA788
		public static string ThoughtNullifiedMessage(Pawn pawn, ThoughtDef def)
		{
			TaggedString t = "ThoughtNullifiedBy".Translate().CapitalizeFirst() + ": ";
			Trait trait = ThoughtUtility.NullifyingTrait(def, pawn);
			if (trait != null)
			{
				return t + trait.LabelCap;
			}
			Hediff hediff = ThoughtUtility.NullifyingHediff(def, pawn);
			if (hediff != null)
			{
				return t + hediff.def.LabelCap;
			}
			TaleDef taleDef = ThoughtUtility.NullifyingTale(def, pawn);
			if (taleDef != null)
			{
				return t + taleDef.LabelCap;
			}
			return "";
		}

		// Token: 0x04006F29 RID: 28457
		public static List<ThoughtDef> situationalSocialThoughtDefs;

		// Token: 0x04006F2A RID: 28458
		public static List<ThoughtDef> situationalNonSocialThoughtDefs;
	}
}
