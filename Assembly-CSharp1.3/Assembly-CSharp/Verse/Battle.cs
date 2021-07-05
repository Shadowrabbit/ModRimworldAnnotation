using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000124 RID: 292
	public class Battle : IExposable, ILoadReferenceable
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00023FE8 File Offset: 0x000221E8
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00023FF5 File Offset: 0x000221F5
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00023FFD File Offset: 0x000221FD
		public int LastEntryTimestamp
		{
			get
			{
				if (this.entries.Count <= 0)
				{
					return 0;
				}
				return this.entries[this.entries.Count - 1].Timestamp;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0002402C File Offset: 0x0002222C
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00024034 File Offset: 0x00022234
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002405A File Offset: 0x0002225A
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00024084 File Offset: 0x00022284
		public string GetName()
		{
			if (this.battleName.NullOrEmpty())
			{
				HashSet<Faction> hashSet = new HashSet<Faction>(from p in this.concerns
				select p.Faction);
				GrammarRequest request = default(GrammarRequest);
				if (this.concerns.Count == 1)
				{
					if (hashSet.Count((Faction f) => f != null) < 2)
					{
						request.Includes.Add(RulePackDefOf.Battle_Solo);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
						goto IL_1D9;
					}
				}
				if (this.concerns.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_Duel);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT2", this.concerns.Last<Pawn>(), null, true, true));
				}
				else if (hashSet.Count == 1)
				{
					request.Includes.Add(RulePackDefOf.Battle_Internal);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
				}
				else if (hashSet.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_War);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION2", hashSet.Last<Faction>(), request.Constants, true));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.Battle_Brawl);
				}
				IL_1D9:
				this.battleName = GrammarResolver.Resolve("r_battlename", request, null, false, null, null, null, true);
			}
			return this.battleName;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00024288 File Offset: 0x00022488
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			foreach (Thing thing in entry.GetConcerns())
			{
				if (thing is Pawn)
				{
					this.concerns.Add(thing as Pawn);
				}
			}
			this.battleName = null;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000242FC File Offset: 0x000224FC
		public void Absorb(Battle battle)
		{
			this.creationTimestamp = Mathf.Min(this.creationTimestamp, battle.creationTimestamp);
			this.entries.AddRange(battle.entries);
			this.concerns.AddRange(battle.concerns);
			this.entries = (from e in this.entries
			orderby e.Age
			select e).ToList<LogEntry>();
			battle.entries.Clear();
			battle.concerns.Clear();
			battle.absorbedBy = this;
			this.battleName = null;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002439B File Offset: 0x0002259B
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000243AC File Offset: 0x000225AC
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			if (!this.concerns.Contains(p))
			{
				return;
			}
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by a battle log entry ",
							this.entries[i],
							"."
						}));
					}
					this.entries.RemoveAt(i);
				}
			}
			this.concerns.Remove(p);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002444C File Offset: 0x0002264C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.creationTimestamp, "creationTimestamp", 0, false);
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.absorbedBy, "absorbedBy", false);
			Scribe_Values.Look<string>(ref this.battleName, "battleName", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.concerns.Clear();
				foreach (Pawn item in this.entries.SelectMany((LogEntry e) => e.GetConcerns()).OfType<Pawn>())
				{
					this.concerns.Add(item);
				}
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00024538 File Offset: 0x00022738
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID;
		}

		// Token: 0x04000784 RID: 1924
		public const int TicksForBattleExit = 5000;

		// Token: 0x04000785 RID: 1925
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000786 RID: 1926
		private string battleName;

		// Token: 0x04000787 RID: 1927
		private Battle absorbedBy;

		// Token: 0x04000788 RID: 1928
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		// Token: 0x04000789 RID: 1929
		private int loadID;

		// Token: 0x0400078A RID: 1930
		private int creationTimestamp;
	}
}
