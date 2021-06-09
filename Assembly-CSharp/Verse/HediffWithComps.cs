using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000401 RID: 1025
	public class HediffWithComps : Hediff
	{
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x000E0790 File Offset: 0x000DE990
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

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x000E080C File Offset: 0x000DEA0C
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

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x000E0854 File Offset: 0x000DEA54
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

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060018F1 RID: 6385 RVA: 0x000E089C File Offset: 0x000DEA9C
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

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x000E0904 File Offset: 0x000DEB04
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

		// Token: 0x060018F3 RID: 6387 RVA: 0x000E094C File Offset: 0x000DEB4C
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

		// Token: 0x060018F4 RID: 6388 RVA: 0x000E0990 File Offset: 0x000DEB90
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

		// Token: 0x060018F5 RID: 6389 RVA: 0x000E09D4 File Offset: 0x000DEBD4
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

		// Token: 0x060018F6 RID: 6390 RVA: 0x000E0A34 File Offset: 0x000DEC34
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

		// Token: 0x060018F7 RID: 6391 RVA: 0x00017B32 File Offset: 0x00015D32
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public override void Tended(float quality, int batchPosition = 0)
		{
			this.Tended_NewTemp(quality, 1f, batchPosition);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x000E0A84 File Offset: 0x000DEC84
		public override void Tended_NewTemp(float quality, float maxQuality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended_NewTemp(quality, maxQuality, batchPosition);
			}
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x000E0ABC File Offset: 0x000DECBC
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

		// Token: 0x060018FA RID: 6394 RVA: 0x000E0B00 File Offset: 0x000DED00
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnDied();
			}
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000E0B3C File Offset: 0x000DED3C
		public override void Notify_PawnKilled()
		{
			base.Notify_PawnKilled();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnKilled();
			}
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x000E0B78 File Offset: 0x000DED78
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			}
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x000E0BB8 File Offset: 0x000DEDB8
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x000E0BF0 File Offset: 0x000DEDF0
		public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
			base.Notify_PawnUsedVerb(verb, target);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x000E0C30 File Offset: 0x000DEE30
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing src = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, src);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_EntropyGained(baseAmount, finalAmount, src);
			}
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x000E0C70 File Offset: 0x000DEE70
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			}
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x000E0CB0 File Offset: 0x000DEEB0
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
					Log.Error("Error in HediffComp.CompPostMake(): " + arg, false);
					this.comps.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x000E0D28 File Offset: 0x000DEF28
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
						Log.Error("Could not instantiate or initialize a HediffComp: " + arg, false);
						this.comps.Remove(hediffComp);
					}
				}
			}
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x000E0DF0 File Offset: 0x000DEFF0
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

		// Token: 0x040012BF RID: 4799
		public List<HediffComp> comps = new List<HediffComp>();
	}
}
