using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001125 RID: 4389
	public class CompDryadCocoon : CompDryadHolder
	{
		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06006966 RID: 26982 RVA: 0x00238693 File Offset: 0x00236893
		private Material FrontMat
		{
			get
			{
				if (this.cachedFrontMat == null)
				{
					this.cachedFrontMat = MaterialPool.MatFrom("Things/Building/Misc/DryadSphere/DryadSphereFront", ShaderDatabase.Cutout);
				}
				return this.cachedFrontMat;
			}
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x002386BE File Offset: 0x002368BE
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
				this.tickExpire = Find.TickManager.TicksGame + 600;
			}
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x002386E8 File Offset: 0x002368E8
		public override void TryAcceptPawn(Pawn p)
		{
			base.TryAcceptPawn(p);
			p.Rotation = Rot4.South;
			this.tickComplete = Find.TickManager.TicksGame + (int)(60000f * base.Props.daysToComplete);
			this.tickExpire = -1;
			this.dryadKind = this.tree.TryGetComp<CompTreeConnection>().DryadKind;
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x00238748 File Offset: 0x00236948
		protected override void Complete()
		{
			this.tickComplete = Find.TickManager.TicksGame;
			CompTreeConnection compTreeConnection = this.tree.TryGetComp<CompTreeConnection>();
			if (compTreeConnection != null && this.innerContainer.Count > 0)
			{
				Pawn pawn = (Pawn)this.innerContainer[0];
				long ageBiologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
				compTreeConnection.RemoveDryad(pawn);
				Pawn pawn2 = compTreeConnection.GenerateNewDryad(this.dryadKind);
				pawn2.ageTracker.AgeBiologicalTicks = ageBiologicalTicks;
				if (!pawn.Name.Numerical)
				{
					pawn2.Name = pawn.Name;
				}
				pawn.Destroy(DestroyMode.Vanish);
				this.innerContainer.TryAddOrTransfer(pawn2, 1, true);
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x002387FB File Offset: 0x002369FB
		public override void PostDeSpawn(Map map)
		{
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, delegate(Thing t, int c)
			{
				t.Rotation = Rot4.South;
				SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(this.parent);
			}, null, false);
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x00238824 File Offset: 0x00236A24
		public override void CompTick()
		{
			if (this.dryadKind != null && this.dryadKind != this.tree.TryGetComp<CompTreeConnection>().DryadKind)
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.tickExpire >= 0 && Find.TickManager.TicksGame >= this.tickExpire)
			{
				this.tickExpire = -1;
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.CompTick();
		}

		// Token: 0x0600696C RID: 26988 RVA: 0x00238894 File Offset: 0x00236A94
		public override void PostDraw()
		{
			for (int i = 0; i < this.innerContainer.Count; i++)
			{
				this.innerContainer[i].DrawAt(this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop), false);
			}
			Matrix4x4 matrix = default(Matrix4x4);
			Vector3 pos = this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop.AltitudeFor() + 0.01f);
			Quaternion q = Quaternion.Euler(0f, this.parent.Rotation.AsAngle, 0f);
			Vector3 s = new Vector3(this.parent.Graphic.drawSize.x, 1f, this.parent.Graphic.drawSize.y);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, this.FrontMat, 0);
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x00238984 File Offset: 0x00236B84
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!this.innerContainer.NullOrEmpty<Thing>() && this.dryadKind != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "ChangingDryadIntoType".Translate(this.innerContainer[0].Named("DRYAD"), this.dryadKind.Named("TYPE")).Resolve();
			}
			return text;
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x00238A01 File Offset: 0x00236C01
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.tickExpire, "tickExpire", -1, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.dryadKind, "dryadKind");
		}

		// Token: 0x04003AF3 RID: 15091
		private int tickExpire = -1;

		// Token: 0x04003AF4 RID: 15092
		private Material cachedFrontMat;

		// Token: 0x04003AF5 RID: 15093
		private PawnKindDef dryadKind;

		// Token: 0x04003AF6 RID: 15094
		private const int ExpiryDurationTicks = 600;
	}
}
