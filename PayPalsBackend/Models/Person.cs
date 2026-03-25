using System;
using System.Collections.Generic;

namespace PayPalsBackend.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ShoppingListPerson> ShoppingListPeople { get; set; } = new List<ShoppingListPerson>();

    public virtual ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
}
