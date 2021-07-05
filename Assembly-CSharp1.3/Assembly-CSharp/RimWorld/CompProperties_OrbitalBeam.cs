using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A07 RID: 2567
	public class CompProperties_OrbitalBeam : CompProperties
	{
		// Token: 0x06003EF3 RID: 16115 RVA: 0x00157D27 File Offset: 0x00155F27
		public CompProperties_OrbitalBeam()
		{
			this.compClass = typeof(CompOrbitalBeam);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x00157D55 File Offset: 0x00155F55
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.drawerType != DrawerType.RealtimeOnly && parentDef.drawerType != DrawerType.MapMeshAndRealTime)
			{
				yield return "orbital beam requires realtime drawer";
			}
			yield break;
			yield break;
		}

		// Token: 0x040021E7 RID: 8679
		public float width = 8f;

		// Token: 0x040021E8 RID: 8680
		public Color color = Color.white;

		// Token: 0x040021E9 RID: 8681
		public SoundDef sound;
	}
}
