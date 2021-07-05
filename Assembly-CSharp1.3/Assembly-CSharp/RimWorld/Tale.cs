using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001030 RID: 4144
	public class Tale : IExposable, ILoadReferenceable
	{
		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x060061CF RID: 25039 RVA: 0x00214258 File Offset: 0x00212458
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x060061D0 RID: 25040 RVA: 0x0021426B File Offset: 0x0021246B
		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x060061D1 RID: 25041 RVA: 0x00214273 File Offset: 0x00212473
		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x060061D2 RID: 25042 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x060061D3 RID: 25043 RVA: 0x00214280 File Offset: 0x00212480
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

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x060061D4 RID: 25044 RVA: 0x00214315 File Offset: 0x00212515
		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x060061D5 RID: 25045 RVA: 0x0021434B File Offset: 0x0021254B
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

		// Token: 0x060061D7 RID: 25047 RVA: 0x00214385 File Offset: 0x00212585
		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.");
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		// Token: 0x060061D8 RID: 25048 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x002143C0 File Offset: 0x002125C0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x0021443A File Offset: 0x0021263A
		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x0021444A File Offset: 0x0021264A
		public void Notify_ReferenceDestroyed()
		{
			if (this.uses == 0)
			{
				Log.Warning("Called reference destroyed method on tale " + this + " but uses count is 0.");
				return;
			}
			this.uses--;
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x00214478 File Offset: 0x00212678
		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			if (this.def.rulePack != null)
			{
				yield return this.def.rulePack;
			}
			yield break;
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x00214488 File Offset: 0x00212688
		public IEnumerable<Rule> GetTextGenerationRules(Dictionary<string, string> outConstants = null)
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
				foreach (Rule rule in this.surroundings.GetRules(null))
				{
					yield return rule;
				}
				enumerator = null;
			}
			foreach (Rule rule2 in this.SpecialTextGenerationRules(outConstants))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x0021449F File Offset: 0x0021269F
		protected virtual IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			yield break;
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x002144A8 File Offset: 0x002126A8
		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id;
		}

		// Token: 0x060061E1 RID: 25057 RVA: 0x002144BF File Offset: 0x002126BF
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x060061E2 RID: 25058 RVA: 0x002144C8 File Offset: 0x002126C8
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

		// Token: 0x040037C8 RID: 14280
		public TaleDef def;

		// Token: 0x040037C9 RID: 14281
		public int id;

		// Token: 0x040037CA RID: 14282
		private int uses;

		// Token: 0x040037CB RID: 14283
		public int date = -1;

		// Token: 0x040037CC RID: 14284
		public TaleData_Surroundings surroundings;

		// Token: 0x040037CD RID: 14285
		public string customLabel;
	}
}
