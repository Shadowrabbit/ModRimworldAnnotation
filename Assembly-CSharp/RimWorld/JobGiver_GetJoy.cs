using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF6 RID: 3318
	public class JobGiver_GetJoy : ThinkNode_JobGiver
	{
		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected virtual bool CanDoDuringMedicalRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool JoyGiverAllowed(JoyGiverDef def)
		{
			return true;
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x0003634B File Offset: 0x0003454B
		protected virtual Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJob(pawn);
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x00036359 File Offset: 0x00034559
		public override void ResolveReferences()
		{
			this.joyGiverChances = new DefMap<JoyGiverDef, float>();
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x001A9744 File Offset: 0x001A7944
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!this.CanDoDuringMedicalRest && pawn.InBed() && HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return null;
			}
			List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
			JoyToleranceSet tolerances = pawn.needs.joy.tolerances;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyGiverDef joyGiverDef = allDefsListForReading[i];
				this.joyGiverChances[joyGiverDef] = 0f;
				if (this.JoyGiverAllowed(joyGiverDef) && !pawn.needs.joy.tolerances.BoredOf(joyGiverDef.joyKind) && joyGiverDef.Worker.CanBeGivenTo(pawn))
				{
					if (joyGiverDef.pctPawnsEverDo < 1f)
					{
						Rand.PushState(pawn.thingIDNumber ^ 63216713);
						if (Rand.Value >= joyGiverDef.pctPawnsEverDo)
						{
							Rand.PopState();
							goto IL_11A;
						}
						Rand.PopState();
					}
					float num = tolerances[joyGiverDef.joyKind];
					float num2 = Mathf.Pow(1f - num, 5f);
					num2 = Mathf.Max(0.001f, num2);
					this.joyGiverChances[joyGiverDef] = joyGiverDef.Worker.GetChance(pawn) * num2;
				}
				IL_11A:;
			}
			int num3 = 0;
			JoyGiverDef def;
			while (num3 < this.joyGiverChances.Count && allDefsListForReading.TryRandomElementByWeight((JoyGiverDef d) => this.joyGiverChances[d], out def))
			{
				Job job = this.TryGiveJobFromJoyGiverDefDirect(def, pawn);
				if (job != null)
				{
					return job;
				}
				this.joyGiverChances[def] = 0f;
				num3++;
			}
			return null;
		}

		// Token: 0x04003241 RID: 12865
		[Unsaved(false)]
		private DefMap<JoyGiverDef, float> joyGiverChances;
	}
}
