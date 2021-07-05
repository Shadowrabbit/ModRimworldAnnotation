using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010BA RID: 4282
	public class PawnJumper : PawnFlyer
	{
		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x0600664F RID: 26191 RVA: 0x00228B40 File Offset: 0x00226D40
		private Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.def.pawnFlyer.shadow.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.def.pawnFlyer.shadow, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x00228B98 File Offset: 0x00226D98
		static PawnJumper()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.AddKey(0f, 0f);
			animationCurve.AddKey(0.1f, 0.15f);
			animationCurve.AddKey(1f, 1f);
			PawnJumper.FlightSpeed = new Func<float, float>(animationCurve.Evaluate);
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06006651 RID: 26193 RVA: 0x00228BFE File Offset: 0x00226DFE
		public override Vector3 DrawPos
		{
			get
			{
				this.RecomputePosition();
				return this.effectivePos;
			}
		}

		// Token: 0x06006652 RID: 26194 RVA: 0x00228C0C File Offset: 0x00226E0C
		protected override bool ValidateFlyer()
		{
			return ModLister.CheckRoyalty("Jumping");
		}

		// Token: 0x06006653 RID: 26195 RVA: 0x00228C20 File Offset: 0x00226E20
		private void RecomputePosition()
		{
			if (this.positionLastComputedTick == this.ticksFlying)
			{
				return;
			}
			this.positionLastComputedTick = this.ticksFlying;
			float arg = (float)this.ticksFlying / (float)this.ticksFlightTime;
			float num = PawnJumper.FlightSpeed(arg);
			this.effectiveHeight = PawnJumper.FlightCurveHeight(num);
			this.groundPos = Vector3.Lerp(this.startVec, base.DestinationPos, num);
			Vector3 a = new Vector3(0f, 0f, 2f);
			Vector3 b = Altitudes.AltIncVect * this.effectiveHeight;
			Vector3 b2 = a * this.effectiveHeight;
			this.effectivePos = this.groundPos + b + b2;
		}

		// Token: 0x06006654 RID: 26196 RVA: 0x00228CD6 File Offset: 0x00226ED6
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.RecomputePosition();
			this.DrawShadow(this.groundPos, this.effectiveHeight);
			base.FlyingPawn.DrawAt(this.effectivePos, flip);
		}

		// Token: 0x06006655 RID: 26197 RVA: 0x00228D04 File Offset: 0x00226F04
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			Material shadowMaterial = this.ShadowMaterial;
			if (shadowMaterial == null)
			{
				return;
			}
			float num = Mathf.Lerp(1f, 0.6f, height);
			Vector3 s = new Vector3(num, 1f, num);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawLoc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, shadowMaterial, 0);
		}

		// Token: 0x06006656 RID: 26198 RVA: 0x00228D64 File Offset: 0x00226F64
		protected override void RespawnPawn()
		{
			this.LandingEffects();
			base.RespawnPawn();
		}

		// Token: 0x06006657 RID: 26199 RVA: 0x00228D74 File Offset: 0x00226F74
		public override void Tick()
		{
			if (this.flightEffecter == null && this.def.pawnFlyer.flightEffecterDef != null)
			{
				this.flightEffecter = this.def.pawnFlyer.flightEffecterDef.Spawn();
				this.flightEffecter.Trigger(this, TargetInfo.Invalid);
			}
			else
			{
				Effecter effecter = this.flightEffecter;
				if (effecter != null)
				{
					effecter.EffectTick(this, TargetInfo.Invalid);
				}
			}
			base.Tick();
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x00228DF0 File Offset: 0x00226FF0
		private void LandingEffects()
		{
			if (this.def.pawnFlyer.soundLanding != null)
			{
				this.def.pawnFlyer.soundLanding.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			FleckMaker.ThrowDustPuff(base.DestinationPos + Gen.RandomHorizontalVector(0.5f), base.Map, 2f);
		}

		// Token: 0x06006659 RID: 26201 RVA: 0x00228E60 File Offset: 0x00227060
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.flightEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			base.Destroy(mode);
		}

		// Token: 0x040039C1 RID: 14785
		private static readonly Func<float, float> FlightSpeed;

		// Token: 0x040039C2 RID: 14786
		private static readonly Func<float, float> FlightCurveHeight = new Func<float, float>(GenMath.InverseParabola);

		// Token: 0x040039C3 RID: 14787
		private Material cachedShadowMaterial;

		// Token: 0x040039C4 RID: 14788
		private Effecter flightEffecter;

		// Token: 0x040039C5 RID: 14789
		private int positionLastComputedTick = -1;

		// Token: 0x040039C6 RID: 14790
		private Vector3 groundPos;

		// Token: 0x040039C7 RID: 14791
		private Vector3 effectivePos;

		// Token: 0x040039C8 RID: 14792
		private float effectiveHeight;
	}
}
