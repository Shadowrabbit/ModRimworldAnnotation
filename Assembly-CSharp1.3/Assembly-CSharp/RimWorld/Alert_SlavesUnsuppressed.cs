using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001266 RID: 4710
	public class Alert_SlavesUnsuppressed : Alert
	{
		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x060070CC RID: 28876 RVA: 0x002595FC File Offset: 0x002577FC
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
					{
						if (!pawn.Suspended && pawn.IsSlave)
						{
							Need_Suppression need_Suppression = pawn.needs.TryGetNeed<Need_Suppression>();
							if (need_Suppression != null && need_Suppression.IsHigh)
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060070CD RID: 28877 RVA: 0x002596B4 File Offset: 0x002578B4
		public Alert_SlavesUnsuppressed()
		{
			this.defaultLabel = "SlavesUnsuppressedLabel".Translate();
			this.defaultExplanation = "SlavesUnsuppressedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060070CE RID: 28878 RVA: 0x00259703 File Offset: 0x00257903
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060070CF RID: 28879 RVA: 0x00259710 File Offset: 0x00257910
		public override TaggedString GetExplanation()
		{
			Pawn value = this.Targets[0];
			return "SlavesUnsuppressedDesc".Translate(value);
		}

		// Token: 0x04003E26 RID: 15910
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
