using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001717 RID: 5911
	public class PawnJumper : PawnFlyer
	{
		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x06008242 RID: 33346 RVA: 0x00269F98 File Offset: 0x00268198
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

		// Token: 0x06008243 RID: 33347 RVA: 0x00269FF0 File Offset: 0x002681F0
		static PawnJumper()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.AddKey(0f, 0f);
			animationCurve.AddKey(0.1f, 0.15f);
			animationCurve.AddKey(1f, 1f);
			PawnJumper.FlightSpeed = new Func<float, float>(animationCurve.Evaluate);
		}

		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06008244 RID: 33348 RVA: 0x000577B9 File Offset: 0x000559B9
		public override Vector3 DrawPos
		{
			get
			{
				this.RecomputePosition();
				return this.effectivePos;
			}
		}

		// Token: 0x06008245 RID: 33349 RVA: 0x000577C7 File Offset: 0x000559C7
		protected override bool ValidateFlyer()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Items with jump capability are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 550136797, false);
				return false;
			}
			return true;
		}

		// Token: 0x06008246 RID: 33350 RVA: 0x0026A058 File Offset: 0x00268258
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

		// Token: 0x06008247 RID: 33351 RVA: 0x000577E3 File Offset: 0x000559E3
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.RecomputePosition();
			this.DrawShadow(this.groundPos, this.effectiveHeight);
			base.FlyingPawn.DrawAt(this.effectivePos, flip);
		}

		// Token: 0x06008248 RID: 33352 RVA: 0x0026A110 File Offset: 0x00268310
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

		// Token: 0x06008249 RID: 33353 RVA: 0x0005780F File Offset: 0x00055A0F
		protected override void RespawnPawn()
		{
			this.LandingEffects();
			base.RespawnPawn();
		}

		// Token: 0x0600824A RID: 33354 RVA: 0x0026A170 File Offset: 0x00268370
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

		// Token: 0x0600824B RID: 33355 RVA: 0x0026A1EC File Offset: 0x002683EC
		private void LandingEffects()
		{
			if (this.def.pawnFlyer.soundLanding != null)
			{
				this.def.pawnFlyer.soundLanding.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			MoteMaker.ThrowDustPuff(base.DestinationPos + Gen.RandomHorizontalVector(0.5f), base.Map, 2f);
		}

		// Token: 0x0600824C RID: 33356 RVA: 0x0005781D File Offset: 0x00055A1D
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.flightEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			base.Destroy(mode);
		}

		// Token: 0x04005472 RID: 21618
		private static readonly Func<float, float> FlightSpeed;

		// Token: 0x04005473 RID: 21619
		private static readonly Func<float, float> FlightCurveHeight = new Func<float, float>(GenMath.InverseParabola);

		// Token: 0x04005474 RID: 21620
		private Material cachedShadowMaterial;

		// Token: 0x04005475 RID: 21621
		private Effecter flightEffecter;

		// Token: 0x04005476 RID: 21622
		private int positionLastComputedTick = -1;

		// Token: 0x04005477 RID: 21623
		private Vector3 groundPos;

		// Token: 0x04005478 RID: 21624
		private Vector3 effectivePos;

		// Token: 0x04005479 RID: 21625
		private float effectiveHeight;
	}
}
