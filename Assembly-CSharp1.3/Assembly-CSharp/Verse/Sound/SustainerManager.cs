using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x0200057F RID: 1407
	public class SustainerManager
	{
		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x000F9EA0 File Offset: 0x000F80A0
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000F9EA8 File Offset: 0x000F80A8
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000F9EB6 File Offset: 0x000F80B6
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000F9EC8 File Offset: 0x000F80C8
		public bool SustainerExists(SoundDef def)
		{
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				if (this.allSustainers[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000F9F04 File Offset: 0x000F8104
		public void SustainerManagerUpdate()
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				this.allSustainers[i].SustainerUpdate();
			}
			this.UpdateAllSustainerScopes();
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000F9F40 File Offset: 0x000F8140
		public void UpdateAllSustainerScopes()
		{
			SustainerManager.playingPerDef.Clear();
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				Sustainer sustainer = this.allSustainers[i];
				if (!SustainerManager.playingPerDef.ContainsKey(sustainer.def))
				{
					List<Sustainer> list = SimplePool<List<Sustainer>>.Get();
					list.Add(sustainer);
					SustainerManager.playingPerDef.Add(sustainer.def, list);
				}
				else
				{
					SustainerManager.playingPerDef[sustainer.def].Add(sustainer);
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair in SustainerManager.playingPerDef)
			{
				SoundDef key = keyValuePair.Key;
				List<Sustainer> value = keyValuePair.Value;
				if (value.Count - key.maxVoices < 0)
				{
					for (int j = 0; j < value.Count; j++)
					{
						value[j].scopeFader.inScope = true;
					}
				}
				else
				{
					for (int k = 0; k < value.Count; k++)
					{
						value[k].scopeFader.inScope = false;
					}
					value.Sort(SustainerManager.SortSustainersByCameraDistanceCached);
					int num = 0;
					for (int l = 0; l < value.Count; l++)
					{
						value[l].scopeFader.inScope = true;
						num++;
						if (num >= key.maxVoices)
						{
							break;
						}
					}
					for (int m = 0; m < value.Count; m++)
					{
						if (!value[m].scopeFader.inScope)
						{
							value[m].scopeFader.inScopePercent = 0f;
						}
					}
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair2 in SustainerManager.playingPerDef)
			{
				keyValuePair2.Value.Clear();
				SimplePool<List<Sustainer>>.Return(keyValuePair2.Value);
			}
			SustainerManager.playingPerDef.Clear();
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000FA188 File Offset: 0x000F8388
		public void EndAllInMap(Map map)
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				if (this.allSustainers[i].info.Maker.Map == map)
				{
					this.allSustainers[i].End();
				}
			}
		}

		// Token: 0x0400199B RID: 6555
		private List<Sustainer> allSustainers = new List<Sustainer>();

		// Token: 0x0400199C RID: 6556
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();

		// Token: 0x0400199D RID: 6557
		private static readonly Comparison<Sustainer> SortSustainersByCameraDistanceCached = (Sustainer a, Sustainer b) => a.CameraDistanceSquared.CompareTo(b.CameraDistanceSquared);
	}
}
