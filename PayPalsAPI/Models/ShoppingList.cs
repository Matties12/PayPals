using System;
using System.Collections.Generic;

namespace PayPalsAPI.Models;

public partial class ShoppingList
{
    public int ShoppingListId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? PayerId { get; set; }

    public virtual Person? Payer { get; set; }

    public virtual ICollection<ShoppingListPerson> ShoppingListPeople { get; set; } = new List<ShoppingListPerson>();

    public virtual ICollection<ShoppingListProduct> ShoppingListProducts { get; set; } = new List<ShoppingListProduct>();
}
