using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F72 RID: 3954
	public class RitualOutcomeEffectWorker_Bestowing : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x06005DC5 RID: 24005 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_Bestowing()
		{
		}

		// Token: 0x06005DC6 RID: 24006 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_Bestowing(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DC7 RID: 24007 RVA: 0x0020280C File Offset: 0x00200A0C
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			RitualOutcomeEffectWorker_Bestowing.<>c__DisplayClass2_0 CS$<>8__locals1 = new RitualOutcomeEffectWorker_Bestowing.<>c__DisplayClass2_0();
			LordJob_BestowingCeremony lordJob_BestowingCeremony = (LordJob_BestowingCeremony)jobRitual;
			Pawn target = lordJob_BestowingCeremony.target;
			Pawn bestower = lordJob_BestowingCeremony.bestower;
			Hediff_Psylink mainPsylinkSource = target.GetMainPsylinkSource();
			float quality = base.GetQuality(jobRitual, progress);
			OutcomeChance outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = target;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			RoyalTitleDef currentTitle = target.royalty.GetCurrentTitle(bestower.Faction);
			RoyalTitleDef titleAwardedWhenUpdating = target.royalty.GetTitleAwardedWhenUpdating(bestower.Faction, target.royalty.GetFavor(bestower.Faction));
			string text2;
			string str;
			Pawn_RoyaltyTracker.MakeLetterTextForTitleChange(target, bestower.Faction, currentTitle, titleAwardedWhenUpdating, out text2, out str);
			if (target.royalty != null)
			{
				target.royalty.TryUpdateTitle(bestower.Faction, false, titleAwardedWhenUpdating);
			}
			RitualOutcomeEffectWorker_Bestowing.<>c__DisplayClass2_0 CS$<>8__locals2 = CS$<>8__locals1;
			List<AbilityDef> abilitiesPreUpdate;
			if (mainPsylinkSource != null)
			{
				abilitiesPreUpdate = (from a in target.abilities.abilities
				select a.def).ToList<AbilityDef>();
			}
			else
			{
				abilitiesPreUpdate = new List<AbilityDef>();
			}
			CS$<>8__locals2.abilitiesPreUpdate = abilitiesPreUpdate;
			ThingOwner<Thing> innerContainer = bestower.inventory.innerContainer;
			Thing thing = innerContainer.First((Thing t) => t.def == ThingDefOf.PsychicAmplifier);
			innerContainer.Remove(thing);
			thing.Destroy(DestroyMode.Vanish);
			for (int i = target.GetPsylinkLevel(); i < target.GetMaxPsylinkLevelByTitle(); i++)
			{
				target.ChangePsylinkLevel(1, false);
				Find.History.Notify_PsylinkAvailable();
			}
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				Pawn key = keyValuePair.Key;
				if (key != target)
				{
					key.needs.mood.thoughts.memories.TryGainMemory(outcome.memory, null, null);
				}
			}
			int num = 0;
			for (int j = this.def.honorFromQuality.PointsCount - 1; j >= 0; j--)
			{
				if (quality >= this.def.honorFromQuality[j].x)
				{
					num = (int)this.def.honorFromQuality[j].y;
					break;
				}
			}
			if (num > 0)
			{
				target.royalty.GainFavor(bestower.Faction, num);
			}
			List<AbilityDef> list;
			if (mainPsylinkSource != null)
			{
				list = (from a in target.abilities.abilities
				select a.def into def
				where !CS$<>8__locals1.abilitiesPreUpdate.Contains(def)
				select def).ToList<AbilityDef>();
			}
			else
			{
				list = new List<AbilityDef>();
			}
			List<AbilityDef> newAbilities = list;
			string text3 = text2;
			text3 = text3 + "\n\n" + Hediff_Psylink.MakeLetterTextNewPsylinkLevel(lordJob_BestowingCeremony.target, target.GetPsylinkLevel(), newAbilities);
			text3 = text3 + "\n\n" + str;
			if (text != null)
			{
				text3 = text3 + "\n\n" + text;
			}
			Find.LetterStack.ReceiveLetter("LetterLabelGainedRoyalTitle".Translate(titleAwardedWhenUpdating.GetLabelCapFor(target).Named("TITLE"), target.Named("PAWN")), text3, LetterDefOf.RitualOutcomePositive, lookTargets, lordJob_BestowingCeremony.bestower.Faction, null, null, null);
			string str2 = this.OutcomeDesc(outcome, quality, progress, lordJob_BestowingCeremony, num, totalPresence.Count);
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), "RitualBestowingCeremony".Translate().Named("RITUALLABEL")), str2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, target, null, null, null, null);
		}

		// Token: 0x06005DC8 RID: 24008 RVA: 0x00202BD8 File Offset: 0x00200DD8
		private string OutcomeDesc(OutcomeChance outcome, float quality, float progress, LordJob_BestowingCeremony jobRitual, int honor, int totalPresence)
		{
			TaggedString taggedString = "BestowingOutcomeQualitySpecific".Translate(quality.ToStringPercent()) + ":\n";
			Pawn target = jobRitual.target;
			Pawn bestower = jobRitual.bestower;
			if (this.def.startingQuality > 0f)
			{
				taggedString += "\n  - " + "StartingRitualQuality".Translate(this.def.startingQuality.ToStringPercent()) + ".";
			}
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality && ritualOutcomeComp.Applies(jobRitual) && Mathf.Abs(ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp))) >= 1E-45f)
				{
					taggedString += "\n  - " + ritualOutcomeComp.GetDesc(jobRitual, base.DataForComp(ritualOutcomeComp)).CapitalizeFirst();
				}
			}
			if (progress < 1f)
			{
				taggedString += "\n  - " + "RitualOutcomeProgress".Translate("RitualBestowingCeremony".Translate()) + ": x" + Mathf.Lerp(RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min, RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max, progress).ToStringPercent();
			}
			taggedString += "\n\n";
			if (honor > 0)
			{
				taggedString += "LetterPartBestowingExtraHonor".Translate(target.Named("PAWN"), honor, bestower.Faction.Named("FACTION"), totalPresence);
			}
			else
			{
				taggedString += "LetterPartNoExtraHonor".Translate(target.Named("PAWN"));
			}
			taggedString += "\n\n" + "RitualOutcomeChances".Translate(quality.ToStringPercent()) + ":\n";
			float num = 0f;
			foreach (OutcomeChance outcomeChance in this.def.outcomeChances)
			{
				num += (outcomeChance.Positive ? (outcomeChance.chance * quality) : outcomeChance.chance);
			}
			foreach (OutcomeChance outcomeChance2 in this.def.outcomeChances)
			{
				taggedString += "\n  - ";
				if (outcomeChance2.Positive)
				{
					taggedString += outcomeChance2.memory.stages[0].LabelCap + ": " + (outcomeChance2.chance * quality / num).ToStringPercent();
				}
				else
				{
					taggedString += outcomeChance2.memory.stages[0].LabelCap + ": " + (outcomeChance2.chance / num).ToStringPercent();
				}
			}
			return taggedString;
		}
	}
}
