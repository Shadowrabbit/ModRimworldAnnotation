using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CB RID: 5323
	public class Pawn_DrugPolicyTracker : IExposable
	{
		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x060072AB RID: 29355 RVA: 0x0004D1F7 File Offset: 0x0004B3F7
		// (set) Token: 0x060072AC RID: 29356 RVA: 0x0004D21C File Offset: 0x0004B41C
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

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x060072AD RID: 29357 RVA: 0x00230420 File Offset: 0x0022E620
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

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x060072AE RID: 29358 RVA: 0x00230500 File Offset: 0x0022E700
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

		// Token: 0x060072AF RID: 29359 RVA: 0x0004D22F File Offset: 0x0004B42F
		public Pawn_DrugPolicyTracker()
		{
		}

		// Token: 0x060072B0 RID: 29360 RVA: 0x0004D242 File Offset: 0x0004B442
		public Pawn_DrugPolicyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060072B1 RID: 29361 RVA: 0x00230554 File Offset: 0x0022E754
		public void ExposeData()
		{
			Scribe_References.Look<DrugPolicy>(ref this.curPolicy, "curAssignedDrugs", false);
			Scribe_Collections.Look<DrugTakeRecord>(ref this.drugTakeRecords, "drugTakeRecords", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.drugTakeRecords.RemoveAll((DrugTakeRecord x) => x.drug == null) != 0)
				{
					Log.ErrorOnce("Removed some null drugs from drug policy tracker", 816929737, false);
				}
			}
		}

		// Token: 0x060072B2 RID: 29362 RVA: 0x002305CC File Offset: 0x0022E7CC
		public bool HasEverTaken(ThingDef drug)
		{
			if (!drug.IsDrug)
			{
				Log.Warning(drug + " is not a drug.", false);
				return false;
			}
			return this.drugTakeRecords.Any((DrugTakeRecord x) => x.drug == drug);
		}

		// Token: 0x060072B3 RID: 29363 RVA: 0x00230624 File Offset: 0x0022E824
		public bool AllowedToTakeToInventory(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
				return false;
			}
			if (thingDef.IsNonMedicalDrug && this.pawn.IsTeetotaler())
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			return !drugPolicyEntry.allowScheduled && drugPolicyEntry.takeToInventory > 0 && !this.pawn.inventory.innerContainer.Contains(thingDef);
		}

		// Token: 0x060072B4 RID: 29364 RVA: 0x002306BC File Offset: 0x0022E8BC
		public bool AllowedToTakeScheduledEver(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
				return false;
			}
			return this.CurrentPolicy[thingDef].allowScheduled && (!thingDef.IsNonMedicalDrug || !this.pawn.IsTeetotaler());
		}

		// Token: 0x060072B5 RID: 29365 RVA: 0x0023072C File Offset: 0x0022E92C
		public bool AllowedToTakeScheduledNow(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
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

		// Token: 0x060072B6 RID: 29366 RVA: 0x00230898 File Offset: 0x0022EA98
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

		// Token: 0x060072B7 RID: 29367 RVA: 0x00230A40 File Offset: 0x0022EC40
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

		// Token: 0x060072B8 RID: 29368 RVA: 0x00230AB8 File Offset: 0x0022ECB8
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

		// Token: 0x060072B9 RID: 29369 RVA: 0x00230B14 File Offset: 0x0022ED14
		private bool CanCauseOverdose(ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			return compProperties != null && compProperties.CanCauseOverdose;
		}

		// Token: 0x04004B86 RID: 19334
		public Pawn pawn;

		// Token: 0x04004B87 RID: 19335
		private DrugPolicy curPolicy;

		// Token: 0x04004B88 RID: 19336
		private List<DrugTakeRecord> drugTakeRecords = new List<DrugTakeRecord>();

		// Token: 0x04004B89 RID: 19337
		private const float DangerousDrugOverdoseSeverity = 0.5f;
	}
}
