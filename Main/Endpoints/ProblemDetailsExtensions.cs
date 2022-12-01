using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Main.Endpoints;

public static class ProblemDetailsExtensions {
    public static Dictionary<string, string[]> ConvertToProblemDetails(this IReadOnlyCollection<Notification> notifications) {
        return notifications
            .GroupBy(group => group.Key)
            .ToDictionary(group => group.Key, group => group.Select(item => item.Message).ToArray());
    }

    public static Dictionary<string, string[]> ConvertToProblemDetails(this IEnumerable<IdentityError> error) {
        var ditictionary = new Dictionary<string, string[]>();
        ditictionary.Add("Error", error.Select(error => error.Description).ToArray());
        return ditictionary;
    }
}
