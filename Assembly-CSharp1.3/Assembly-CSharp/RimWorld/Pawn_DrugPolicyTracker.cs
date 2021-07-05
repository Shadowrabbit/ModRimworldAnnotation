using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2E RID: 3630
	public class Pawn_DrugPolicyTracker : IExposable
	{
		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x060053ED RID: 21485 RVA: 0x001C657B File Offset: 0x001C477B
		// (set) Token: 0x060053EE RID: 21486 RVA: 0x001C65A0 File Offset: 0x001C47A0
		public DrugPolicy CurrentPolicy
		{
			get
			{
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.drugPolicyDatabase.DefaultDrugPolicy();
				}
				return this.curPolicy;
			}
			set
			{
				if (this.curPolicy == value)
				{
					return;
				}
				this.curPolicy = value;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060053EF RID: 21487 RVA: 0x001C65B4 File Offset: 0x001C47B4
		private float DayPercentNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(this.pawn));
				}
				if (this.pawn.timetable == null)
				{
					return GenLocalDate.DayPercent(this.pawn);
				}
				float hoursPerDayNotSleeping = this.HoursPerDayNotSleeping;
				if (hoursPerDayNotSleeping == 0f)
				{
					return 1f;
				}
				float num = 0f;
				int num2 = GenLocalDate.HourOfDay(this.pawn);
				for (int i = 0; i < num2; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num += 1f;
					}
				}
				if (this.pawn.timetable.CurrentAssignment != TimeAssignmentDefOf.Sleep)
				{
					float num3 = (float)(Find.TickManager.TicksAbs % 2500) / 2500f;
					num += num3;
				}
				return num / hoursPerDayNotSleeping;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060053F0 RID: 21488 RVA: 0x001C6694 File Offset: 0x001C4894
		private float HoursPerDayNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return 16f;
				}
				int num = 0;
				for (int i = 0; i < 24; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num++;
					}
				}
				return (float)num;
			}
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x001C66E6 File Offset: 0x001C48E6
		public Pawn_DrugPolicyTracker()
		{
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x001C66F9 File Offset: 0x001C48F9
		public Pawn_DrugPolicyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x001C6714 File Offset: 0x001C4914
		public void ExposeData()
		{
			Scribe_References.Look<DrugPolicy>(ref this.curPolicy, "curAssignedDrugs", false);
			Scribe_Collections.Look<DrugTakeRecord>(ref this.drugTakeRecords, "drugTakeRecords", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.drugTakeRecords.RemoveAll((DrugTakeRecord x) => x.drug == null) != 0)
				{
					Log.ErrorOnce("Removed some null drugs from drug policy tracker", 816929737);
				}
			}
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x001C678C File Offset: 0x001C498C
		public bool HasEverTaken(ThingDef drug)
		{
			if (!drug.IsDrug)
			{
				Log.Warning(drug + " is not a drug.");
				return false;
			}
			return this.drugTakeRecords.Any((DrugTakeRecord x) => x.drug == drug);
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x001C67E4 File Offset: 0x001C49E4
		public bool AllowedToTakeToInventory(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef);
				return false;
			}
			if (thingDef.IsNonMedicalDrug && this.pawn.IsTeetotaler())
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			return !drugPolicyEntry.allowScheduled && drugPolicyEntry.takeToInventory > 0 && !this.pawn.inventory.innerContainer.Contains(thingDef);
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x001C6878 File Offset: 0x001C4A78
		public bool AllowedToTakeScheduledEver(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef);
				return false;
			}
			return this.CurrentPolicy[thingDef].allowScheduled && (!thingDef.IsNonMedicalDrug || !this.pawn.IsTeetotaler()) && (thingDef.ingestible.drugCategory != DrugCategory.Hard || new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, this.pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo());
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x001C6918 File Offset: 0x001C4B18
		public bool AllowedToTakeScheduledNow(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef);
				return false;
			}
			if (!this.AllowedToTakeScheduledEver(thingDef))
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			if (drugPolicyEntry.onlyIfMoodBelow < 1f && this.pawn.needs.mood != null && this.pawn.needs.mood.CurLevelPercentage >= drugPolicyEntry.onlyIfMoodBelow)
			{
				return false;
			}
			if (drugPolicyEntry.onlyIfJoyBelow < 1f && this.pawn.needs.joy != null && this.pawn.needs.joy.CurLevelPercentage >= drugPolicyEntry.onlyIfJoyBelow)
			{
				return false;
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == thingDef);
			if (drugTakeRecord != null)
			{
				if (drugPolicyEntry.daysFrequency < 1f)
				{
					int num = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
					if (drugTakeRecord.TimesTakenThisDay >= num)
					{
						return false;
					}
				}
				else
				{
					int num2 = Mathf.Abs(GenDate.DaysPassed - drugTakeRecord.LastTakenDays);
					int num3 = Mathf.RoundToInt(drugPolicyEntry.daysFrequency);
					if (num2 < num3)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x001C6A84 File Offset: 0x001C4C84
		public bool ShouldTryToTakeScheduledNow(ThingDef ingestible)
		{
			if (!ingestible.IsDrug)
			{
				return false;
			}
			if (!this.AllowedToTakeScheduledNow(ingestible))
			{
				return false;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.5f && this.CanCauseOverdose(ingestible))
			{
				int num = this.LastTicksWhenTakenDrugWhichCanCauseOverdose();
				if (Find.TickManager.TicksGame - num < 1250)
				{
					return false;
				}
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == ingestible);
			if (drugTakeRecord == null)
			{
				return true;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[ingestible];
			if (drugPolicyEntry.daysFrequency < 1f)
			{
				int num2 = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
				float num3 = 1f / (float)(num2 + 1);
				int num4 = 0;
				float dayPercentNotSleeping = this.DayPercentNotSleeping;
				for (int i = 0; i < num2; i++)
				{
					if (dayPercentNotSleeping > (float)(i + 1) * num3 - num3 * 0.5f)
					{
						num4++;
					}
				}
				return drugTakeRecord.TimesTakenThisDay < num4 && (drugTakeRecord.TimesTakenThisDay == 0 || (float)(Find.TickManager.TicksGame - drugTakeRecord.lastTakenTicks) / (this.HoursPerDayNotSleeping * 2500f) >= 0.6f * num3);
			}
			float dayPercentNotSleeping2 = this.DayPercentNotSleeping;
			Rand.PushState();
			Rand.Seed = Gen.HashCombineInt(GenDate.DaysPassed, this.pawn.thingIDNumber);
			bool result = dayPercentNotSleeping2 >= Rand.Range(0.1f, 0.35f);
			Rand.PopState();
			return result;
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x001C6C2C File Offset: 0x001C4E2C
		public void Notify_DrugIngested(Thing drug)
		{
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == drug.def);
			if (drugTakeRecord == null)
			{
				drugTakeRecord = new DrugTakeRecord();
				drugTakeRecord.drug = drug.def;
				this.drugTakeRecords.Add(drugTakeRecord);
			}
			drugTakeRecord.lastTakenTicks = Find.TickManager.TicksGame;
			DrugTakeRecord drugTakeRecord2 = drugTakeRecord;
			int timesTakenThisDay = drugTakeRecord2.TimesTakenThisDay;
			drugTakeRecord2.TimesTakenThisDay = timesTakenThisDay + 1;
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x001C6CA4 File Offset: 0x001C4EA4
		private int LastTicksWhenTakenDrugWhichCanCauseOverdose()
		{
			int num = -999999;
			for (int i = 0; i < this.drugTakeRecords.Count; i++)
			{
				if (this.CanCauseOverdose(this.drugTakeRecords[i].drug))
				{
					num = Mathf.Max(num, this.drugTakeRecords[i].lastTakenTicks);
				}
			}
			return num;
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x001C6D00 File Offset: 0x001C4F00
		private bool CanCauseOverdose(ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			return compProperties != null && compProperties.CanCauseOverdose;
		}

		// Token: 0x04003168 RID: 12648
		public Pawn pawn;

		// Token: 0x04003169 RID: 12649
		private DrugPolicy curPolicy;

		// Token: 0x0400316A RID: 12650
		private List<DrugTakeRecord> drugTakeRecords = new List<DrugTakeRecord>();

		// Token: 0x0400316B RID: 12651
		private const float DangerousDrugOverdoseSeverity = 0.5f;
	}
}
