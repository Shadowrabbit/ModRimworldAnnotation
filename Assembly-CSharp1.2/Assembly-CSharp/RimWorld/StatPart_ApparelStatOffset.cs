using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2A RID: 7466
	public class StatPart_ApparelStatOffset : StatPart
	{
		// Token: 0x0600A250 RID: 41552 RVA: 0x002F4F00 File Offset: 0x002F3100
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					if (pawn.apparel != null)
					{
						for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
						{
							float num = pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true);
							num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[i], this.apparelStat);
							if (this.subtract)
							{
								val -= num;
							}
							else
							{
								val += num;
							}
						}
					}
					if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
					{
						float num2 = pawn.equipment.Primary.GetStatValue(this.apparelStat, true);
						num2 += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.apparelStat);
						if (this.subtract)
						{
							val -= num2;
							return;
						}
						val += num2;
					}
				}
			}
		}

		// Token: 0x0600A251 RID: 41553 RVA: 0x002F5014 File Offset: 0x002F3214
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.PawnWearingRelevantGear(pawn))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
					if (pawn.apparel != null)
					{
						for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
						{
							Apparel gear = pawn.apparel.WornApparel[i];
							stringBuilder.AppendLine(this.InfoTextLineFrom(gear));
						}
					}
					if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null)
					{
						stringBuilder.AppendLine(this.InfoTextLineFrom(pawn.equipment.Primary));
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}

		// Token: 0x0600A252 RID: 41554 RVA: 0x002F50F4 File Offset: 0x002F32F4
		private string InfoTextLineFrom(Thing gear)
		{
			float num = gear.GetStatValue(this.apparelStat, true);
			num += StatWorker.StatOffsetFromGear(gear, this.apparelStat);
			if (this.subtract)
			{
				num = -num;
			}
			return "    " + gear.LabelCap + ": " + num.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x0600A253 RID: 41555 RVA: 0x002F5150 File Offset: 0x002F3350
		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = pawn.apparel.WornApparel[i];
					if (apparel.GetStatValue(this.apparelStat, true) != 0f)
					{
						return true;
					}
					if (StatWorker.StatOffsetFromGear(apparel, this.apparelStat) != 0f)
					{
						return true;
					}
				}
			}
			return this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.apparelStat) != 0f;
		}

		// Token: 0x0600A254 RID: 41556 RVA: 0x0006BD95 File Offset: 0x00069F95
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn.apparel != null)
			{
				int num;
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i = num + 1)
				{
					Apparel thing = pawn.apparel.WornApparel[i];
					if (Mathf.Abs(thing.GetStatValue(this.apparelStat, true)) > 0f)
					{
						yield return new Dialog_InfoCard.Hyperlink(thing, -1);
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04006E5C RID: 28252
		private StatDef apparelStat;

		// Token: 0x04006E5D RID: 28253
		private bool subtract;

		// Token: 0x04006E5E RID: 28254
		private bool includeWeapon;
	}
}
