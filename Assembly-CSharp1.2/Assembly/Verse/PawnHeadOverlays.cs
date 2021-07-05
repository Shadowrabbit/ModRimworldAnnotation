using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037C RID: 892
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x0600166C RID: 5740 RVA: 0x00015DA4 File Offset: 0x00013FA4
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000D6490 File Offset: 0x000D4690
		public void RenderStatusOverlays(Vector3 bodyLoc, Quaternion quat, Mesh headMesh)
		{
			if (!this.pawn.IsColonistPlayerControlled)
			{
				return;
			}
			Vector3 headLoc = bodyLoc + new Vector3(0f, 0f, 0.32f);
			if (this.pawn.needs.mood != null && !this.pawn.Downed && this.pawn.HitPoints > 0)
			{
				if (this.pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
				{
					if (Time.time % 1.2f < 0.4f)
					{
						this.DrawHeadGlow(headLoc, PawnHeadOverlays.MentalStateImminentMat);
						return;
					}
				}
				else if (this.pawn.mindState.mentalBreaker.BreakExtremeIsApproaching && Time.time % 1.2f < 0.4f)
				{
					this.DrawHeadGlow(headLoc, PawnHeadOverlays.UnhappyMat);
				}
			}
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00015DB3 File Offset: 0x00013FB3
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x04001135 RID: 4405
		private Pawn pawn;

		// Token: 0x04001136 RID: 4406
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x04001137 RID: 4407
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x04001138 RID: 4408
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x04001139 RID: 4409
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
