using Lumina.Excel.Sheets;

namespace SortaKinda.Classes;

public static class SortingRuleExt
{
    public static int ItemTypeSwap(Item firstItem, Item secondItem)
    {
        switch (firstItem.ItemSortCategory.Value!.Param.CompareTo(secondItem.ItemSortCategory.Value!.Param))
        {
            case < 0: return -1;
            case > 0: return 1;
            default: break;
        }

        switch (ShouldSwapItemUiCategory(firstItem, secondItem))
        {
            case < 0: return -1;
            case > 0: return 1;
            default: break;
        }

        switch (firstItem.Unknown4.CompareTo(secondItem.Unknown4))
        {
            case < 0: return -1;
            case > 0: return 1;
            default: break;
        }

        switch (firstItem.RowId.CompareTo(secondItem.RowId))
        {
            case < 0: return -1;
            case > 0: return 1;
            default: break;
        }
        
        return 0;
    }

    public static int ShouldSwapItemUiCategory(Item firstItem, Item secondItem)
    {
        if (firstItem is { ItemUICategory.Value: { } first } && secondItem is { ItemUICategory.Value: { } second })
        {
            switch (first.OrderMajor.CompareTo(second.OrderMajor))
            {
                case < 0: return -1;
                case > 0: return 1;
                default: break;
            }

            switch (first.OrderMinor.CompareTo(second.OrderMinor))
            {
                case < 0: return -1;
                case > 0: return 1;
                default: break;
            }
        }

        return 0;
    }
}
