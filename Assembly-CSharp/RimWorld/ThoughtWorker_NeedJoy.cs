using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7F RID: 3711
	public class ThoughtWorker_NeedJoy : ThoughtWorker
	{
		// Token: 0x06005347 RID: 21319 RVA: 0x001C032C File Offset: 0x001BE52C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.joy == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.joy.CurCategory)
			{
			case JoyCategory.Empty:
				return ThoughtState.ActiveAtStage(0);
			case JoyCategory.VeryLow:
				return ThoughtState.ActiveAtStage(1);
			case JoyCategory.Low:
				return ThoughtState.ActiveAtStage(2);
			case JoyCategory.Satisfied:
				return ThoughtState.Inactive;
			case JoyCategory.High:
				return ThoughtState.ActiveAtStage(3);
			case JoyCategory.Extreme:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
