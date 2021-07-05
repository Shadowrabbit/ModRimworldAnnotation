using System;

namespace Verse
{
	// Token: 0x02000351 RID: 849
	public class Graphic_FleckPulse : Graphic_Fleck
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001821 RID: 6177 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool AllowInstancing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0008F834 File Offset: 0x0008DA34
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, drawData.ageSecs);
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0008F86C File Offset: 0x0008DA6C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Graphic_FleckPulse(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}
