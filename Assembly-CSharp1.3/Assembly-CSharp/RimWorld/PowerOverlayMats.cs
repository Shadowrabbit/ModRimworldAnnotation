using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CD6 RID: 3286
	[StaticConstructorOnStartup]
	public static class PowerOverlayMats
	{
		// Token: 0x06004CB5 RID: 19637 RVA: 0x00199364 File Offset: 0x00197564
		static PowerOverlayMats()
		{
			Graphic graphic = GraphicDatabase.Get<Graphic_Single>("Things/Special/Power/TransmitterAtlas", PowerOverlayMats.TransmitterShader);
			PowerOverlayMats.LinkedOverlayGraphic = GraphicUtility.WrapLinked(graphic, LinkDrawerType.TransmitterOverlay);
			graphic.MatSingle.renderQueue = 3600;
			PowerOverlayMats.MatConnectorBase.renderQueue = 3600;
			PowerOverlayMats.MatConnectorLine.renderQueue = 3600;
		}

		// Token: 0x04002E6D RID: 11885
		private const string TransmitterAtlasPath = "Things/Special/Power/TransmitterAtlas";

		// Token: 0x04002E6E RID: 11886
		private static readonly Shader TransmitterShader = ShaderDatabase.MetaOverlay;

		// Token: 0x04002E6F RID: 11887
		public static readonly Graphic LinkedOverlayGraphic;

		// Token: 0x04002E70 RID: 11888
		public static readonly Material MatConnectorBase = MaterialPool.MatFrom("Things/Special/Power/OverlayBase", ShaderDatabase.MetaOverlay);

		// Token: 0x04002E71 RID: 11889
		public static readonly Material MatConnectorLine = MaterialPool.MatFrom("Things/Special/Power/OverlayWire", ShaderDatabase.MetaOverlay);

		// Token: 0x04002E72 RID: 11890
		public static readonly Material MatConnectorAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayWireAnticipated", ShaderDatabase.MetaOverlay);

		// Token: 0x04002E73 RID: 11891
		public static readonly Material MatConnectorBaseAnticipated = MaterialPool.MatFrom("Things/Special/Power/OverlayBaseAnticipated", ShaderDatabase.MetaOverlay);
	}
}
