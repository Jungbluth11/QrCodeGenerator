namespace QrCodeGenerator.Models;

[ContentProperty(Name = "ItemTemplate")]
internal class MenuItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate ItemTemplate { get; set; }
    
    protected override DataTemplate SelectTemplateCore(object item)
    {
        return ItemTemplate;
    }
}
