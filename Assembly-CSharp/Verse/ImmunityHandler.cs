using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000422 RID: 1058
	public class ImmunityHandler : IExposable
	{
		// Token: 0x060019C5 RID: 6597 RVA: 0x000180E6 File Offset: 0x000162E6
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00018100 File Offset: 0x00016300
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000E36E4 File Offset: 0x000E18E4
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000E3700 File Offset: 0x000E1900
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, out HediffDef immunityCause, BodyPartRecord part = null)
		{
			immunityCause = null;
			if (!this.pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo_NewTemp(diseaseDef, out hediff))
			{
				immunityCause = hediff.def;
				return 0f;
			}
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == diseaseDef && hediffs[i].Part == part)
				{
					return 0f;
				}
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				if (this.immunityList[j].hediffDef == diseaseDef)
				{
					immunityCause = this.immunityList[j].source;
					return Mathf.Lerp(1f, 0f, this.immunityList[j].immunity / 0.6f);
				}
			}
			return 1f;
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000E37F8 File Offset: 0x000E19F8
		public float GetImmunity(HediffDef def)
		{
			float num = 0f;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				ImmunityRecord immunityRecord = this.immunityList[i];
				if (immunityRecord.hediffDef == def)
				{
					num = immunityRecord.immunity;
					break;
				}
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo_NewTemp(def, out hediff) && num < 0.65000004f)
			{
				num = 0.65000004f;
			}
			return num;
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000E385C File Offset: 0x000E1A5C
		internal void ImmunityHandlerTick()
		{
			List<ImmunityHandler.ImmunityInfo> list = this.NeededImmunitiesNow();
			for (int i = 0; i < list.Count; i++)
			{
				this.TryAddImmunityRecord(list[i].immunity, list[i].source);
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				ImmunityRecord immunityRecord = this.immunityList[j];
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(immunityRecord.hediffDef, false);
				immunityRecord.ImmunityTick(this.pawn, firstHediffOfDef != null, firstHediffOfDef);
			}
			for (int k = this.immunityList.Count - 1; k >= 0; k--)
			{
				if (this.immunityList[k].immunity <= 0f)
				{
					bool flag = false;
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].immunity == this.immunityList[k].hediffDef)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.immunityList.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000E3978 File Offset: 0x000E1B78
		private List<ImmunityHandler.ImmunityInfo> NeededImmunitiesNow()
		{
			ImmunityHandler.tmpNeededImmunitiesNow.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff hediff = hediffs[i];
				if (hediff.def.PossibleToDevelopImmunityNaturally())
				{
					ImmunityHandler.tmpNeededImmunitiesNow.Add(new ImmunityHandler.ImmunityInfo
					{
						immunity = hediff.def,
						source = hediff.def
					});
				}
			}
			return ImmunityHandler.tmpNeededImmunitiesNow;
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000E3A00 File Offset: 0x000E1C00
		[Obsolete("Will be removed in a future update, use AnyHediffMakesFullyImmuneTo_NewTemp")]
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def)
		{
			Hediff hediff;
			return this.AnyHediffMakesFullyImmuneTo_NewTemp(def, out hediff);
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000E3A18 File Offset: 0x000E1C18
		private bool AnyHediffMakesFullyImmuneTo_NewTemp(HediffDef def, out Hediff sourceHediff)
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							sourceHediff = hediffs[i];
							return true;
						}
					}
				}
			}
			sourceHediff = null;
			return false;
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x000E3A98 File Offset: 0x000E1C98
		private void TryAddImmunityRecord(HediffDef def, HediffDef source)
		{
			if (def.CompProps<HediffCompProperties_Immunizable>() == null)
			{
				return;
			}
			if (this.ImmunityRecordExists(def))
			{
				return;
			}
			ImmunityRecord immunityRecord = new ImmunityRecord();
			immunityRecord.hediffDef = def;
			immunityRecord.source = source;
			this.immunityList.Add(immunityRecord);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x000E3AD8 File Offset: 0x000E1CD8
		public ImmunityRecord GetImmunityRecord(HediffDef def)
		{
			ImmunityRecord immunityRecord = null;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				if (this.immunityList[i].hediffDef == def)
				{
					immunityRecord = this.immunityList[i];
					break;
				}
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo_NewTemp(def, out hediff) && (immunityRecord == null || immunityRecord.immunity < 0.65000004f))
			{
				immunityRecord = new ImmunityRecord
				{
					immunity = 0.65000004f,
					hediffDef = def,
					source = hediff.def
				};
			}
			return immunityRecord;
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00018118 File Offset: 0x00016318
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		// Token: 0x04001334 RID: 4916
		public Pawn pawn;

		// Token: 0x04001335 RID: 4917
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		// Token: 0x04001336 RID: 4918
		private const float ForcedImmunityLevel = 0.65000004f;

		// Token: 0x04001337 RID: 4919
		private static List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		// Token: 0x02000423 RID: 1059
		public struct ImmunityInfo
		{
			// Token: 0x04001338 RID: 4920
			public HediffDef immunity;

			// Token: 0x04001339 RID: 4921
			public HediffDef source;
		}
	}
}
