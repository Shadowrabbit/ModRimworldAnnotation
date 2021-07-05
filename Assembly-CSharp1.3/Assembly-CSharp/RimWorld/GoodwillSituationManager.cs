using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC2 RID: 3778
	public class GoodwillSituationManager
	{
		// Token: 0x0600592B RID: 22827 RVA: 0x001E6CEC File Offset: 0x001E4EEC
		public List<GoodwillSituationManager.CachedSituation> GetSituations(Faction other)
		{
			if (other == null || other.IsPlayer)
			{
				Log.Error("Called GetSituations() for faction " + other);
				return null;
			}
			List<GoodwillSituationManager.CachedSituation> result;
			if (this.cachedData.TryGetValue(other, out result))
			{
				return result;
			}
			this.Recalculate(other, true);
			return this.cachedData[other];
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x001E6D3C File Offset: 0x001E4F3C
		public int GetMaxGoodwill(Faction other)
		{
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			int num = 100;
			for (int i = 0; i < situations.Count; i++)
			{
				num = Mathf.Min(num, situations[i].maxGoodwill);
			}
			return num;
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x001E6D7C File Offset: 0x001E4F7C
		public int GetNaturalGoodwill(Faction other)
		{
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			int num = 0;
			for (int i = 0; i < situations.Count; i++)
			{
				num += situations[i].naturalGoodwillOffset;
			}
			return num;
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x001E6DB4 File Offset: 0x001E4FB4
		public string GetExplanation(Faction other)
		{
			if (other == null || other == Faction.OfPlayer)
			{
				Log.Error("Tried to get CachedGoodwillData explanation for faction " + other);
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			List<GoodwillSituationManager.CachedSituation> situations = this.GetSituations(other);
			for (int i = 0; i < situations.Count; i++)
			{
				stringBuilder.AppendInNewLine(situations[i].def.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x001E6E1F File Offset: 0x001E501F
		public void GoodwillManagerTick()
		{
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				this.RecalculateAll(true);
			}
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x001E6E3C File Offset: 0x001E503C
		public void RecalculateAll(bool canSendHostilityChangedLetter)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i] != Faction.OfPlayer && allFactionsListForReading[i].HasGoodwill)
				{
					this.Recalculate(allFactionsListForReading[i], canSendHostilityChangedLetter);
				}
			}
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x001E6E90 File Offset: 0x001E5090
		private void Recalculate(Faction other, bool canSendHostilityChangedLetter)
		{
			List<GoodwillSituationManager.CachedSituation> outSituations;
			if (this.cachedData.TryGetValue(other, out outSituations))
			{
				this.Recalculate(other, outSituations);
			}
			else
			{
				List<GoodwillSituationManager.CachedSituation> list = new List<GoodwillSituationManager.CachedSituation>();
				this.Recalculate(other, list);
				this.cachedData.Add(other, list);
			}
			this.CheckHostilityChanged(other, canSendHostilityChangedLetter);
		}

		// Token: 0x06005932 RID: 22834 RVA: 0x001E6EDC File Offset: 0x001E50DC
		private void Recalculate(Faction other, List<GoodwillSituationManager.CachedSituation> outSituations)
		{
			outSituations.Clear();
			if (!other.HasGoodwill)
			{
				return;
			}
			List<GoodwillSituationDef> allDefsListForReading = DefDatabase<GoodwillSituationDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				int maxGoodwill = allDefsListForReading[i].Worker.GetMaxGoodwill(other);
				int naturalGoodwillOffset = allDefsListForReading[i].Worker.GetNaturalGoodwillOffset(other);
				if (maxGoodwill < 100 || naturalGoodwillOffset != 0)
				{
					outSituations.Add(new GoodwillSituationManager.CachedSituation
					{
						def = allDefsListForReading[i],
						maxGoodwill = maxGoodwill,
						naturalGoodwillOffset = naturalGoodwillOffset
					});
				}
			}
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x001E6F6C File Offset: 0x001E516C
		private void CheckHostilityChanged(Faction other, bool canSendHostilityChangedLetter)
		{
			if (Current.ProgramState == ProgramState.Entry)
			{
				return;
			}
			if (!other.HasGoodwill)
			{
				return;
			}
			Faction.OfPlayer.Notify_GoodwillSituationsChanged(other, canSendHostilityChangedLetter, null, null);
		}

		// Token: 0x0400345E RID: 13406
		private Dictionary<Faction, List<GoodwillSituationManager.CachedSituation>> cachedData = new Dictionary<Faction, List<GoodwillSituationManager.CachedSituation>>();

		// Token: 0x0400345F RID: 13407
		private const int RecacheEveryTicks = 1000;

		// Token: 0x0200231D RID: 8989
		public struct CachedSituation
		{
			// Token: 0x040085ED RID: 34285
			public GoodwillSituationDef def;

			// Token: 0x040085EE RID: 34286
			public int maxGoodwill;

			// Token: 0x040085EF RID: 34287
			public int naturalGoodwillOffset;
		}
	}
}
