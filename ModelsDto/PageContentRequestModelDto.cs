using System;

namespace DataBaseModels.Shared
{
    public class PageContentRequestModelDto
    {
        public int SkipItems { get; set; }

        public int TakeItems { get; set; }

        public RequestPageContentMode RequestPageContentMode { get; set; }

    }
}
