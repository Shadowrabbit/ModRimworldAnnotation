using System;

namespace Verse
{
	// Token: 0x020004E7 RID: 1255
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06001F59 RID: 8025 RVA: 0x0001BA60 File Offset: 0x00019C60
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000FF920 File Offset: 0x000FDB20
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Terrain(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				")"
			});
		}
	}
}
