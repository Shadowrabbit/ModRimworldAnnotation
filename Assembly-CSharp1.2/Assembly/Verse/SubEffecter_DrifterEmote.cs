using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A2 RID: 162
	public abstract class SubEffecter_DrifterEmote : SubEffecter
	{
		// Token: 0x0600055D RID: 1373 RVA: 0x0000A876 File Offset: 0x00008A76
		public SubEffecter_DrifterEmote(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0008B8E8 File Offset: 0x00089AE8
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
