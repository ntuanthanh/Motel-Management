using MotelManagement.Data.Models;

namespace MotelManagement.Common
{
    public class UserUtil
    {
        public static User getUserFromSession(string json)
        {
            if(json == null)
            {
                return null;
            }
            else
            {
                User user = (User)JsonUtil.DeserializeObject<User>(json);
                return user;
            }
        }
    }
}
