using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C4 RID: 708
	public class HediffWithComps : Hediff
	{
		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x0006CADC File Offset: 0x0006ACDC
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string compLabelInBracketsExtra = this.comps[i].CompLabelInBracketsExtra;
						if (!compLabelInBracketsExtra.NullOrEmpty())
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(compLabelInBracketsExtra);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x0006CB58 File Offset: 0x0006AD58
		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
						{
							return true;
						}
					}
				}
				return base.ShouldRemove;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x0006CBA0 File Offset: 0x0006ADA0
		public override bool Visible
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompDisallowVisible())
						{
							return false;
						}
					}
				}
				return base.Visible;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x0006CBE8 File Offset: 0x0006ADE8
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string compTipStringExtra = this.comps[i].CompTipStringExtra;
						if (!compTipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine(compTipStringExtra);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x0006CC50 File Offset: 0x0006AE50
		public override TextureAndColor StateIcon
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					TextureAndColor compStateIcon = this.comps[i].CompStateIcon;
					if (compStateIcon.HasValue)
					{
						return compStateIcon;
					}
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0006CC95 File Offset: 0x0006AE95
		public override IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				IEnumerable<Gizmo> enumerable = this.comps[i].CompGetGizmos();
				if (enumerable != null)
				{
					foreach (Gizmo gizmo in enumerable)
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0006CCA8 File Offset: 0x0006AEA8
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostAdd(dinfo);
				}
			}
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0006CCEC File Offset: 0x0006AEEC
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostRemoved();
				}
			}
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0006CD30 File Offset: 0x0006AF30
		public override void PostTick()
		{
			base.PostTick();
			if (this.comps != null)
			{
				float num = 0f;
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostTick(ref num);
				}
				if (num != 0f)
				{
					this.Severity += num;
				}
			}
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0006CD90 File Offset: 0x0006AF90
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompExposeData();
				}
			}
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0006CDE0 File Offset: 0x0006AFE0
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, maxQuality, batchPosition);
			}
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0006CE18 File Offset: 0x0006B018
		public override bool TryMergeWith(Hediff other)
		{
			if (base.TryMergeWith(other))
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostMerged(other);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0006CE5C File Offset: 0x0006B05C
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				this.comps[i].Notify_PawnDied();
			}
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0006CE98 File Offset: 0x0006B098
		public override void Notify_PawnKilled()
		{
			base.Notify_PawnKilled();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnKilled();
			}
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0006CED4 File Offset: 0x0006B0D4
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			}
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0006CF14 File Offset: 0x0006B114
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0006CF4C File Offset: 0x0006B14C
		public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
			base.Notify_PawnUsedVerb(verb, target);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0006CF8C File Offset: 0x0006B18C
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing src = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, src);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_EntropyGained(baseAmount, finalAmount, src);
			}
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0006CFCC File Offset: 0x0006B1CC
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			}
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0006D00C File Offset: 0x0006B20C
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				try
				{
					this.comps[i].CompPostMake();
				}
				catch (Exception arg)
				{
					Log.Error("Error in HediffComp.CompPostMake(): " + arg);
					this.comps.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0006D080 File Offset: 0x0006B280
		private void InitializeComps()
		{
			if (this.def.comps != null)
			{
				this.comps = new List<HediffComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					HediffComp hediffComp = null;
					try
					{
						hediffComp = (HediffComp)Activator.CreateInstance(this.def.comps[i].compClass);
						hediffComp.props = this.def.comps[i];
						hediffComp.parent = this;
						this.comps.Add(hediffComp);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate or initialize a HediffComp: " + arg);
						this.comps.Remove(hediffComp);
					}
				}
			}
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0006D148 File Offset: 0x0006B348
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.DebugString());
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string str;
					if (this.comps[i].ToString().Contains('_'))
					{
						str = this.comps[i].ToString().Split(new char[]
						{
							'_'
						})[1];
					}
					else
					{
						str = this.comps[i].ToString();
					}
					stringBuilder.AppendLine("--" + str);
					string text = this.comps[i].CompDebugString();
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text.TrimEnd(Array.Empty<char>()).Indented("    "));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000E4F RID: 3663
		public List<HediffComp> comps = new List<HediffComp>();
	}
}
