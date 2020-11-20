using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;
using Haley.Abstractions;

namespace Haley.Models
{
    public sealed class MappingLoad
    {
        public IMappingProvider provider { get; set; }
        public MappingLevel level { get; set; }
        public InjectionTarget injection { get; set; }

        [HaleyIgnore]
        public MappingLoad(IMappingProvider _provider, MappingLevel _mapping_level = MappingLevel.None, InjectionTarget _injection = InjectionTarget.All)
        {
            provider = _provider;
            level = _mapping_level;
            injection = _injection;
        }
        [HaleyIgnore]
        public MappingLoad()
        {
            level = MappingLevel.None;
            injection = InjectionTarget.All;
        }
    }
}
