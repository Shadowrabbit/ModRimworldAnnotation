using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000669 RID: 1641
	public abstract class LordToil
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002E91 RID: 11921 RVA: 0x00116BED File Offset: 0x00114DED
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002E92 RID: 11922 RVA: 0x00116BFF File Offset: 0x00114DFF
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002E93 RID: 11923 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002E94 RID: 11924 RVA: 0x00116C08 File Offset: 0x00114E08
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002E95 RID: 11925 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002E96 RID: 11926 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool AllowAggressiveTargettingOfRoamers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002E97 RID: 11927 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002E98 RID: 11928 RVA: 0x00116C20 File Offset: 0x00114E20
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

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002E99 RID: 11929 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Init()
		{
		}

		// Token: 0x06002E9B RID: 11931
		public abstract void UpdateAllDuties();

		// Token: 0x06002E9C RID: 11932 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void LordToilTick()
		{
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Cleanup()
		{
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawPawnGUIOverlay(Pawn pawn)
		{
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn target, Pawn forPawn)
		{
			return null;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x00116C59 File Offset: 0x00114E59
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			yield break;
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_BuildingSpawnedOnMap(Building b)
		{
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_BuildingDespawnedOnMap(Building b)
		{
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x00116C62 File Offset: 0x00114E62
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x00116C70 File Offset: 0x00114E70
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

		// Token: 0x04001C96 RID: 7318
		public Lord lord;

		// Token: 0x04001C97 RID: 7319
		public LordToilData data;

		// Token: 0x04001C98 RID: 7320
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x04001C99 RID: 7321
		public bool useAvoidGrid;
	}
}
