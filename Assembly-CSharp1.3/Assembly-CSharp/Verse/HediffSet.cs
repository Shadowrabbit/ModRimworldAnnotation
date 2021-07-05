using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DD RID: 733
	public class HediffSet : IExposable
	{
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x0006F7BE File Offset: 0x0006D9BE
		public float PainTotal
		{
			get
			{
				if (this.cachedPain < 0f)
				{
					this.cachedPain = this.CalculatePain();
				}
				return this.cachedPain;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x0006F7DF File Offset: 0x0006D9DF
		public float BleedRateTotal
		{
			get
			{
				if (this.cachedBleedRate < 0f)
				{
					this.cachedBleedRate = this.CalculateBleedRate();
				}
				return this.cachedBleedRate;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0006F800 File Offset: 0x0006DA00
		public bool HasHead
		{
			get
			{
				if (this.cachedHasHead == null)
				{
					this.cachedHasHead = new bool?(this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head));
				}
				return this.cachedHasHead.Value;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x0006F85E File Offset: 0x0006DA5E
		public float HungerRateFactor
		{
			get
			{
				return this.GetHungerRateFactor(null);
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x0006F868 File Offset: 0x0006DA68
		public float RestFallFactor
		{
			get
			{
				float num = 1f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.restFallFactor;
					}
				}
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.restFallFactorOffset;
					}
				}
				return Mathf.Max(num, 0f);
			}
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0006F8F4 File Offset: 0x0006DAF4
		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0006F950 File Offset: 0x0006DB50
		public void ExposeData()
		{
			Scribe_Collections.Look<Hediff>(ref this.hediffs, "hediffs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.hediffs.RemoveAll((Hediff x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had some null hediffs.");
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.hediffs.RemoveAll((Hediff x) => x.def == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had some hediffs with null defs.");
				}
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					this.hediffs[i].pawn = this.pawn;
				}
				this.DirtyCache();
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0006FA4C File Offset: 0x0006DC4C
		public void AddDirect(Hediff hediff, DamageInfo? dinfo = null, DamageWorker.DamageResult damageResult = null)
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.");
				return;
			}
			if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff.Part))
			{
				Log.Error("Tried to add health diff to missing part " + hediff.Part);
				return;
			}
			hediff.ageTicks = 0;
			hediff.pawn = this.pawn;
			bool flag = false;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].TryMergeWith(hediff))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.hediffs.Add(hediff);
				hediff.PostAdd(dinfo);
				if (this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			bool flag2 = hediff is Hediff_MissingPart;
			if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0f && hediff.Part != this.pawn.RaceProps.body.corePart)
			{
				bool flag3 = this.HasDirectlyAddedPartFor(hediff.Part);
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = !flag3;
				hediff_MissingPart.lastInjury = hediff.def;
				this.pawn.health.AddHediff(hediff_MissingPart, hediff.Part, dinfo, null);
				if (damageResult != null)
				{
					damageResult.AddHediff(hediff_MissingPart);
				}
				if (flag3)
				{
					if (dinfo != null)
					{
						hediff_MissingPart.lastInjury = HealthUtility.GetHediffDefFromDamage(dinfo.Value.Def, this.pawn, hediff.Part);
					}
					else
					{
						hediff_MissingPart.lastInjury = null;
					}
				}
				flag2 = true;
			}
			this.DirtyCache();
			if (flag2 && this.pawn.apparel != null)
			{
				this.pawn.apparel.Notify_LostBodyPart();
			}
			if (hediff.def.causesNeed != null && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0006FCA0 File Offset: 0x0006DEA0
		public void DirtyCache()
		{
			this.CacheMissingPartsCommonAncestors();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			this.cachedPain = -1f;
			this.cachedBleedRate = -1f;
			this.cachedHasHead = null;
			this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			this.pawn.health.summaryHealth.Notify_HealthChanged();
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0006FD18 File Offset: 0x0006DF18
		public float GetHungerRateFactor(HediffDef ignore = null)
		{
			float num = 1f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def != ignore)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.hungerRateFactor;
					}
				}
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j].def != ignore)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.hungerRateFactorOffset;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0006FDCC File Offset: 0x0006DFCC
		public int GetHediffCount(HediffDef def)
		{
			int num = 0;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x0006FE0A File Offset: 0x0006E00A
		public IEnumerable<T> GetHediffs<T>() where T : Hediff
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				T t = this.hediffs[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0006FE1C File Offset: 0x0006E01C
		public Hediff GetFirstHediffOfDef(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0006FE78 File Offset: 0x0006E078
		public bool PartIsMissing(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0006FEC8 File Offset: 0x0006E0C8
		public float GetPartHealth(BodyPartRecord part)
		{
			if (part == null)
			{
				return 0f;
			}
			float num = part.def.GetMaxHealth(this.pawn);
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is Hediff_MissingPart && this.hediffs[i].Part == part)
				{
					return 0f;
				}
				if (this.hediffs[i].Part == part)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null)
					{
						num -= hediff_Injury.Severity;
					}
				}
			}
			num = Mathf.Max(num, 0f);
			if (!part.def.destroyableByDamage)
			{
				num = Mathf.Max(num, 1f);
			}
			return (float)Mathf.RoundToInt(num);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0006FF94 File Offset: 0x0006E194
		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0006FFFC File Offset: 0x0006E1FC
		public bool HasHediff(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0007004C File Offset: 0x0006E24C
		public bool HasHediff(HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && this.hediffs[i].Part == bodyPart && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000700B0 File Offset: 0x0006E2B0
		public IEnumerable<Verb> GetHediffsVerbs()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				HediffComp_VerbGiver hediffComp_VerbGiver = this.hediffs[i].TryGetComp<HediffComp_VerbGiver>();
				if (hediffComp_VerbGiver != null)
				{
					List<Verb> verbList = hediffComp_VerbGiver.VerbTracker.AllVerbs;
					for (int j = 0; j < verbList.Count; j = num + 1)
					{
						yield return verbList[j];
						num = j;
					}
					verbList = null;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000700C0 File Offset: 0x0006E2C0
		public IEnumerable<Hediff> GetHediffsTendable()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x000700D0 File Offset: 0x0006E2D0
		public bool HasTendableHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00070125 File Offset: 0x0006E325
		public IEnumerable<HediffComp> GetAllComps()
		{
			foreach (Hediff hediff in this.hediffs)
			{
				HediffWithComps hediffWithComps = hediff as HediffWithComps;
				if (hediffWithComps != null)
				{
					foreach (HediffComp hediffComp in hediffWithComps.comps)
					{
						yield return hediffComp;
					}
					List<HediffComp>.Enumerator enumerator2 = default(List<HediffComp>.Enumerator);
				}
			}
			List<Hediff>.Enumerator enumerator = default(List<Hediff>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00070135 File Offset: 0x0006E335
		public IEnumerable<Hediff_Injury> GetInjuriesTendable()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow(false))
				{
					yield return hediff_Injury;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00070148 File Offset: 0x0006E348
		public bool HasTendableInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0007018C File Offset: 0x0006E38C
		public bool HasNaturallyHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealNaturally())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x000701D0 File Offset: 0x0006E3D0
		public bool HasTendedAndHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00070220 File Offset: 0x0006E420
		public bool HasTemperatureInjury(TemperatureInjuryStage minStage)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((this.hediffs[i].def == HediffDefOf.Hypothermia || this.hediffs[i].def == HediffDefOf.Heatstroke) && this.hediffs[i].CurStageIndex >= (int)minStage)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0007028C File Offset: 0x0006E48C
		public IEnumerable<BodyPartRecord> GetInjuredParts()
		{
			return (from x in this.hediffs
			where x is Hediff_Injury
			select x.Part).Distinct<BodyPartRecord>();
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x000702EC File Offset: 0x0006E4EC
		public IEnumerable<BodyPartRecord> GetNaturallyHealingInjuredParts()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetInjuredParts())
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && this.hediffs[i].Part == bodyPartRecord && hediff_Injury.CanHealNaturally())
					{
						yield return bodyPartRecord;
						break;
					}
				}
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x000702FC File Offset: 0x0006E4FC
		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00070312 File Offset: 0x0006E512
		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartTagDef tag = null, BodyPartRecord partParent = null)
		{
			List<BodyPartRecord> allPartsList = this.pawn.def.race.body.AllParts;
			int num;
			for (int i = 0; i < allPartsList.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = allPartsList[i];
				if (!this.PartIsMissing(bodyPartRecord) && (height == BodyPartHeight.Undefined || bodyPartRecord.height == height) && (depth == BodyPartDepth.Undefined || bodyPartRecord.depth == depth) && (tag == null || bodyPartRecord.def.tags.Contains(tag)) && (partParent == null || bodyPartRecord.parent == partParent))
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00070340 File Offset: 0x0006E540
		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartRecord partParent = null)
		{
			IEnumerable<BodyPartRecord> notMissingParts;
			if (this.GetNotMissingParts(height, depth, null, partParent).Any<BodyPartRecord>())
			{
				notMissingParts = this.GetNotMissingParts(height, depth, null, partParent);
			}
			else
			{
				if (!this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent).Any<BodyPartRecord>())
				{
					return null;
				}
				notMissingParts = this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent);
			}
			BodyPartRecord result;
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef), out result))
			{
				return result;
			}
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs, out result))
			{
				return result;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x000703E8 File Offset: 0x0006E5E8
		public float GetCoverageOfNotMissingNaturalParts(BodyPartRecord part)
		{
			if (this.PartIsMissing(part))
			{
				return 0f;
			}
			if (this.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return 0f;
			}
			this.coverageRejectedPartsSet.Clear();
			List<Hediff_MissingPart> missingPartsCommonAncestors = this.GetMissingPartsCommonAncestors();
			for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
			{
				this.coverageRejectedPartsSet.Add(missingPartsCommonAncestors[i].Part);
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j] is Hediff_AddedPart)
				{
					this.coverageRejectedPartsSet.Add(this.hediffs[j].Part);
				}
			}
			float num = 0f;
			this.coveragePartsStack.Clear();
			this.coveragePartsStack.Push(part);
			while (this.coveragePartsStack.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = this.coveragePartsStack.Pop();
				num += bodyPartRecord.coverageAbs;
				for (int k = 0; k < bodyPartRecord.parts.Count; k++)
				{
					if (!this.coverageRejectedPartsSet.Contains(bodyPartRecord.parts[k]))
					{
						this.coveragePartsStack.Push(bodyPartRecord.parts[k]);
					}
				}
			}
			this.coveragePartsStack.Clear();
			this.coverageRejectedPartsSet.Clear();
			return num;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00070540 File Offset: 0x0006E740
		private void CacheMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.cachedMissingPartsCommonAncestors = new List<Hediff_MissingPart>();
			}
			else
			{
				this.cachedMissingPartsCommonAncestors.Clear();
			}
			this.missingPartsCommonAncestorsQueue.Clear();
			this.missingPartsCommonAncestorsQueue.Enqueue(this.pawn.def.race.body.corePart);
			while (this.missingPartsCommonAncestorsQueue.Count != 0)
			{
				BodyPartRecord node = this.missingPartsCommonAncestorsQueue.Dequeue();
				if (!this.PartOrAnyAncestorHasDirectlyAddedParts(node))
				{
					Hediff_MissingPart hediff_MissingPart = (from x in this.GetHediffs<Hediff_MissingPart>()
					where x.Part == node
					select x).FirstOrDefault<Hediff_MissingPart>();
					if (hediff_MissingPart != null)
					{
						this.cachedMissingPartsCommonAncestors.Add(hediff_MissingPart);
					}
					else
					{
						for (int i = 0; i < node.parts.Count; i++)
						{
							this.missingPartsCommonAncestorsQueue.Enqueue(node.parts[i]);
						}
					}
				}
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0007063C File Offset: 0x0006E83C
		public bool HasDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00070689 File Offset: 0x0006E889
		public bool PartOrAnyAncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return this.HasDirectlyAddedPartFor(part) || (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent));
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x000706AF File Offset: 0x0006E8AF
		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x000706CA File Offset: 0x0006E8CA
		public IEnumerable<Hediff> GetTendableNonInjuryNonMissingPartHediffs()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x000706DC File Offset: 0x0006E8DC
		public bool HasTendableNonInjuryNonMissingPartHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && !(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00070758 File Offset: 0x0006E958
		public bool HasImmunizableNotImmuneHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].Visible && this.hediffs[i].def.PossibleToDevelopImmunityNaturally() && !this.hediffs[i].FullyImmune())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x000707E4 File Offset: 0x0006E9E4
		public bool AnyHediffMakesSickThought
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].def.makesSickThought && this.hediffs[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00070838 File Offset: 0x0006EA38
		private float CalculateBleedRate()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.health.Dead)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				HediffStage curStage = hediff.CurStage;
				if (curStage != null)
				{
					num *= curStage.totalBleedFactor;
				}
				num2 += hediff.BleedRate;
			}
			return num2 * num / this.pawn.HealthScale;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x000708CC File Offset: 0x0006EACC
		private float CalculatePain()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.Dead)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				num += this.hediffs[i].PainOffset;
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				num *= this.hediffs[j].PainFactor;
			}
			return Mathf.Clamp(num, 0f, 1f);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00070968 File Offset: 0x0006EB68
		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}

		// Token: 0x04000E77 RID: 3703
		public Pawn pawn;

		// Token: 0x04000E78 RID: 3704
		public List<Hediff> hediffs = new List<Hediff>();

		// Token: 0x04000E79 RID: 3705
		private List<Hediff_MissingPart> cachedMissingPartsCommonAncestors;

		// Token: 0x04000E7A RID: 3706
		private float cachedPain = -1f;

		// Token: 0x04000E7B RID: 3707
		private float cachedBleedRate = -1f;

		// Token: 0x04000E7C RID: 3708
		private bool? cachedHasHead;

		// Token: 0x04000E7D RID: 3709
		private Stack<BodyPartRecord> coveragePartsStack = new Stack<BodyPartRecord>();

		// Token: 0x04000E7E RID: 3710
		private HashSet<BodyPartRecord> coverageRejectedPartsSet = new HashSet<BodyPartRecord>();

		// Token: 0x04000E7F RID: 3711
		private Queue<BodyPartRecord> missingPartsCommonAncestorsQueue = new Queue<BodyPartRecord>();
	}
}
