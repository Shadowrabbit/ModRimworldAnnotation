using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C3 RID: 5315
	public class StatPart_ApparelStatOffset : StatPart
	{
		// Token: 0x06007EE2 RID: 32482 RVA: 0x002CEB40 File Offset: 0x002CCD40
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

		// Token: 0x06007EE3 RID: 32483 RVA: 0x002CEC54 File Offset: 0x002CCE54
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
							if (!Mathf.Approximately(this.hideAtValue, this.GetStatValueForGear(gear)))
							{
								stringBuilder.AppendLine(this.InfoTextLineFrom(gear));
							}
						}
					}
					if (this.includeWeapon && pawn.equipment != null && pawn.equipment.Primary != null && !Mathf.Approximately(this.hideAtValue, this.GetStatValueForGear(pawn.equipment.Primary)))
					{
						stringBuilder.AppendLine(this.InfoTextLineFrom(pawn.equipment.Primary));
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}

		// Token: 0x06007EE4 RID: 32484 RVA: 0x002CED68 File Offset: 0x002CCF68
		private string InfoTextLineFrom(Thing gear)
		{
			float num = this.GetStatValueForGear(gear);
			if (this.subtract)
			{
				num = -num;
			}
			return "    " + gear.LabelCap + ": " + num.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x06007EE5 RID: 32485 RVA: 0x002CEDAF File Offset: 0x002CCFAF
		private float GetStatValueForGear(Thing gear)
		{
			return gear.GetStatValue(this.apparelStat, true) + StatWorker.StatOffsetFromGear(gear, this.apparelStat);
		}

		// Token: 0x06007EE6 RID: 32486 RVA: 0x002CEDCC File Offset: 0x002CCFCC
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

		// Token: 0x06007EE7 RID: 32487 RVA: 0x002CEE76 File Offset: 0x002CD076
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

		// Token: 0x04004F60 RID: 20320
		private StatDef apparelStat;

		// Token: 0x04004F61 RID: 20321
		private bool subtract;

		// Token: 0x04004F62 RID: 20322
		private bool includeWeapon;

		// Token: 0x04004F63 RID: 20323
		private float hideAtValue = float.MinValue;
	}
}
