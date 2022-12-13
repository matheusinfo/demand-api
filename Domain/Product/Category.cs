using Flunt.Validations;

namespace IWantApp.Domain.Product;

public class Category : Entity {
    public string Name { get; private set; }
    public bool Active { get; private set; }

    private void ValidateCategory() {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name");

        AddNotifications(contract);
    }

    public Category(string name, string createdBy, string editedBy) {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;

        ValidateCategory();
    }

    public void EditInfo(string name, bool active, string editedBy) {
        Active = active;
        Name = name;
        EditedBy = editedBy;
        EditedOn = DateTime.Now;

        ValidateCategory();
    }
}
