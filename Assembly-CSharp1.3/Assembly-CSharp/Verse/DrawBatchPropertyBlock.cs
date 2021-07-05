using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000193 RID: 403
	public class DrawBatchPropertyBlock
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x0003E0EC File Offset: 0x0003C2EC
		public void Clear()
		{
			this.properties.Clear();
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0003E0F9 File Offset: 0x0003C2F9
		public void SetFloat(string name, float val)
		{
			this.SetFloat(Shader.PropertyToID(name), val);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0003E108 File Offset: 0x0003C308
		public void SetFloat(int propertyId, float val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Float,
				floatVal = val
			});
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0003E141 File Offset: 0x0003C341
		public void SetColor(string name, Color val)
		{
			this.SetColor(Shader.PropertyToID(name), val);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0003E150 File Offset: 0x0003C350
		public void SetColor(int propertyId, Color val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Color,
				vectorVal = val
			});
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0003E18E File Offset: 0x0003C38E
		public void SetVector(string name, Vector4 val)
		{
			this.SetVector(Shader.PropertyToID(name), val);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
		public void SetVector(int propertyId, Vector4 val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Vector,
				vectorVal = val
			});
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0003E1DC File Offset: 0x0003C3DC
		public void Write(MaterialPropertyBlock propertyBlock)
		{
			foreach (DrawBatchPropertyBlock.Property property in this.properties)
			{
				property.Write(propertyBlock);
			}
		}

		// Token: 0x0400096B RID: 2411
		private List<DrawBatchPropertyBlock.Property> properties = new List<DrawBatchPropertyBlock.Property>();

		// Token: 0x0400096C RID: 2412
		public string leakDebugString;

		// Token: 0x0200195D RID: 6493
		private enum PropertyType
		{
			// Token: 0x04006158 RID: 24920
			Float,
			// Token: 0x04006159 RID: 24921
			Color,
			// Token: 0x0400615A RID: 24922
			Vector
		}

		// Token: 0x0200195E RID: 6494
		private struct Property
		{
			// Token: 0x06009827 RID: 38951 RVA: 0x0035E384 File Offset: 0x0035C584
			public void Write(MaterialPropertyBlock propertyBlock)
			{
				switch (this.type)
				{
				case DrawBatchPropertyBlock.PropertyType.Float:
					propertyBlock.SetFloat(this.propertyId, this.floatVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Color:
					propertyBlock.SetColor(this.propertyId, this.vectorVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Vector:
					propertyBlock.SetVector(this.propertyId, this.vectorVal);
					return;
				default:
					return;
				}
			}

			// Token: 0x0400615B RID: 24923
			public int propertyId;

			// Token: 0x0400615C RID: 24924
			public DrawBatchPropertyBlock.PropertyType type;

			// Token: 0x0400615D RID: 24925
			public float floatVal;

			// Token: 0x0400615E RID: 24926
			public Vector4 vectorVal;
		}
	}
}
