using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DE RID: 734
	public class ImmunityHandler : IExposable
	{
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x0007097B File Offset: 0x0006EB7B
		public List<ImmunityRecord> ImmunityListForReading
		{
			get
			{
				return this.immunityList;
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00070983 File Offset: 0x0006EB83
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0007099D File Offset: 0x0006EB9D
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000709B8 File Offset: 0x0006EBB8
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x000709D4 File Offset: 0x0006EBD4
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, out HediffDef immunityCause, BodyPartRecord part = null)
		{
			immunityCause = null;
			if (!this.pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo(diseaseDef, out hediff))
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

		// Token: 0x060013D7 RID: 5079 RVA: 0x00070ACC File Offset: 0x0006ECCC
		public float GetImmunity(HediffDef def, bool naturalImmunityOnly = false)
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
			if (!naturalImmunityOnly && this.AnyHediffMakesFullyImmuneTo(def, out hediff) && num < 0.65000004f)
			{
				num = 0.65000004f;
			}
			return num;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00070B34 File Offset: 0x0006ED34
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

		// Token: 0x060013D9 RID: 5081 RVA: 0x00070C50 File Offset: 0x0006EE50
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

		// Token: 0x060013DA RID: 5082 RVA: 0x00070CD8 File Offset: 0x0006EED8
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def, out Hediff sourceHediff)
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

		// Token: 0x060013DB RID: 5083 RVA: 0x00070D58 File Offset: 0x0006EF58
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

		// Token: 0x060013DC RID: 5084 RVA: 0x00070D98 File Offset: 0x0006EF98
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
			if (this.AnyHediffMakesFullyImmuneTo(def, out hediff) && (immunityRecord == null || immunityRecord.immunity < 0.65000004f))
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

		// Token: 0x060013DD RID: 5085 RVA: 0x00070E20 File Offset: 0x0006F020
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		// Token: 0x04000E80 RID: 3712
		public Pawn pawn;

		// Token: 0x04000E81 RID: 3713
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		// Token: 0x04000E82 RID: 3714
		private const float ForcedImmunityLevel = 0.65000004f;

		// Token: 0x04000E83 RID: 3715
		private static List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		// Token: 0x02001A02 RID: 6658
		public struct ImmunityInfo
		{
			// Token: 0x040063B9 RID: 25529
			public HediffDef immunity;

			// Token: 0x040063BA RID: 25530
			public HediffDef source;
		}
	}
}
