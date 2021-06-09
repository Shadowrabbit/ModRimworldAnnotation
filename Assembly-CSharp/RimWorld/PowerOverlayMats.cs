using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F6 RID: 4854
	[StaticConstructorOnStartup]
	public static class PowerOverlayMats
	{
		// Token: 0x0600695E RID: 26974 RVA: 0x00207338 File Offset: 0x00205538
		static PowerOverlayMats()
		{
			Graphic graphic = GraphicDatabase.Get<Graphic_Single>("Things/Special/Power/TransmitterAtlas", PowerOverlayMats.TransmitterShader);
			PowerOverlayMats.LinkedOverlayGraphic = GraphicUtility.WrapLinked(graphic, LinkDrawerType.TransmitterOverlay);
			graphic.MatSingle.renderQueue = 3600;
			PowerOverlayMats.MatConnectorBase.renderQueue = 3600;
			PowerOverlayMats.MatConnectorLine.renderQueue = 3600;
		}

		// Token: 0x0400462B RID: 17963
		private const string TransmitterAtlasPath = "Things/Special/Power/TransmitterAtlas";

		// Token: 0x0400462C RID: 17964
		private static readonly Shader TransmitterShader = ShaderDatabase.MetaOverlay;

		// Token: 0x0400462D RID: 17965
		public static readonly Graphic LinkedOverlayGraphic;

		// Token: 0x0400462E RID: 17966
		public static readonly Material MatConnectorBase = MaterialPool.MatFrom("Things/Special/Power/OverlayBase", ShaderDatabase.MetaOverlay);

		// Token: 0x0400462F RID: 17967
		public static readonly Material MatConnectorLine = MaterialPool.MatFrom("Things/Special/Power/OverlayWire", ShaderDatabase.MetaOverlay);

		// Token: 0x04004630 RID: 17968
		public static readonly Material MatConnectorAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayWireAnticipated", ShaderDatabase.MetaOverlay);

		// Token: 0x04004631 RID: 17969
		public static readonly Material MatConnectorBaseAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayBaseAnticipated", ShaderDatabase.MetaOverlay);
	}
}
