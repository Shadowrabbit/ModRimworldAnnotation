using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001649 RID: 5705
	public class Tale : IExposable, ILoadReferenceable
	{
		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06007C0A RID: 31754 RVA: 0x00053467 File Offset: 0x00051667
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06007C0B RID: 31755 RVA: 0x0005347A File Offset: 0x0005167A
		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06007C0C RID: 31756 RVA: 0x00053482 File Offset: 0x00051682
		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x06007C0D RID: 31757 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x06007C0E RID: 31758 RVA: 0x002532B0 File Offset: 0x002514B0
		public float InterestLevel
		{
			get
			{
				float num = this.def.baseInterest;
				num /= (float)(1 + this.uses * 3);
				float a = 0f;
				switch (this.def.type)
				{
				case TaleType.Volatile:
					a = 50f;
					break;
				case TaleType.Expirable:
					a = this.def.expireDays;
					break;
				case TaleType.PermanentHistorical:
					a = 50f;
					break;
				}
				float value = (float)(this.AgeTicks / 60000);
				num *= Mathf.InverseLerp(a, 0f, value);
				if (num < 0.01f)
				{
					num = 0.01f;
				}
				return num;
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x06007C0F RID: 31759 RVA: 0x0005348D File Offset: 0x0005168D
		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x06007C10 RID: 31760 RVA: 0x000534C3 File Offset: 0x000516C3
		public virtual string ShortSummary
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel.CapitalizeFirst();
				}
				return this.def.LabelCap;
			}
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x000534FD File Offset: 0x000516FD
		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.", false);
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		// Token: 0x06007C13 RID: 31763 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		// Token: 0x06007C14 RID: 31764 RVA: 0x00253348 File Offset: 0x00251548
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
		}

		// Token: 0x06007C15 RID: 31765 RVA: 0x00053536 File Offset: 0x00051736
		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x00053546 File Offset: 0x00051746
		public void Notify_ReferenceDestroyed()
		{
			if (this.uses == 0)
			{
				Log.Warning("Called reference destroyed method on tale " + this + " but uses count is 0.", false);
				return;
			}
			this.uses--;
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x00053575 File Offset: 0x00051775
		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			if (this.def.rulePack != null)
			{
				yield return this.def.rulePack;
			}
			yield break;
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x00053585 File Offset: 0x00051785
		public IEnumerable<Rule> GetTextGenerationRules()
		{
			Vector2 location = Vector2.zero;
			if (this.surroundings != null && this.surroundings.tile >= 0)
			{
				location = Find.WorldGrid.LongLatOf(this.surroundings.tile);
			}
			yield return new Rule_String("DATE", GenDate.DateFullStringAt((long)this.date, location));
			IEnumerator<Rule> enumerator;
			if (this.surroundings != null)
			{
				foreach (Rule rule in this.surroundings.GetRules())
				{
					yield return rule;
				}
				enumerator = null;
			}
			foreach (Rule rule2 in this.SpecialTextGenerationRules())
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007C1A RID: 31770 RVA: 0x00053595 File Offset: 0x00051795
		protected virtual IEnumerable<Rule> SpecialTextGenerationRules()
		{
			yield break;
		}

		// Token: 0x06007C1B RID: 31771 RVA: 0x0005359E File Offset: 0x0005179E
		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id;
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x000535B5 File Offset: 0x000517B5
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x002533C4 File Offset: 0x002515C4
		public override string ToString()
		{
			string str = string.Concat(new object[]
			{
				"(#",
				this.id,
				": ",
				this.ShortSummary,
				"(age=",
				((float)this.AgeTicks / 60000f).ToString("F2"),
				" interest=",
				this.InterestLevel
			});
			if (this.Unused && this.def.type == TaleType.Expirable)
			{
				str = str + ", expireDays=" + this.def.expireDays.ToString("F2");
			}
			return str + ")";
		}

		// Token: 0x04005145 RID: 20805
		public TaleDef def;

		// Token: 0x04005146 RID: 20806
		public int id;

		// Token: 0x04005147 RID: 20807
		private int uses;

		// Token: 0x04005148 RID: 20808
		public int date = -1;

		// Token: 0x04005149 RID: 20809
		public TaleData_Surroundings surroundings;

		// Token: 0x0400514A RID: 20810
		public string customLabel;
	}
}
