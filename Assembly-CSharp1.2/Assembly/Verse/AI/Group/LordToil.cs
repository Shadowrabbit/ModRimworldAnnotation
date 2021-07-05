using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000ACE RID: 2766
	public abstract class LordToil
	{
		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06004172 RID: 16754 RVA: 0x00030CC1 File Offset: 0x0002EEC1
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x00030CD3 File Offset: 0x0002EED3
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06004174 RID: 16756 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06004175 RID: 16757 RVA: 0x00187D8C File Offset: 0x00185F8C
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06004176 RID: 16758 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06004177 RID: 16759 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06004178 RID: 16760 RVA: 0x00187DA4 File Offset: 0x00185FA4
		public virtual bool ShouldFail
		{
			get
			{
				for (int i = 0; i < this.failConditions.Count; i++)
				{
					if (this.failConditions[i]())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06004179 RID: 16761 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Init()
		{
		}

		// Token: 0x0600417B RID: 16763
		public abstract void UpdateAllDuties();

		// Token: 0x0600417C RID: 16764 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void LordToilTick()
		{
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawPawnGUIOverlay(Pawn pawn)
		{
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn target, Pawn forPawn)
		{
			return null;
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x00030CDA File Offset: 0x0002EEDA
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x00187DE0 File Offset: 0x00185FE0
		public override string ToString()
		{
			string text = base.GetType().ToString();
			if (text.Contains('.'))
			{
				text = text.Substring(text.LastIndexOf('.') + 1);
			}
			if (text.Contains('_'))
			{
				text = text.Substring(text.LastIndexOf('_') + 1);
			}
			return text;
		}

		// Token: 0x04002D25 RID: 11557
		public Lord lord;

		// Token: 0x04002D26 RID: 11558
		public LordToilData data;

		// Token: 0x04002D27 RID: 11559
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x04002D28 RID: 11560
		public bool useAvoidGrid;
	}
}
