using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001704 RID: 5892
	public class Apparel : ThingWithComps
	{
		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x0600819E RID: 33182 RVA: 0x0026776C File Offset: 0x0026596C
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

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x0600819F RID: 33183 RVA: 0x00057107 File Offset: 0x00055307
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x060081A0 RID: 33184 RVA: 0x00267790 File Offset: 0x00265990
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

		// Token: 0x060081A1 RID: 33185 RVA: 0x0005710F File Offset: 0x0005530F
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x060081A2 RID: 33186 RVA: 0x0005712A File Offset: 0x0005532A
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x060081A3 RID: 33187 RVA: 0x00057133 File Offset: 0x00055333
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x060081A4 RID: 33188 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x060081A5 RID: 33189 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x060081A6 RID: 33190 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x060081A7 RID: 33191 RVA: 0x0005714D File Offset: 0x0005534D
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

		// Token: 0x060081A8 RID: 33192 RVA: 0x0005715D File Offset: 0x0005535D
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			RoyalTitleDef royalTitleDef = (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.requiredApparel != null && t.requiredApparel.Any((RoyalTitleDef.ApparelRequirement req) => req.ApparelMeetsRequirement(this.def, false))
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

		// Token: 0x060081A9 RID: 33193 RVA: 0x002677D0 File Offset: 0x002659D0
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

		// Token: 0x060081AA RID: 33194 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		// Token: 0x0400541F RID: 21535
		private bool wornByCorpseInt;
	}
}
