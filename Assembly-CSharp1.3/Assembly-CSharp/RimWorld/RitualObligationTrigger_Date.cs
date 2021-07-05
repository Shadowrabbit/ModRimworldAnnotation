using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F51 RID: 3921
	public class RitualObligationTrigger_Date : RitualObligationTrigger
	{
		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06005D0F RID: 23823 RVA: 0x001FF46C File Offset: 0x001FD66C
		public string DateString
		{
			get
			{
				Map map = (Current.ProgramState == ProgramState.Playing) ? Find.AnyPlayerHomeMap : null;
				return GenDate.QuadrumDateStringAt((long)(this.triggerDaysSinceStartOfYear * 60000), (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f);
			}
		}

		// Token: 0x06005D10 RID: 23824 RVA: 0x001FF4BB File Offset: 0x001FD6BB
		public override void Init(RitualObligationTriggerProperties props)
		{
			base.Init(props);
			this.triggerDaysSinceStartOfYear = this.RandomDate();
		}

		// Token: 0x06005D11 RID: 23825 RVA: 0x001FF4D0 File Offset: 0x001FD6D0
		public int RandomDate()
		{
			List<int> list = new List<int>();
			foreach (Precept precept in this.ritual.ideo.PreceptsListForReading)
			{
				Precept_Ritual precept_Ritual;
				if (precept != this.ritual && (precept_Ritual = (precept as Precept_Ritual)) != null)
				{
					using (List<RitualObligationTrigger>.Enumerator enumerator2 = precept_Ritual.obligationTriggers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							RitualObligationTrigger_Date ritualObligationTrigger_Date;
							if ((ritualObligationTrigger_Date = (enumerator2.Current as RitualObligationTrigger_Date)) != null)
							{
								list.Add(ritualObligationTrigger_Date.triggerDaysSinceStartOfYear);
							}
						}
					}
				}
			}
			List<int> source = Enumerable.Range(0, 60).Except(list).ToList<int>();
			int num = 20;
			bool flag = false;
			int num2 = 0;
			while (num >= 5 && !flag)
			{
				for (int i = 0; i < 10; i++)
				{
					num2 = source.RandomElement<int>();
					bool flag2 = false;
					for (int j = 0; j < list.Count; j++)
					{
						int num3 = list[j];
						int num4 = Mathf.Abs(num2 - num3);
						if (Mathf.Min(num4, 59 - num4) < num)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						flag = true;
						break;
					}
				}
				num -= 5;
			}
			return num2;
		}

		// Token: 0x06005D12 RID: 23826 RVA: 0x001FF628 File Offset: 0x001FD828
		public override void Tick()
		{
			if (this.ritual.isAnytime)
			{
				return;
			}
			int num = this.CurrentTickRelative();
			int num2 = this.OccursOnTick();
			if ((!this.mustBePlayerIdeo || Faction.OfPlayer.ideos.Has(this.ritual.ideo)) && num == num2)
			{
				this.ritual.AddObligation(new RitualObligation(this.ritual, true));
			}
		}

		// Token: 0x06005D13 RID: 23827 RVA: 0x001FF690 File Offset: 0x001FD890
		public int CurrentTickRelative()
		{
			Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
			long num = GenDate.LocalTicksOffsetFromLongitude((anyPlayerHomeMap != null) ? Find.WorldGrid.LongLatOf(anyPlayerHomeMap.Tile).x : 0f);
			return (int)((long)(GenTicks.TicksAbs % 3600000) + num);
		}

		// Token: 0x06005D14 RID: 23828 RVA: 0x001FF6D7 File Offset: 0x001FD8D7
		public int OccursOnTick()
		{
			return (this.triggerDaysSinceStartOfYear - 1) * 60000;
		}

		// Token: 0x06005D15 RID: 23829 RVA: 0x001FF6E7 File Offset: 0x001FD8E7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.triggerDaysSinceStartOfYear, "triggerDaysSinceStartOfYear", 0, false);
		}

		// Token: 0x040035E1 RID: 13793
		public int triggerDaysSinceStartOfYear;
	}
}
