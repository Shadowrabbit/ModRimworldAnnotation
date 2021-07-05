using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000258 RID: 600
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x00060F19 File Offset: 0x0005F119
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00060F28 File Offset: 0x0005F128
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

		// Token: 0x0600111B RID: 4379 RVA: 0x00060FF9 File Offset: 0x0005F1F9
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x04000CFA RID: 3322
		private Pawn pawn;

		// Token: 0x04000CFB RID: 3323
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x04000CFC RID: 3324
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x04000CFD RID: 3325
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x04000CFE RID: 3326
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
