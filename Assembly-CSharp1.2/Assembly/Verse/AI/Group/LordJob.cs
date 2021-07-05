using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AC6 RID: 2758
	public abstract class LordJob : IExposable
	{
		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06004138 RID: 16696 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06004139 RID: 16697 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x0600413A RID: 16698 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x0600413B RID: 16699 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x0600413C RID: 16700 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanBlockHostileVisitors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x0600413D RID: 16701 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AddFleeToil
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x0600413E RID: 16702 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool OrganizerIsStartingPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x0600413F RID: 16703 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06004140 RID: 16704 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool AlwaysShowWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06004141 RID: 16705 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool IsCaravanSendable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06004142 RID: 16706 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool RemoveDownedPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06004143 RID: 16707 RVA: 0x00030B29 File Offset: 0x0002ED29
		protected Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x06004144 RID: 16708
		public abstract StateGraph CreateGraph();

		// Token: 0x06004145 RID: 16709 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void LordJobTick()
		{
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Cleanup()
		{
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_BuildingAdded(Building b)
		{
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string GetReport(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}

		// Token: 0x04002D13 RID: 11539
		public Lord lord;
	}
}
