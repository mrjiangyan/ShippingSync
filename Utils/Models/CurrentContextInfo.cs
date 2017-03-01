using iPms.WebEntity.Security;

namespace iPms.WebUtilities.Models
{
    public class CurrentContextInfo
    {
        private User _currentUser;

     
        public User CurrentUser
        {
            get
            {

                return _currentUser;
            }
            set { _currentUser = value; }
        }

        public long? CurrentOrgId { get; private set; }

        public void SetCurrentOrgId(long orgId)
        {
            CurrentOrgId = orgId;
        }
    }
}