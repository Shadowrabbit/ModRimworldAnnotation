using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000ED6 RID: 3798
	public class IdeoManager : IExposable
	{
		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x060059FB RID: 23035 RVA: 0x001ED9E3 File Offset: 0x001EBBE3
		public List<Ideo> IdeosListForReading
		{
			get
			{
				return this.ideos;
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060059FC RID: 23036 RVA: 0x001ED9EC File Offset: 0x001EBBEC
		public IEnumerable<Ideo> IdeosInViewOrder
		{
			get
			{
				IEnumerable<Faction> factions = Find.FactionManager.AllFactionsInViewOrder;
				return this.IdeosListForReading.OrderBy(delegate(Ideo ideo)
				{
					int num = 0;
					int num2 = int.MaxValue;
					foreach (Faction faction in factions)
					{
						if (faction.ideos != null && faction.ideos.IsPrimary(ideo))
						{
							if (faction.IsPlayer)
							{
								return int.MinValue;
							}
							num2 = Mathf.Min(num2, num);
						}
						num++;
					}
					return num2;
				});
			}
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x001EDA26 File Offset: 0x001EBC26
		public bool Add(Ideo ideo)
		{
			if (ideo == null)
			{
				Log.Error("Tried to add a null ideoligion.");
				return false;
			}
			if (this.ideos.Contains(ideo))
			{
				Log.Error("Tried to add the same ideoligion twice.");
				return false;
			}
			this.ideos.Add(ideo);
			return true;
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x001EDA60 File Offset: 0x001EBC60
		public bool Remove(Ideo ideo)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.ideos != null && faction.ideos.AllIdeos.Contains(ideo))
				{
					Log.Error(string.Concat(new string[]
					{
						"Faction ",
						faction.Name,
						" contains ideo ",
						ideo.name,
						" which was removed!"
					}));
				}
			}
			if (this.ideos.Remove(ideo))
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					Pawn_IdeoTracker ideo2 = pawn.ideo;
					if (ideo2 != null)
					{
						ideo2.Notify_IdeoRemoved(ideo);
					}
				}
				Find.PlayLog.Notify_IdeoRemoved(ideo);
				return true;
			}
			return false;
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x001EDB68 File Offset: 0x001EBD68
		public List<LordJob_Ritual> GetActiveRituals(Map map)
		{
			this.activeRitualsTmp.Clear();
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_Ritual item;
				if ((item = (lords[i].LordJob as LordJob_Ritual)) != null)
				{
					this.activeRitualsTmp.Add(item);
				}
			}
			return this.activeRitualsTmp;
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x001EDBC4 File Offset: 0x001EBDC4
		public LordJob_Ritual GetActiveRitualOn(TargetInfo target)
		{
			foreach (LordJob_Ritual lordJob_Ritual in this.GetActiveRituals(target.Map))
			{
				if (lordJob_Ritual.selectedTarget == target)
				{
					return lordJob_Ritual;
				}
			}
			return null;
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x001EDC2C File Offset: 0x001EBE2C
		public void SortIdeos()
		{
			this.ideos.SortByDescending(delegate(Ideo x)
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				Ideo ideo;
				if (ofPlayerSilentFail == null)
				{
					ideo = null;
				}
				else
				{
					FactionIdeosTracker factionIdeosTracker = ofPlayerSilentFail.ideos;
					ideo = ((factionIdeosTracker != null) ? factionIdeosTracker.PrimaryIdeo : null);
				}
				if (x == ideo)
				{
					return 999;
				}
				int num = 0;
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int i = 0; i < allFactionsListForReading.Count; i++)
				{
					if (allFactionsListForReading[i].ideos != null && allFactionsListForReading[i].ideos.IsPrimary(x))
					{
						num++;
					}
				}
				return num;
			});
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x001EDC58 File Offset: 0x001EBE58
		public void IdeoManagerTick()
		{
			for (int i = 0; i < this.ideos.Count; i++)
			{
				this.ideos[i].IdeoTick();
			}
			for (int j = this.toRemove.Count - 1; j >= 0; j--)
			{
				Ideo ideo = this.toRemove[j];
				this.toRemove.RemoveAt(j);
				this.Remove(ideo);
			}
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x001EDCC8 File Offset: 0x001EBEC8
		public void RemoveUnusedStartingIdeos()
		{
			for (int i = this.ideos.Count - 1; i >= 0; i--)
			{
				if (this.CanRemoveIdeo(this.ideos[i]))
				{
					this.Remove(this.ideos[i]);
				}
			}
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x001EDD14 File Offset: 0x001EBF14
		public void Notify_PawnKilled(Pawn pawn)
		{
			this.TryQueueIdeoRemoval(pawn.Ideo);
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x001EDD14 File Offset: 0x001EBF14
		public void Notify_PawnLeftMap(Pawn pawn)
		{
			this.TryQueueIdeoRemoval(pawn.Ideo);
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x001EDD24 File Offset: 0x001EBF24
		public void Notify_FactionRemoved(Faction faction)
		{
			foreach (Ideo ideo in faction.ideos.AllIdeos)
			{
				this.TryQueueIdeoRemoval(ideo);
			}
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x001EDD78 File Offset: 0x001EBF78
		private bool TryQueueIdeoRemoval(Ideo ideo)
		{
			if (ideo == null)
			{
				return false;
			}
			if (!this.CanRemoveIdeo(ideo))
			{
				return false;
			}
			if (!this.toRemove.Contains(ideo))
			{
				this.toRemove.Add(ideo);
			}
			return true;
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x001EDDA8 File Offset: 0x001EBFA8
		private bool CanRemoveIdeo(Ideo ideo)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.ideos != null && faction.ideos.AllIdeos.Contains(ideo))
				{
					return false;
				}
			}
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.ideo != null && pawn.ideo.Ideo == ideo)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x001EDE6C File Offset: 0x001EC06C
		public void Notify_GameStarted()
		{
			foreach (Ideo ideo in this.ideos)
			{
				ideo.Notify_GameStarted();
			}
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x001EDEBC File Offset: 0x001EC0BC
		public void ExposeData()
		{
			Scribe_Collections.Look<Ideo>(ref this.ideos, "ideos", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Ideo>(ref this.toRemove, "toRemove", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ideos.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Some ideoligions were null after loading.");
				}
				if (this.toRemove == null)
				{
					this.toRemove = new List<Ideo>();
				}
			}
		}

		// Token: 0x040034B3 RID: 13491
		private List<Ideo> ideos = new List<Ideo>();

		// Token: 0x040034B4 RID: 13492
		private List<Ideo> toRemove = new List<Ideo>();

		// Token: 0x040034B5 RID: 13493
		private List<LordJob_Ritual> activeRitualsTmp = new List<LordJob_Ritual>();
	}
}
