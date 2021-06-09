using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E35 RID: 3637
	public class Pawn_WorkSettings : IExposable
	{
		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x0600526B RID: 21099 RVA: 0x00039A30 File Offset: 0x00037C30
		public bool EverWork
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x0600526C RID: 21100 RVA: 0x00039A3B File Offset: 0x00037C3B
		public List<WorkGiver> WorkGiversInOrderNormal
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderNormal;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x0600526D RID: 21101 RVA: 0x00039A51 File Offset: 0x00037C51
		public List<WorkGiver> WorkGiversInOrderEmergency
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderEmerg;
			}
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x00039A67 File Offset: 0x00037C67
		public Pawn_WorkSettings()
		{
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x00039A8C File Offset: 0x00037C8C
		public Pawn_WorkSettings(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x001BE430 File Offset: 0x001BC630
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<WorkTypeDef, int>>(ref this.priorities, "priorities", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.priorities != null)
			{
				List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
				for (int i = 0; i < disabledWorkTypes.Count; i++)
				{
					this.Disable(disabledWorkTypes[i]);
				}
			}
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x00039AB8 File Offset: 0x00037CB8
		public void EnableAndInitializeIfNotAlreadyInitialized()
		{
			if (this.priorities == null)
			{
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x001BE490 File Offset: 0x001BC690
		public void EnableAndInitialize()
		{
			if (this.priorities == null)
			{
				this.priorities = new DefMap<WorkTypeDef, int>();
			}
			this.priorities.SetAll(0);
			int num = 0;
			foreach (WorkTypeDef w3 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where !w.alwaysStartActive && !this.pawn.WorkTypeIsDisabled(w)
			orderby this.pawn.skills.AverageOfRelevantSkillsFor(w) descending
			select w)
			{
				this.SetPriority(w3, 3);
				num++;
				if (num >= 6)
				{
					break;
				}
			}
			foreach (WorkTypeDef w2 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where w.alwaysStartActive
			select w)
			{
				if (!this.pawn.WorkTypeIsDisabled(w2))
				{
					this.SetPriority(w2, 3);
				}
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int i = 0; i < disabledWorkTypes.Count; i++)
			{
				this.Disable(disabledWorkTypes[i]);
			}
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x00039AC8 File Offset: 0x00037CC8
		private void ConfirmInitializedDebug()
		{
			if (this.priorities == null)
			{
				Log.Error(this.pawn + " did not have work settings initialized.", false);
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x001BE5CC File Offset: 0x001BC7CC
		public void SetPriority(WorkTypeDef w, int priority)
		{
			this.ConfirmInitializedDebug();
			if (priority != 0 && this.pawn.WorkTypeIsDisabled(w))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to change priority on disabled worktype ",
					w,
					" for pawn ",
					this.pawn
				}), false);
				return;
			}
			if (priority < 0 || priority > 4)
			{
				Log.Message("Trying to set work to invalid priority " + priority, false);
			}
			this.priorities[w] = priority;
			if (priority == 0 && this.pawn.jobs != null)
			{
				this.pawn.jobs.Notify_WorkTypeDisabled(w);
			}
			this.workGiversDirty = true;
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x001BE674 File Offset: 0x001BC874
		public int GetPriority(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			int num = this.priorities[w];
			if (num > 0 && !Find.PlaySettings.useWorkPriorities)
			{
				return 3;
			}
			return num;
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x00039AEE File Offset: 0x00037CEE
		public bool WorkIsActive(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			return this.GetPriority(w) > 0;
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x00039B00 File Offset: 0x00037D00
		public void Disable(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			this.SetPriority(w, 0);
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x00039B10 File Offset: 0x00037D10
		public void DisableAll()
		{
			this.ConfirmInitializedDebug();
			this.priorities.SetAll(0);
			this.workGiversDirty = true;
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x00039B2B File Offset: 0x00037D2B
		public void Notify_UseWorkPrioritiesChanged()
		{
			this.workGiversDirty = true;
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x001BE6A8 File Offset: 0x001BC8A8
		public void Notify_DisabledWorkTypesChanged()
		{
			if (this.priorities == null)
			{
				return;
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int i = 0; i < disabledWorkTypes.Count; i++)
			{
				this.Disable(disabledWorkTypes[i]);
			}
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x001BE6EC File Offset: 0x001BC8EC
		private void CacheWorkGiversInOrder()
		{
			Pawn_WorkSettings.wtsByPrio.Clear();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			int num = 999;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				int priority = this.GetPriority(workTypeDef);
				if (priority > 0)
				{
					if (priority < num)
					{
						if (workTypeDef.workGiversByPriority.Any((WorkGiverDef wg) => !wg.emergency))
						{
							num = priority;
						}
					}
					Pawn_WorkSettings.wtsByPrio.Add(workTypeDef);
				}
			}
			Pawn_WorkSettings.wtsByPrio.InsertionSort(delegate(WorkTypeDef a, WorkTypeDef b)
			{
				float value = (float)(a.naturalPriority + (4 - this.GetPriority(a)) * 100000);
				return ((float)(b.naturalPriority + (4 - this.GetPriority(b)) * 100000)).CompareTo(value);
			});
			this.workGiversInOrderEmerg.Clear();
			for (int j = 0; j < Pawn_WorkSettings.wtsByPrio.Count; j++)
			{
				WorkTypeDef workTypeDef2 = Pawn_WorkSettings.wtsByPrio[j];
				for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
				{
					WorkGiver worker = workTypeDef2.workGiversByPriority[k].Worker;
					if (worker.def.emergency && this.GetPriority(worker.def.workType) <= num)
					{
						this.workGiversInOrderEmerg.Add(worker);
					}
				}
			}
			this.workGiversInOrderNormal.Clear();
			for (int l = 0; l < Pawn_WorkSettings.wtsByPrio.Count; l++)
			{
				WorkTypeDef workTypeDef3 = Pawn_WorkSettings.wtsByPrio[l];
				for (int m = 0; m < workTypeDef3.workGiversByPriority.Count; m++)
				{
					WorkGiver worker2 = workTypeDef3.workGiversByPriority[m].Worker;
					if (!worker2.def.emergency || this.GetPriority(worker2.def.workType) > num)
					{
						this.workGiversInOrderNormal.Add(worker2);
					}
				}
			}
			this.workGiversDirty = false;
		}

		// Token: 0x0600527C RID: 21116 RVA: 0x001BE8BC File Offset: 0x001BCABC
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WorkSettings for " + this.pawn);
			stringBuilder.AppendLine("Cached emergency WorkGivers in order:");
			for (int i = 0; i < this.WorkGiversInOrderEmergency.Count; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					i,
					": ",
					this.DebugStringFor(this.WorkGiversInOrderEmergency[i].def)
				}));
			}
			stringBuilder.AppendLine("Cached normal WorkGivers in order:");
			for (int j = 0; j < this.WorkGiversInOrderNormal.Count; j++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					j,
					": ",
					this.DebugStringFor(this.WorkGiversInOrderNormal[j].def)
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x001BE9BC File Offset: 0x001BCBBC
		private string DebugStringFor(WorkGiverDef wg)
		{
			return string.Concat(new object[]
			{
				"[",
				this.GetPriority(wg.workType),
				" ",
				wg.workType.defName,
				"] - ",
				wg.defName,
				" (",
				wg.priorityInType,
				")"
			});
		}

		// Token: 0x040034D1 RID: 13521
		private Pawn pawn;

		// Token: 0x040034D2 RID: 13522
		private DefMap<WorkTypeDef, int> priorities;

		// Token: 0x040034D3 RID: 13523
		private bool workGiversDirty = true;

		// Token: 0x040034D4 RID: 13524
		private List<WorkGiver> workGiversInOrderEmerg = new List<WorkGiver>();

		// Token: 0x040034D5 RID: 13525
		private List<WorkGiver> workGiversInOrderNormal = new List<WorkGiver>();

		// Token: 0x040034D6 RID: 13526
		public const int LowestPriority = 4;

		// Token: 0x040034D7 RID: 13527
		public const int DefaultPriority = 3;

		// Token: 0x040034D8 RID: 13528
		private const int MaxInitialActiveWorks = 6;

		// Token: 0x040034D9 RID: 13529
		private static List<WorkTypeDef> wtsByPrio = new List<WorkTypeDef>();
	}
}
