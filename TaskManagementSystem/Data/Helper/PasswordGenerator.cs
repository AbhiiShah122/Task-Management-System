namespace TaskManagementSystem.Data.Helper
{
    public class PasswordGenerator
    {
        private const string CharPool = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{}|;:,.<>?";

        public string GeneratePassword(int length = 12)
        {
            Random random = new Random();
            char[] password = new char[length];

            for (int i = 0; i < length; i++)
            {
                password[i] = CharPool[random.Next(CharPool.Length)];
            }

            return new string(password);
        }
    }
}
