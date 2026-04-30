namespace Bfar.XCutting.Abstractions.Entities.Dtos
{
    public sealed class MenuDto
    {
        public int MenuId { get; set; }
        public int MethodId { get; set; }
        public string MenuTitle { get; set; }
        public string ItemName { get; set; }
        public string ResourcePath { get; set; }
        public string HostAddress { get; set; }
        public string IconStyle { get; set; }
        public int MenuSortOrder { get; set; }
        public int MenuItemSortOrder { get; set; }

    }
}
