using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RestModels
{
    public class PageJsonConverter : DerivedTypeJsonConverter<Page>
    {
        protected override Type NameToType(string typeName)
        {
            return typeName switch
            {
                nameof(ImageWallPage) => typeof(ImageWallPage),
                nameof(ResumeExpPage) => typeof(ResumeExpPage),
                nameof(ResumeHeadPage) => typeof(ResumeHeadPage),
                nameof(SchoolsPage) => typeof(SchoolsPage),
                nameof(TextBlockPage) => typeof(TextBlockPage),
                _ => typeof(Page)
            };
        }

        protected override string TypeToName(Type type)
        {
            if (type == typeof(ImageWallPage))
                return nameof(ImageWallPage);
            if (type == typeof(ResumeExpPage))
                return nameof(ResumeExpPage);
            if (type == typeof(ResumeHeadPage))
                return nameof(ResumeHeadPage);
            if (type == typeof(SchoolsPage))
                return nameof(SchoolsPage);
            if (type == typeof(TextBlockPage))
                return nameof(TextBlockPage);

            return nameof(Page);
        }
    }
}
