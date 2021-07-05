using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000125 RID: 293
	public class BattleLog : IExposable
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0002454F File Offset: 0x0002274F
		public List<Battle> Battles
		{
			get
			{
				return this.battles;
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00024558 File Offset: 0x00022758
		public void Add(LogEntry entry)
		{
			Battle battle = null;
			foreach (Thing thing in entry.GetConcerns())
			{
				Battle battleActive = ((Pawn)thing).records.BattleActive;
				if (battle == null)
				{
					battle = battleActive;
				}
				else if (battleActive != null)
				{
					battle = ((battle.Importance > battleActive.Importance) ? battle : battleActive);
				}
			}
			if (battle == null)
			{
				battle = Battle.Create();
				this.battles.Insert(0, battle);
			}
			foreach (Thing thing2 in entry.GetConcerns())
			{
				Pawn pawn = (Pawn)thing2;
				Battle battleActive2 = pawn.records.BattleActive;
				if (battleActive2 != null && battleActive2 != battle)
				{
					battle.Absorb(battleActive2);
					this.battles.Remove(battleActive2);
				}
				pawn.records.EnterBattle(battle);
			}
			battle.Add(entry);
			this.cachedActiveEntries = null;
			this.ReduceToCapacity();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00024664 File Offset: 0x00022864
		private void ReduceToCapacity()
		{
			int num = this.battles.Count((Battle btl) => btl.AbsorbedBy == null);
			while (num > 20 && this.battles[this.battles.Count - 1].LastEntryTimestamp + Mathf.Max(420000, 5000) < Find.TickManager.TicksGame)
			{
				if (this.battles[this.battles.Count - 1].AbsorbedBy == null)
				{
					num--;
				}
				this.battles.RemoveAt(this.battles.Count - 1);
				this.cachedActiveEntries = null;
			}
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002471E File Offset: 0x0002291E
		public void ExposeData()
		{
			Scribe_Collections.Look<Battle>(ref this.battles, "battles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.battles == null)
			{
				this.battles = new List<Battle>();
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00024754 File Offset: 0x00022954
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.battles.Count; i++)
			{
				if (this.battles[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00024790 File Offset: 0x00022990
		public bool IsEntryActive(LogEntry log)
		{
			if (this.cachedActiveEntries == null)
			{
				this.cachedActiveEntries = new HashSet<LogEntry>();
				for (int i = 0; i < this.battles.Count; i++)
				{
					List<LogEntry> entries = this.battles[i].Entries;
					for (int j = 0; j < entries.Count; j++)
					{
						this.cachedActiveEntries.Add(entries[j]);
					}
				}
			}
			return this.cachedActiveEntries.Contains(log);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00024808 File Offset: 0x00022A08
		public void RemoveEntry(LogEntry log)
		{
			int num = 0;
			while (num < this.battles.Count && !this.battles[num].Entries.Remove(log))
			{
				num++;
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002484C File Offset: 0x00022A4C
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.battles.Count - 1; i >= 0; i--)
			{
				this.battles[i].Notify_PawnDiscarded(p, silentlyRemoveReferences);
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x0400078B RID: 1931
		private List<Battle> battles = new List<Battle>();

		// Token: 0x0400078C RID: 1932
		private const int BattleHistoryLength = 20;

		// Token: 0x0400078D RID: 1933
		private HashSet<LogEntry> cachedActiveEntries;
	}
}
