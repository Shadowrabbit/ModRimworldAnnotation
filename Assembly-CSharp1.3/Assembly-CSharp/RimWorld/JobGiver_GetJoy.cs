using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DC RID: 2012
	public class JobGiver_GetJoy : ThinkNode_JobGiver
	{
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003604 RID: 13828 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool CanDoDuringMedicalRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool JoyGiverAllowed(JoyGiverDef def)
		{
			return true;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x00131EF3 File Offset: 0x001300F3
		protected virtual Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJob(pawn);
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x00131F01 File Offset: 0x00130101
		public override void ResolveReferences()
		{
			this.joyGiverChances = new DefMap<JoyGiverDef, float>();
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x00131F10 File Offset: 0x00130110
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!this.CanDoDuringMedicalRest && pawn.InBed() && HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return null;
			}
			if (pawn.needs.joy.CurLevel >= 0.99f)
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
							goto IL_133;
						}
						Rand.PopState();
					}
					float num = tolerances[joyGiverDef.joyKind];
					float num2 = Mathf.Pow(1f - num, 5f);
					num2 = Mathf.Max(0.001f, num2);
					this.joyGiverChances[joyGiverDef] = joyGiverDef.Worker.GetChance(pawn) * num2;
				}
				IL_133:;
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

		// Token: 0x04001ECB RID: 7883
		[Unsaved(false)]
		private DefMap<JoyGiverDef, float> joyGiverChances;

		// Token: 0x04001ECC RID: 7884
		private const float JoyBuffer = 0.99f;
	}
}
