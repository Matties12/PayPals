using System;
using System.Collections.Generic;

namespace PayPalsBackend.Models;

public partial class ShoppingListProduct
{
    public int ShoppingListProductId { get; set; }

    public int ShoppingListId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ShoppingList ShoppingList { get; set; } = null!;
}
