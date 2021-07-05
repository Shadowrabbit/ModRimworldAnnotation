using System;

namespace Verse
{
	// Token: 0x02000360 RID: 864
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06001888 RID: 6280 RVA: 0x000912D8 File Offset: 0x0008F4D8
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x000912E4 File Offset: 0x0008F4E4
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
