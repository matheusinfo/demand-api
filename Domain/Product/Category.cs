﻿namespace IWantApp.Domain.Product;

public class Category : Entity {
    public string Name { get; set; }
    public bool Active { get; set; } = true;
}
