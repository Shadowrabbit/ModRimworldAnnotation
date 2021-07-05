using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020008E8 RID: 2280
	public class Pawn_WorkSettings : IExposable
	{
		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06003BAF RID: 15279 RVA: 0x0014C9A0 File Offset: 0x0014ABA0
		public bool EverWork
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x0014C9AB File Offset: 0x0014ABAB
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

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003BB1 RID: 15281 RVA: 0x0014C9C1 File Offset: 0x0014ABC1
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

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0014C9D7 File Offset: 0x0014ABD7
		public Pawn_WorkSettings()
		{
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x0014C9FC File Offset: 0x0014ABFC
		public Pawn_WorkSettings(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0014CA28 File Offset: 0x0014AC28
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

		// Token: 0x06003BB5 RID: 15285 RVA: 0x0014CA85 File Offset: 0x0014AC85
		public void EnableAndInitializeIfNotAlreadyInitialized()
		{
			if (this.priorities == null)
			{
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x0014CA98 File Offset: 0x0014AC98
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

		// Token: 0x06003BB7 RID: 15287 RVA: 0x0014CBD4 File Offset: 0x0014ADD4
		private void ConfirmInitializedDebug()
		{
			if (this.priorities == null)
			{
				Log.Error(this.pawn + " did not have work settings initialized.");
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x0014CBFC File Offset: 0x0014ADFC
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
				}));
				return;
			}
			if (priority < 0 || priority > 4)
			{
				Log.Message("Trying to set work to invalid priority " + priority);
			}
			this.priorities[w] = priority;
			this.workGiversDirty = true;
			if (priority == 0 && this.pawn.jobs != null)
			{
				this.pawn.jobs.Notify_WorkTypeDisabled(w);
			}
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x0014CCA0 File Offset: 0x0014AEA0
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

		// Token: 0x06003BBA RID: 15290 RVA: 0x0014CCD3 File Offset: 0x0014AED3
		public bool WorkIsActive(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			return this.GetPriority(w) > 0;
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0014CCE5 File Offset: 0x0014AEE5
		public void Disable(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			this.SetPriority(w, 0);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0014CCF5 File Offset: 0x0014AEF5
		public void DisableAll()
		{
			this.ConfirmInitializedDebug();
			this.priorities.SetAll(0);
			this.workGiversDirty = true;
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0014CD10 File Offset: 0x0014AF10
		public void Notify_UseWorkPrioritiesChanged()
		{
			this.workGiversDirty = true;
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x0014CD1C File Offset: 0x0014AF1C
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

		// Token: 0x06003BBF RID: 15295 RVA: 0x0014CD60 File Offset: 0x0014AF60
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

		// Token: 0x06003BC0 RID: 15296 RVA: 0x0014CF30 File Offset: 0x0014B130
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

		// Token: 0x06003BC1 RID: 15297 RVA: 0x0014D030 File Offset: 0x0014B230
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

		// Token: 0x04002079 RID: 8313
		private Pawn pawn;

		// Token: 0x0400207A RID: 8314
		private DefMap<WorkTypeDef, int> priorities;

		// Token: 0x0400207B RID: 8315
		private bool workGiversDirty = true;

		// Token: 0x0400207C RID: 8316
		private List<WorkGiver> workGiversInOrderEmerg = new List<WorkGiver>();

		// Token: 0x0400207D RID: 8317
		private List<WorkGiver> workGiversInOrderNormal = new List<WorkGiver>();

		// Token: 0x0400207E RID: 8318
		public const int LowestPriority = 4;

		// Token: 0x0400207F RID: 8319
		public const int DefaultPriority = 3;

		// Token: 0x04002080 RID: 8320
		private const int MaxInitialActiveWorks = 6;

		// Token: 0x04002081 RID: 8321
		private static List<WorkTypeDef> wtsByPrio = new List<WorkTypeDef>();
	}
}
