using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AC RID: 4268
	public class Apparel : ThingWithComps
	{
		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x060065DC RID: 26076 RVA: 0x00226B98 File Offset: 0x00224D98
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker == null)
				{
					return null;
				}
				return pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x060065DD RID: 26077 RVA: 0x00226BBC File Offset: 0x00224DBC
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x060065DE RID: 26078 RVA: 0x00226BC4 File Offset: 0x00224DC4
		public string WornGraphicPath
		{
			get
			{
				if (base.StyleDef != null && !base.StyleDef.wornGraphicPath.NullOrEmpty())
				{
					return base.StyleDef.wornGraphicPath;
				}
				if (!this.def.apparel.wornGraphicPaths.NullOrEmpty<string>())
				{
					return this.def.apparel.wornGraphicPaths[this.thingIDNumber % this.def.apparel.wornGraphicPaths.Count];
				}
				return this.def.apparel.wornGraphicPath;
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x060065DF RID: 26079 RVA: 0x00226C50 File Offset: 0x00224E50
		public override string DescriptionDetailed
		{
			get
			{
				string text = base.DescriptionDetailed;
				if (this.WornByCorpse)
				{
					text += "\n" + "WasWornByCorpse".Translate();
				}
				return text;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x060065E0 RID: 26080 RVA: 0x00226C90 File Offset: 0x00224E90
		public override Color DrawColor
		{
			get
			{
				if (base.StyleDef != null && base.StyleDef.color != default(Color))
				{
					return base.StyleDef.color;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x060065E1 RID: 26081 RVA: 0x00226CD4 File Offset: 0x00224ED4
		// (set) Token: 0x060065E2 RID: 26082 RVA: 0x00226CFC File Offset: 0x00224EFC
		public Color? DesiredColor
		{
			get
			{
				CompColorable comp = base.GetComp<CompColorable>();
				if (comp != null)
				{
					return comp.DesiredColor;
				}
				return null;
			}
			set
			{
				CompColorable comp = base.GetComp<CompColorable>();
				if (comp != null)
				{
					comp.DesiredColor = value;
					return;
				}
				Log.Error("Tried setting Apparel.DesiredColor without having CompColorable comp!");
			}
		}

		// Token: 0x060065E3 RID: 26083 RVA: 0x00226D28 File Offset: 0x00224F28
		public override string GetInspectStringLowPriority()
		{
			string text = base.GetInspectStringLowPriority();
			if (base.StyleDef != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "VariantOf".Translate().CapitalizeFirst() + ": " + this.def.LabelCap;
			}
			return text;
		}

		// Token: 0x060065E4 RID: 26084 RVA: 0x00226D91 File Offset: 0x00224F91
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x060065E5 RID: 26085 RVA: 0x00226DAC File Offset: 0x00224FAC
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x060065E6 RID: 26086 RVA: 0x00226DB5 File Offset: 0x00224FB5
		public override void Notify_ColorChanged()
		{
			if (this.Wearer != null)
			{
				this.Wearer.apparel.Notify_ApparelChanged();
				return;
			}
			base.Notify_ColorChanged();
		}

		// Token: 0x060065E7 RID: 26087 RVA: 0x00226DD6 File Offset: 0x00224FD6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x060065E8 RID: 26088 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x060065EA RID: 26090 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowVerbCast(Verb verb)
		{
			return true;
		}

		// Token: 0x060065EB RID: 26091 RVA: 0x00226DF0 File Offset: 0x00224FF0
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			List<ThingComp> comps = base.AllComps;
			int num;
			for (int i = 0; i < comps.Count; i = num + 1)
			{
				ThingComp thingComp = comps[i];
				foreach (Gizmo gizmo in thingComp.CompGetWornGizmosExtra())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060065EC RID: 26092 RVA: 0x00226E00 File Offset: 0x00225000
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			RoyalTitleDef royalTitleDef = (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.requiredApparel != null && t.requiredApparel.Any((ApparelRequirement req) => req.ApparelMeetsRequirement(this.def, false))
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
			if (royalTitleDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Apparel_MaxSatisfiedTitle_Desc".Translate(), 2752, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(royalTitleDef, -1)
				}, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x060065ED RID: 26093 RVA: 0x00226E10 File Offset: 0x00225010
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.WornByCorpse)
			{
				if (text.Length > 0)
				{
					text += "\n";
				}
				text += "WasWornByCorpse".Translate();
			}
			return text;
		}

		// Token: 0x060065EE RID: 26094 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		// Token: 0x04003986 RID: 14726
		private bool wornByCorpseInt;
	}
}
