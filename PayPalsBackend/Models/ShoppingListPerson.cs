using System;
using System.Collections.Generic;

namespace PayPalsBackend.Models;

public partial class ShoppingListPerson
{
    public int ShoppingListPersonId { get; set; }

    public int ShoppingListId { get; set; }

    public int PersonId { get; set; }

    public bool? PaysForAlcohol { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ShoppingList ShoppingList { get; set; } = null!;
}
