using System;

namespace Verse
{
	// Token: 0x02000197 RID: 407
	public interface IFleck
	{
		// Token: 0x06000B7D RID: 2941
		void Setup(FleckCreationData creationData);

		// Token: 0x06000B7E RID: 2942
		bool TimeInterval(float deltaTime, Map map);

		// Token: 0x06000B7F RID: 2943
		void Draw(DrawBatch batch);
	}
}
