using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D17 RID: 3351
	public static class AbilityUtility
	{
		// Token: 0x06004EA3 RID: 20131 RVA: 0x001A5A02 File Offset: 0x001A3C02
		public static bool ValidateIsConscious(Pawn targetPawn, bool showMessages)
		{
			if (!targetPawn.health.capacities.CanBeAwake)
			{
				if (showMessages)
				{
					Messages.Message("AbilityCantApplyUnconscious".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x001A5A42 File Offset: 0x001A3C42
		public static bool ValidateIsAwake(Pawn targetPawn, bool showMessages)
		{
			if (!targetPawn.Awake())
			{
				if (showMessages)
				{
					Messages.Message("AbilityCantApplyAsleep".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x001A5A78 File Offset: 0x001A3C78
		public static bool ValidateNoMentalState(Pawn targetPawn, bool showMessages)
		{
			if (targetPawn.InMentalState)
			{
				if (showMessages)
				{
					Messages.Message("AbilityCantApplyToMentallyBroken".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x001A5AB0 File Offset: 0x001A3CB0
		public static bool ValidateIsMaddened(Pawn targetPawn, bool showMessages)
		{
			if (!targetPawn.InMentalState || targetPawn.MentalStateDef != MentalStateDefOf.Manhunter)
			{
				if (showMessages)
				{
					if (targetPawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
					{
						Messages.Message("AbilityCantApplyToScaria".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						Messages.Message("AbilityCanOnlyApplyToManhunter".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x001A5B3E File Offset: 0x001A3D3E
		public static bool ValidateCanWalk(Pawn targetPawn, bool showMessages)
		{
			if (targetPawn.Downed)
			{
				if (showMessages)
				{
					Messages.Message("AbilityCantApplyDowned".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x001A5B74 File Offset: 0x001A3D74
		public static bool ValidateHasMentalState(Pawn targetPawn, bool showMessages)
		{
			if (!targetPawn.InMentalState)
			{
				if (showMessages)
				{
					Messages.Message("AbilityCanOnlyApplyToMentallyBroken".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x001A5BAC File Offset: 0x001A3DAC
		public static bool ValidateHasResistance(Pawn targetPawn, bool showMessages)
		{
			Pawn_GuestTracker guest = targetPawn.guest;
			if (guest != null && guest.resistance <= 1E-45f)
			{
				if (showMessages)
				{
					Messages.Message("AbilityCantApplyNoResistance".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x001A5C03 File Offset: 0x001A3E03
		public static bool ValidateNoInspiration(Pawn targetPawn, bool showMessages)
		{
			if (targetPawn.Inspiration != null)
			{
				if (showMessages)
				{
					Messages.Message("AbilityAlreadyInspired".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x001A5C39 File Offset: 0x001A3E39
		public static bool ValidateCanGetInspiration(Pawn targetPawn, bool showMessages)
		{
			if (targetPawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef() == null)
			{
				if (showMessages)
				{
					Messages.Message("AbilityCantGetInspiredNow".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x001A5C79 File Offset: 0x001A3E79
		public static bool ValidateMustBeHuman(Pawn targetPawn, bool showMessages)
		{
			if (targetPawn.NonHumanlikeOrWildMan())
			{
				if (showMessages)
				{
					Messages.Message((targetPawn.IsWildMan() ? "AbilityMustBeHumanNonWild" : "AbilityMustBeHuman").Translate(), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x001A5CB8 File Offset: 0x001A3EB8
		public static bool ValidateMustBeAnimal(Pawn targetPawn, bool showMessages)
		{
			if (!targetPawn.RaceProps.Animal)
			{
				if (showMessages)
				{
					Messages.Message("AbilityMustBeAnimal".Translate(), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001A5CF0 File Offset: 0x001A3EF0
		public static bool ValidateNotSameIdeo(Pawn casterPawn, Pawn targetPawn, bool showMessages)
		{
			if (casterPawn.Ideo != null && casterPawn.Ideo == targetPawn.Ideo)
			{
				if (showMessages)
				{
					Messages.Message("AbilityMustBeNotSameIdeo".Translate(targetPawn, casterPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x001A5D48 File Offset: 0x001A3F48
		public static bool ValidateSameIdeo(Pawn casterPawn, Pawn targetPawn, bool showMessages)
		{
			if (casterPawn.Ideo != targetPawn.Ideo)
			{
				if (showMessages)
				{
					Messages.Message("AbilityMustBeSameIdeo".Translate(targetPawn, casterPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x001A5D98 File Offset: 0x001A3F98
		public static bool ValidateSickOrInjured(Pawn targetPawn, bool showMessage)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (Hediff hediff in targetPawn.health.hediffSet.hediffs)
			{
				flag |= (hediff.TryGetComp<HediffComp_Immunizable>() != null);
				Hediff_Injury hd;
				flag2 |= ((hd = (hediff as Hediff_Injury)) != null && !hd.IsPermanent() && hd.CanHealNaturally());
			}
			if (!flag && !flag2)
			{
				if (showMessage)
				{
					Messages.Message("AbilityMustBeSickOrInjured".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001A5E54 File Offset: 0x001A4054
		public static bool ValidateNotGuilty(Pawn targetPawn, bool showMessages)
		{
			if (targetPawn.guilt != null && targetPawn.guilt.IsGuilty)
			{
				if (showMessages)
				{
					Messages.Message("AbilityMustBeNotGuilty".Translate(targetPawn), targetPawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x001A5EA2 File Offset: 0x001A40A2
		public static void DoClamor(IntVec3 cell, float radius, Thing source, ClamorDef clamor)
		{
			if (clamor != null)
			{
				GenClamor.DoClamor(source, cell, radius, clamor);
			}
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x001A5EB0 File Offset: 0x001A40B0
		public static Ability MakeAbility(AbilityDef def, Pawn pawn)
		{
			return Activator.CreateInstance(def.abilityClass, new object[]
			{
				pawn,
				def
			}) as Ability;
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x001A5ED0 File Offset: 0x001A40D0
		public static Ability MakeAbility(AbilityDef def, Pawn pawn, Precept sourcePrecept)
		{
			return Activator.CreateInstance(def.abilityClass, new object[]
			{
				pawn,
				sourcePrecept,
				def
			}) as Ability;
		}

		// Token: 0x06004EB5 RID: 20149 RVA: 0x001A5EF4 File Offset: 0x001A40F4
		public static void DoTable_AbilityCosts()
		{
			List<TableDataGetter<AbilityDef>> list = new List<TableDataGetter<AbilityDef>>();
			list.Add(new TableDataGetter<AbilityDef>("name", (AbilityDef a) => a.LabelCap));
			list.Add(new TableDataGetter<AbilityDef>("level", (AbilityDef a) => a.level));
			list.Add(new TableDataGetter<AbilityDef>("heat cost", (AbilityDef a) => a.EntropyGain));
			list.Add(new TableDataGetter<AbilityDef>("psyfocus cost", delegate(AbilityDef a)
			{
				if (a.PsyfocusCostRange.Span > 1E-45f)
				{
					return string.Concat(new object[]
					{
						a.PsyfocusCostRange.min * 100f,
						"-",
						a.PsyfocusCostRange.max * 100f,
						"%"
					});
				}
				return a.PsyfocusCostRange.max.ToStringPercent();
			}));
			list.Add(new TableDataGetter<AbilityDef>("max psyfocus recovery time days", delegate(AbilityDef a)
			{
				if (a.PsyfocusCostRange.Span <= 1E-45f)
				{
					return a.PsyfocusCostRange.min / StatDefOf.MeditationFocusGain.defaultBaseValue;
				}
				return a.PsyfocusCostRange.min / StatDefOf.MeditationFocusGain.defaultBaseValue + "-" + a.PsyfocusCostRange.max / StatDefOf.MeditationFocusGain.defaultBaseValue;
			}));
			DebugTables.MakeTablesDialog<AbilityDef>(DefDatabase<AbilityDef>.AllDefsListForReading, list.ToArray());
		}
	}
}
