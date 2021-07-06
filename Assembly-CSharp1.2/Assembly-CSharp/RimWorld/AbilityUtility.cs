using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135F RID: 4959
	public static class AbilityUtility
	{
		// Token: 0x06006BDB RID: 27611 RVA: 0x000495D1 File Offset: 0x000477D1
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

		// Token: 0x06006BDC RID: 27612 RVA: 0x00049611 File Offset: 0x00047811
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

		// Token: 0x06006BDD RID: 27613 RVA: 0x00049647 File Offset: 0x00047847
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

		// Token: 0x06006BDE RID: 27614 RVA: 0x0004967D File Offset: 0x0004787D
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

		// Token: 0x06006BDF RID: 27615 RVA: 0x00213080 File Offset: 0x00211280
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

		// Token: 0x06006BE0 RID: 27616 RVA: 0x000496B3 File Offset: 0x000478B3
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

		// Token: 0x06006BE1 RID: 27617 RVA: 0x000496E9 File Offset: 0x000478E9
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

		// Token: 0x06006BE2 RID: 27618 RVA: 0x00049729 File Offset: 0x00047929
		public static void DoClamor(IntVec3 cell, float radius, Thing source, ClamorDef clamor)
		{
			if (clamor != null)
			{
				GenClamor.DoClamor(source, cell, radius, clamor);
			}
		}

		// Token: 0x06006BE3 RID: 27619 RVA: 0x002130D8 File Offset: 0x002112D8
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
