namespace ys.api.properties.Constants;

public static class Messages
{
    public static class Success
    {
        public static string Found(string resourceName) => $"{resourceName} found successfully.";
        public static string Created(string resourceName) => $"{resourceName} created successfully.";
        public static string Updated(string resourceName) => $"{resourceName} updated successfully.";
        public static string Deleted(string resourceName) => $"{resourceName} deleted successfully.";

    }

    public static class Action
    {
        public static string Called(string method, Guid id) => $"{method} called: {id}.";
        // Overloaded method with default ID (optional)
        public static string Called(string method) => $"{method} called.";
    }

    public static class Errors
    {
        public static string NotFound(string resourceName) => $"The requested {resourceName} was not found.";
        public static string ParentNotFound(string parentName, string childName) => $"The requested {parentName} for your ${childName} was not found. Please enter a valid ${parentName}";
        public static string NameExists(string resourceName) => $"A {resourceName} with this Name already exists.";
        public static string ErrorCreatingValue(string resourceName) => $"The requested {resourceName} could not be created.";
    }

    public static class Geo
    {
        public static string InvalidBoundingBox => "Invalid bounding box parameters.";
        public static string InvalidRadius => "Invalid radius parameters.";
        public static string InvalidPolygon => "Invalid polygon coordinates. At least 3 coordinate pairs are required.";
        public static string NoPropertiesInArea => "No properties found in the specified area.";
    }
}
