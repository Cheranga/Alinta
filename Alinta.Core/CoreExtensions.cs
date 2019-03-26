namespace Alinta.Core
{
    public static class CoreExtensions
    {
        public static bool Validate(this IValidate validatable)
        {
            if (validatable == null)
            {
                return false;
            }

            return validatable.IsValid();
        }
    }
}