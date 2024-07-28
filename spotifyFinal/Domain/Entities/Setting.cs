using Domain.Common;

namespace Domain.Entities
{
    public class Setting : BaseEntitiy
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
