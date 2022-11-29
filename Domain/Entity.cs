using Flunt.Notifications;

namespace IWantApp.Domain;

public abstract class Entity : Notifiable<Notification> {
    public Entity() {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
    }

    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string EditedBy { get; set; }
    public DateTime EditedOn { get; set; }
}
