using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200005A RID: 90
	public abstract class SubEffecter_DrifterEmote : SubEffecter
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x000158D1 File Offset: 0x00013AD1
		public SubEffecter_DrifterEmote(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000158DC File Offset: 0x00013ADC
		protected void MakeMote(TargetInfo A)
		{
			Vector3 vector = A.HasThing ? A.Thing.DrawPos : A.Cell.ToVector3Shifted();
			if (vector.ShouldSpawnMotesAt(A.Map))
			{
				int randomInRange = this.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Mote mote = (Mote)ThingMaker.MakeThing(this.def.moteDef, null);
					mote.Scale = this.def.scale.RandomInRange;
					mote.exactPosition = vector + this.def.positionOffset + Gen.RandomHorizontalVector(this.def.positionRadius);
					mote.rotationRate = this.def.rotationRate.RandomInRange;
					mote.exactRotation = this.def.rotation.RandomInRange;
					MoteThrown moteThrown = mote as MoteThrown;
					if (moteThrown != null)
					{
						moteThrown.airTimeLeft = this.def.airTime.RandomInRange;
						moteThrown.SetVelocity(this.def.angle.RandomInRange, this.def.speed.RandomInRange);
					}
					if (A.HasThing)
					{
						mote.Attach(A);
					}
					GenSpawn.Spawn(mote, vector.ToIntVec3(), A.Map, WipeMode.Vanish);
				}
			}
		}
	}
}
