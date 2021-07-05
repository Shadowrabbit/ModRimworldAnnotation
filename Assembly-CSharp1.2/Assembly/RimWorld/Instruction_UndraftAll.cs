using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD9 RID: 7129
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x06009CEB RID: 40171 RVA: 0x00068680 File Offset: 0x00066880
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x06009CEC RID: 40172 RVA: 0x000686A6 File Offset: 0x000668A6
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06009CED RID: 40173 RVA: 0x002DF0A8 File Offset: 0x002DD2A8
		public override void LessonUpdate()
		{
			foreach (Pawn pawn in this.DraftedPawns())
			{
				GenDraw.DrawArrowPointingAt(pawn.DrawPos, false);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
