using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE4 RID: 3812
	public class PawnColumnDefgenerator
	{
		// Token: 0x06005453 RID: 21587 RVA: 0x0003A8DB File Offset: 0x00038ADB
		public static IEnumerable<PawnColumnDef> ImpliedPawnColumnDefs()
		{
			PawnTableDef animalsTable = PawnTableDefOf.Animals;
			foreach (TrainableDef trainableDef in from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td)
			{
				PawnColumnDef pawnColumnDef = new PawnColumnDef();
				pawnColumnDef.defName = "Trainable_" + trainableDef.defName;
				pawnColumnDef.trainable = trainableDef;
				pawnColumnDef.headerIcon = trainableDef.icon;
				pawnColumnDef.workerClass = typeof(PawnColumnWorker_Trainable);
				pawnColumnDef.sortable = true;
				pawnColumnDef.headerTip = trainableDef.LabelCap;
				pawnColumnDef.paintable = true;
				pawnColumnDef.modContentPack = trainableDef.modContentPack;
				animalsTable.columns.Insert(animalsTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Checkbox) - 1, pawnColumnDef);
				yield return pawnColumnDef;
			}
			IEnumerator<TrainableDef> enumerator = null;
			PawnTableDef workTable = PawnTableDefOf.Work;
			bool moveWorkTypeLabelDown = false;
			foreach (WorkTypeDef workTypeDef in (from d in WorkTypeDefsUtility.WorkTypeDefsInPriorityOrder
			where d.visible
			select d).Reverse<WorkTypeDef>())
			{
				moveWorkTypeLabelDown = !moveWorkTypeLabelDown;
				PawnColumnDef pawnColumnDef2 = new PawnColumnDef();
				pawnColumnDef2.defName = "WorkPriority_" + workTypeDef.defName;
				pawnColumnDef2.workType = workTypeDef;
				pawnColumnDef2.moveWorkTypeLabelDown = moveWorkTypeLabelDown;
				pawnColumnDef2.workerClass = typeof(PawnColumnWorker_WorkPriority);
				pawnColumnDef2.sortable = true;
				pawnColumnDef2.modContentPack = workTypeDef.modContentPack;
				workTable.columns.Insert(workTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_CopyPasteWorkPriorities) + 1, pawnColumnDef2);
				yield return pawnColumnDef2;
			}
			IEnumerator<WorkTypeDef> enumerator2 = null;
			yield break;
			yield break;
		}
	}
}
